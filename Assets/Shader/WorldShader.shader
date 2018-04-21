Shader "Custom/WorldShader" {
	
	Properties {

		_Color ("Color", Color) = (1,1,1,1)
		_AlternateColor ("Color", Color) = (1,1,1,1)
		_Ramp ("Ramp (RGB)", 2D ) = "white" {}
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_AlternateTex( "Alternate Albedo (RGB)", 2D) = "white" {}
		_AlternateScale ("Alternate Texture Scale", float) = 1
		_NoiseTex ("Noise (RGB)", 2D) = "black" {}
		
		_Scale ("Texture Scale", float) = 1
		_NoiseScale ("Noise Scale", float) = 1
		_NoisePower ("Noise Power", float) = 1
		_NoisePowe2 ("Noise Power2", float) = 4
		_NoiseFromDark("From Dark", float) = 1
	}
	
	SubShader {
		
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		float4 _Color;
		float4 _AlternateColor;
		sampler2D _Ramp;
		sampler2D _MainTex;
		sampler2D _Normal;
		sampler2D _NoiseTex;
		sampler2D _AlternateTex;
		float _Scale;
		float _AlternateScale;

		float _NoiseScale;
		float _NoisePower;
		float _NoisePowe2;
		float _NoiseFromDark;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float3 blendNormal = saturate(pow(IN.worldNormal * 1.4,4));
			fixed4 rampColor = tex2D ( _Ramp, IN.uv_MainTex );


			// ************* Get Main Texture Value *********

			// triplanar for top texture for x, y, z sides
			float3 xm = tex2D( _MainTex, IN.worldPos.zy * _Scale);
			float3 zm = tex2D( _MainTex, IN.worldPos.xy * _Scale);
			float3 ym = tex2D( _MainTex, IN.worldPos.zx * _Scale);

			// lerped together all sides for top texture
			float3 mainTex = zm;
			mainTex = lerp( mainTex, xm, blendNormal.x );
			mainTex = lerp( mainTex, ym, blendNormal.y );
			mainTex = mainTex.r * rampColor * _Color;

			
			// ************* Get Alternate Texture Value *********

			// normal noise triplanar for x, y, z sides
			float3 xa = tex2D( _AlternateTex, IN.worldPos.zy * _AlternateScale );
			float3 ya = tex2D( _AlternateTex, IN.worldPos.zx * _AlternateScale );
			float3 za = tex2D( _AlternateTex, IN.worldPos.xy * _AlternateScale );

			// lerped together all sides for noise texture
			float3 alternateTex = za;
			alternateTex = lerp( alternateTex, xa, blendNormal.x );
			alternateTex = lerp( alternateTex, ya, blendNormal.y );
			alternateTex = alternateTex.r * _AlternateColor;

			// *********** Get Noise Value ***********

			// normal noise triplanar for x, y, z sides
			float3 xn = tex2D( _NoiseTex, IN.worldPos.zy * _NoiseScale );
			float3 yn = tex2D( _NoiseTex, IN.worldPos.zx * _NoiseScale );
			float3 zn = tex2D( _NoiseTex, IN.worldPos.xy * _NoiseScale );

			// lerped together all sides for noise texture
			float3 noiseTex = zn;
			noiseTex = lerp( noiseTex, xn, blendNormal.x );
			noiseTex = lerp( noiseTex, yn, blendNormal.y );
			noiseTex = saturate( pow( noiseTex * _NoisePower, _NoisePowe2 ) );

	
			// **********************
			
			float3 tex = lerp( mainTex, alternateTex, noiseTex );

			o.Albedo = tex;
			//o.Smoothness = tex.g;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
