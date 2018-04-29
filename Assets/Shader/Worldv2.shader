Shader "Custom/Worldv2" {
	
	Properties {
		
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		
		_MainTex("Top Texture", 2D) = "white" {}
		_MainTexSide("Side/Bottom Texture", 2D) = "white" {}
		_TopRamp("Top Ramp (RGB)", 2D) = "white" {}
		_SideRamp("Side Ramp (RGB)", 2D) = "white" {}
		_Normal("Normal/Noise", 2D) = "bump" {}
		_EdgeNoise("Edge Noise", 2D) = "bump" {}
		
		_Scale("Top Scale", Range(-2,2)) = 1
		_EdgeNoiseScale("Edge Noise Scale", Range(-5,5)) = 1
		_SideScale("Side Scale", Range(-2,2)) = 1
		_NoiseScale("Noise Scale", Range(-2,2)) = 1
		_TopSpread("TopSpread", Range(-2,2)) = 1
		_EdgeWidth("EdgeWidth", Range(0,0.5)) = 1
	}
	SubShader {
				
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTexSide, _MainTex, _Normal, _EdgeNoise, _TopRamp, _SideRamp;
		float _NoiseScale, _Scale, _SideScale, _EdgeNoiseScale;
		float _TopSpread, _EdgeWidth;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
		};

		void surf ( Input IN, inout SurfaceOutputStandard o ) {


			float3 blendNormal = saturate(pow(IN.worldNormal * 1.4,4));

	
			// ************* Get Noise Texture Value *********

			float3 xn = tex2D( _Normal, IN.worldPos.zy * _NoiseScale );
			float3 yn = tex2D( _Normal, IN.worldPos.zx * _NoiseScale );
			float3 zn = tex2D( _Normal, IN.worldPos.xy * _NoiseScale );

			float3 noisetexture = zn;
			noisetexture = lerp( noisetexture, xn, blendNormal.x );
			noisetexture = lerp( noisetexture, yn, blendNormal.y );

			// ************* Get Edge Noise Texture Value *********

			float3 xn2 = tex2D( _EdgeNoise, IN.worldPos.zy * _EdgeNoiseScale );
			float3 yn2 = tex2D( _EdgeNoise, IN.worldPos.zx * _EdgeNoiseScale );
			float3 zn2 = tex2D( _EdgeNoise, IN.worldPos.xy * _EdgeNoiseScale );

			float3 edgeNoiseTexture = zn2;
			edgeNoiseTexture = lerp( edgeNoiseTexture, xn2, blendNormal.x );
			edgeNoiseTexture = lerp( edgeNoiseTexture, yn2, blendNormal.y );
			
			// ************* Get Top Texture Value *********

			float3 xm = tex2D( _MainTex, IN.worldPos.zy * _Scale );
			float3 zm = tex2D( _MainTex, IN.worldPos.xy * _Scale );
			float3 ym = tex2D( _MainTex, IN.worldPos.zx * _Scale );

			float3 toptexture = zm;
			toptexture = lerp( toptexture, xm, blendNormal.x );
			toptexture = lerp( toptexture, ym, blendNormal.y );
			toptexture = toptexture * tex2D ( _TopRamp, IN.uv_MainTex );

			
			// ************* Get Side Texture Value *********

			float3 x = tex2D( _MainTexSide, IN.worldPos.zy * _SideScale );
			float3 y = tex2D( _MainTexSide, IN.worldPos.zx * _SideScale );
			float3 z = tex2D( _MainTexSide, IN.worldPos.xy * _SideScale );

			float3 sidetexture = z;
			sidetexture = lerp(sidetexture, x, blendNormal.x);
			sidetexture = lerp(sidetexture, y, blendNormal.y);
			sidetexture = sidetexture * tex2D ( _SideRamp, IN.uv_MainTex );


			// ******************** Use **********************

			float worldNormalDotNoise = dot(o.Normal + (noisetexture.y + (noisetexture * 0.5)), IN.worldNormal.y);


					// if dot product is in between the two, use edge mask
			float3 topTextureEdgeResult = step( _TopSpread, worldNormalDotNoise ) * step( worldNormalDotNoise, _TopSpread + _EdgeWidth ) * edgeNoiseTexture;

			
				// if ( worldNormalDotNoise > _TopSpread { o.Albedo = toptexture }
			float3 topTextureResult = step( _TopSpread + topTextureEdgeResult, worldNormalDotNoise ) * toptexture;

			
				// if dot product is lower than the top spread slider, multiplied by triplanar mapped side/bottom texture
			float3 sideTextureResult = step(worldNormalDotNoise, _TopSpread + topTextureEdgeResult) * sidetexture;

			
		
			
			o.Albedo = topTextureResult + sideTextureResult;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
