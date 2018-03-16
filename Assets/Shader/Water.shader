// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:1,fgcg:1,fgcb:0.9686275,fgca:1,fgde:0.04,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|diff-8957-OUT,spec-358-OUT,gloss-1813-OUT,alpha-7215-OUT,voffset-5465-OUT;n:type:ShaderForge.SFN_Color,id:6665,x:32009,y:32321,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.376622,c2:0.9485294,c3:0.6882129,c4:1;n:type:ShaderForge.SFN_Slider,id:358,x:32250,y:32780,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.01063577,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:32250,y:32882,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2d,id:5390,x:31884,y:33129,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_5390,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-7219-OUT;n:type:ShaderForge.SFN_Add,id:7219,x:31679,y:33010,varname:node_7219,prsc:2|A-5398-OUT,B-3202-OUT;n:type:ShaderForge.SFN_Vector1,id:3439,x:31460,y:33201,varname:node_3439,prsc:2,v1:0.01;n:type:ShaderForge.SFN_Time,id:2729,x:31460,y:33269,varname:node_2729,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3202,x:31658,y:33214,varname:node_3202,prsc:2|A-3439-OUT,B-2729-T;n:type:ShaderForge.SFN_Multiply,id:5465,x:32470,y:33136,varname:node_5465,prsc:2|A-9080-OUT,B-7134-OUT;n:type:ShaderForge.SFN_Slider,id:7134,x:32085,y:33348,ptovrint:False,ptlb:Height,ptin:_Height,varname:node_7134,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:5398,x:31467,y:32836,varname:node_5398,prsc:2|A-6669-OUT,B-6951-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6669,x:31467,y:32783,ptovrint:False,ptlb:WaveScale,ptin:_WaveScale,varname:node_6669,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_ValueProperty,id:4786,x:31115,y:32670,ptovrint:False,ptlb:FoamScale,ptin:_FoamScale,varname:node_4786,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:4579,x:31115,y:32728,varname:node_4579,prsc:2|A-4786-OUT,B-6951-OUT;n:type:ShaderForge.SFN_Tex2d,id:6156,x:31298,y:32728,ptovrint:False,ptlb:FoarmNoise,ptin:_FoarmNoise,varname:node_6156,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:19d525f14a95546dca2a618b1b5e4d00,ntxv:2,isnm:False|UVIN-4579-OUT;n:type:ShaderForge.SFN_Vector1,id:9474,x:31529,y:32696,varname:node_9474,prsc:2,v1:2;n:type:ShaderForge.SFN_Add,id:8957,x:32298,y:32465,varname:node_8957,prsc:2|A-6665-RGB,B-562-OUT;n:type:ShaderForge.SFN_Posterize,id:3755,x:31881,y:32445,varname:node_3755,prsc:2|IN-3416-OUT,STPS-9474-OUT;n:type:ShaderForge.SFN_Add,id:3416,x:31298,y:32564,varname:node_3416,prsc:2|A-9098-OUT,B-6156-RGB;n:type:ShaderForge.SFN_Slider,id:9098,x:31092,y:32463,ptovrint:False,ptlb:FoamDensity,ptin:_FoamDensity,varname:node_9098,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-0.2320139,max:1;n:type:ShaderForge.SFN_Clamp01,id:562,x:32086,y:32445,varname:node_562,prsc:2|IN-371-OUT;n:type:ShaderForge.SFN_Append,id:9080,x:32179,y:33156,varname:node_9080,prsc:2|A-5675-OUT,B-5390-R,C-5675-OUT;n:type:ShaderForge.SFN_Vector1,id:5675,x:32179,y:33083,varname:node_5675,prsc:2,v1:0;n:type:ShaderForge.SFN_Subtract,id:371,x:32002,y:32729,varname:node_371,prsc:2|A-3755-OUT,B-5390-RGB;n:type:ShaderForge.SFN_FragmentPosition,id:1416,x:30763,y:32803,varname:node_1416,prsc:2;n:type:ShaderForge.SFN_Append,id:6951,x:30927,y:32803,varname:node_6951,prsc:2|A-1416-X,B-1416-Z;n:type:ShaderForge.SFN_Slider,id:7215,x:32250,y:32974,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_7215,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;proporder:6665-358-1813-5390-7134-6669-4786-6156-9098-7215;pass:END;sub:END;*/

Shader "Shader Forge/Water" {
    Properties {
        _Color ("Color", Color) = (0.376622,0.9485294,0.6882129,1)
        _Metallic ("Metallic", Range(0, 1)) = 0.01063577
        _Gloss ("Gloss", Range(0, 1)) = 1
        _Noise ("Noise", 2D) = "white" {}
        _Height ("Height", Range(0, 1)) = 1
        _WaveScale ("WaveScale", Float ) = 0.5
        _FoamScale ("FoamScale", Float ) = 3
        _FoarmNoise ("FoarmNoise", 2D) = "black" {}
        _FoamDensity ("FoamDensity", Range(-1, 1)) = -0.2320139
        _Opacity ("Opacity", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Height;
            uniform float _WaveScale;
            uniform float _FoamScale;
            uniform sampler2D _FoarmNoise; uniform float4 _FoarmNoise_ST;
            uniform float _FoamDensity;
            uniform float _Opacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                UNITY_FOG_COORDS(6)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD7;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float node_5675 = 0.0;
                float2 node_6951 = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float4 node_2729 = _Time + _TimeEditor;
                float2 node_7219 = ((_WaveScale*node_6951)+(0.01*node_2729.g));
                float4 _Noise_var = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_7219, _Noise),0.0,0));
                v.vertex.xyz += (float3(node_5675,_Noise_var.r,node_5675)*_Height);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float2 node_6951 = float2(i.posWorld.r,i.posWorld.b);
                float2 node_4579 = (_FoamScale*node_6951);
                float4 _FoarmNoise_var = tex2D(_FoarmNoise,TRANSFORM_TEX(node_4579, _FoarmNoise));
                float node_9474 = 2.0;
                float4 node_2729 = _Time + _TimeEditor;
                float2 node_7219 = ((_WaveScale*node_6951)+(0.01*node_2729.g));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_7219, _Noise));
                float3 node_8957 = (_Color.rgb+saturate((floor((_FoamDensity+_FoarmNoise_var.rgb) * node_9474) / (node_9474 - 1)-_Noise_var.rgb)));
                float3 diffuseColor = node_8957; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz)*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,_Opacity);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Height;
            uniform float _WaveScale;
            uniform float _FoamScale;
            uniform sampler2D _FoarmNoise; uniform float4 _FoarmNoise_ST;
            uniform float _FoamDensity;
            uniform float _Opacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float node_5675 = 0.0;
                float2 node_6951 = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float4 node_2729 = _Time + _TimeEditor;
                float2 node_7219 = ((_WaveScale*node_6951)+(0.01*node_2729.g));
                float4 _Noise_var = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_7219, _Noise),0.0,0));
                v.vertex.xyz += (float3(node_5675,_Noise_var.r,node_5675)*_Height);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float2 node_6951 = float2(i.posWorld.r,i.posWorld.b);
                float2 node_4579 = (_FoamScale*node_6951);
                float4 _FoarmNoise_var = tex2D(_FoarmNoise,TRANSFORM_TEX(node_4579, _FoarmNoise));
                float node_9474 = 2.0;
                float4 node_2729 = _Time + _TimeEditor;
                float2 node_7219 = ((_WaveScale*node_6951)+(0.01*node_2729.g));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_7219, _Noise));
                float3 node_8957 = (_Color.rgb+saturate((floor((_FoamDensity+_FoarmNoise_var.rgb) * node_9474) / (node_9474 - 1)-_Noise_var.rgb)));
                float3 diffuseColor = node_8957; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * _Opacity,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Height;
            uniform float _WaveScale;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float node_5675 = 0.0;
                float2 node_6951 = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float4 node_2729 = _Time + _TimeEditor;
                float2 node_7219 = ((_WaveScale*node_6951)+(0.01*node_2729.g));
                float4 _Noise_var = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_7219, _Noise),0.0,0));
                v.vertex.xyz += (float3(node_5675,_Noise_var.r,node_5675)*_Height);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Height;
            uniform float _WaveScale;
            uniform float _FoamScale;
            uniform sampler2D _FoarmNoise; uniform float4 _FoarmNoise_ST;
            uniform float _FoamDensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float node_5675 = 0.0;
                float2 node_6951 = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float4 node_2729 = _Time + _TimeEditor;
                float2 node_7219 = ((_WaveScale*node_6951)+(0.01*node_2729.g));
                float4 _Noise_var = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_7219, _Noise),0.0,0));
                v.vertex.xyz += (float3(node_5675,_Noise_var.r,node_5675)*_Height);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float2 node_6951 = float2(i.posWorld.r,i.posWorld.b);
                float2 node_4579 = (_FoamScale*node_6951);
                float4 _FoarmNoise_var = tex2D(_FoarmNoise,TRANSFORM_TEX(node_4579, _FoarmNoise));
                float node_9474 = 2.0;
                float4 node_2729 = _Time + _TimeEditor;
                float2 node_7219 = ((_WaveScale*node_6951)+(0.01*node_2729.g));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_7219, _Noise));
                float3 node_8957 = (_Color.rgb+saturate((floor((_FoamDensity+_FoarmNoise_var.rgb) * node_9474) / (node_9474 - 1)-_Noise_var.rgb)));
                float3 diffColor = node_8957;
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
                float roughness = 1.0 - _Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
