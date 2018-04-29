Shader "Custom/Vegetation" {
	
	Properties {
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_Noise( "Wind Noise", 2D ) = "white" {}
		_WindStrength( "Wind Strength", float ) = 1
		_Speed( "WindSpeed", float ) = 1
		_Scale( "Wind Scale", float ) = 1
	}
	SubShader {
				
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
		#pragma target 3.0

		float4 _Color;
		sampler2D _Noise;
		float _WindStrength;
		float _Speed;
		float _Scale;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void vert( inout appdata_full v ) {
			
			float4 worldSpacePosition = mul(unity_ObjectToWorld, v.vertex );

		    float4 texOffset = _Time * _Speed;
			
			float4 offsetWoldSpacePosition = worldSpacePosition + texOffset;
			
			float4 noise = ((tex2Dlod ( _Noise, float4( offsetWoldSpacePosition.xz * _Scale ,0, 0 ) )) + 0.5 ) / 2.0;
			
			float4 wind = (1 - v.color.r) * (noise * _WindStrength);

			float4 pos = mul( unity_WorldToObject, worldSpacePosition ) + wind;

			v.vertex = pos;
		}

		void surf ( Input IN, inout SurfaceOutputStandard o ) {
			o.Albedo = _Color;

		}

		ENDCG
	}

	FallBack "Diffuse"
}
