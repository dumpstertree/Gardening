// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:1,fgcg:0.7380325,fgcb:0.5367647,fgca:1,fgde:0.02,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:32891,y:32858,varname:node_2865,prsc:2|diff-4197-OUT,spec-8704-OUT,gloss-7473-OUT,voffset-6509-OUT;n:type:ShaderForge.SFN_Tex2d,id:2385,x:31720,y:32968,ptovrint:False,ptlb:Grass,ptin:_Grass,varname:node_2385,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3475-OUT;n:type:ShaderForge.SFN_Tex2d,id:3426,x:31717,y:33201,ptovrint:False,ptlb:Sand,ptin:_Sand,varname:node_3426,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3475-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:2257,x:30211,y:33298,varname:node_2257,prsc:2;n:type:ShaderForge.SFN_Append,id:2643,x:30398,y:33308,varname:node_2643,prsc:2|A-2257-X,B-2257-Z;n:type:ShaderForge.SFN_Lerp,id:5085,x:32115,y:32967,varname:node_5085,prsc:2|A-2053-OUT,B-4361-OUT,T-3611-R;n:type:ShaderForge.SFN_VertexColor,id:5576,x:31510,y:33388,varname:node_5576,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3475,x:30640,y:33308,varname:node_3475,prsc:2|A-2643-OUT,B-2164-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2164,x:30598,y:33468,ptovrint:False,ptlb:Tiling,ptin:_Tiling,varname:node_2164,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:4021,x:31565,y:33851,ptovrint:False,ptlb:EdgeMask,ptin:_EdgeMask,varname:node_4021,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:93802eaaab74040ac9c29ff3c537be2b,ntxv:0,isnm:False|UVIN-9129-OUT;n:type:ShaderForge.SFN_Multiply,id:9129,x:31364,y:33851,varname:node_9129,prsc:2|A-2643-OUT,B-2856-OUT;n:type:ShaderForge.SFN_Round,id:1003,x:32586,y:33769,varname:node_1003,prsc:2|IN-9358-OUT;n:type:ShaderForge.SFN_RemapRange,id:5428,x:31723,y:33405,varname:node_5428,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-5576-RGB;n:type:ShaderForge.SFN_Add,id:160,x:32145,y:33768,varname:node_160,prsc:2|A-5428-OUT,B-8382-OUT,C-3689-OUT;n:type:ShaderForge.SFN_Lerp,id:859,x:32396,y:32900,varname:node_859,prsc:2|A-5085-OUT,B-7365-OUT,T-3611-B;n:type:ShaderForge.SFN_Tex2d,id:4962,x:31720,y:32744,ptovrint:False,ptlb:Dirt,ptin:_Dirt,varname:node_4962,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3475-OUT;n:type:ShaderForge.SFN_ComponentMask,id:3611,x:32764,y:33769,varname:node_3611,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-1003-OUT;n:type:ShaderForge.SFN_Multiply,id:2053,x:31899,y:32936,varname:node_2053,prsc:2|A-2385-RGB,B-36-RGB;n:type:ShaderForge.SFN_Color,id:36,x:31557,y:32968,ptovrint:False,ptlb:GrassTint,ptin:_GrassTint,varname:node_36,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.585532,c2:0.8382353,c3:0.6081882,c4:1;n:type:ShaderForge.SFN_Color,id:4428,x:31558,y:33201,ptovrint:False,ptlb:SandTint,ptin:_SandTint,varname:node_4428,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9264706,c2:0.8975301,c3:0.7766004,c4:1;n:type:ShaderForge.SFN_Multiply,id:4361,x:31881,y:33200,varname:node_4361,prsc:2|A-4428-RGB,B-3426-RGB;n:type:ShaderForge.SFN_Color,id:1721,x:31545,y:32744,ptovrint:False,ptlb:DirtTint,ptin:_DirtTint,varname:node_1721,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4044118,c2:0.3931326,c3:0.3300714,c4:1;n:type:ShaderForge.SFN_Multiply,id:7365,x:31884,y:32744,varname:node_7365,prsc:2|A-4962-RGB,B-1721-RGB;n:type:ShaderForge.SFN_Tex2d,id:4120,x:31579,y:34129,ptovrint:False,ptlb:PatchMask,ptin:_PatchMask,varname:node_4120,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-5815-OUT;n:type:ShaderForge.SFN_Multiply,id:5815,x:31364,y:34129,varname:node_5815,prsc:2|A-2643-OUT,B-7750-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7750,x:31364,y:34056,ptovrint:False,ptlb:PatchesTiling,ptin:_PatchesTiling,varname:node_7750,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_RemapRange,id:9358,x:32324,y:33768,varname:node_9358,prsc:2,frmn:0,frmx:2,tomn:0,tomx:1|IN-160-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4796,x:31791,y:34282,ptovrint:False,ptlb:PatchSharpness,ptin:_PatchSharpness,varname:node_4796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2856,x:31364,y:33787,ptovrint:False,ptlb:EdgeMaskTiling,ptin:_EdgeMaskTiling,varname:node_2856,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3689,x:31791,y:34129,varname:node_3689,prsc:2|A-4120-RGB,B-4796-OUT;n:type:ShaderForge.SFN_Multiply,id:8382,x:31787,y:33851,varname:node_8382,prsc:2|A-4021-RGB,B-1808-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1808,x:31778,y:33996,ptovrint:False,ptlb:Edge Sharpness,ptin:_EdgeSharpness,varname:node_1808,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_If,id:2609,x:32335,y:33375,varname:node_2609,prsc:2|A-8362-OUT,B-3346-OUT,GT-3346-OUT,EQ-2562-OUT,LT-2562-OUT;n:type:ShaderForge.SFN_Vector1,id:3346,x:31987,y:33419,varname:node_3346,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:2562,x:31987,y:33355,varname:node_2562,prsc:2,v1:0;n:type:ShaderForge.SFN_Add,id:4197,x:32624,y:32900,varname:node_4197,prsc:2|A-859-OUT,B-2609-OUT;n:type:ShaderForge.SFN_Posterize,id:8362,x:32335,y:33516,varname:node_8362,prsc:2|IN-6207-OUT,STPS-2018-OUT;n:type:ShaderForge.SFN_Vector1,id:2018,x:32130,y:33550,varname:node_2018,prsc:2,v1:3;n:type:ShaderForge.SFN_If,id:2976,x:32335,y:33247,varname:node_2976,prsc:2|A-8362-OUT,B-3346-OUT,GT-2562-OUT,EQ-2562-OUT,LT-3346-OUT;n:type:ShaderForge.SFN_ComponentMask,id:6207,x:32593,y:33631,varname:node_6207,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-9358-OUT;n:type:ShaderForge.SFN_Multiply,id:9222,x:32567,y:33285,varname:node_9222,prsc:2|A-2976-OUT,B-2609-OUT;n:type:ShaderForge.SFN_NormalVector,id:3505,x:32362,y:34114,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:5563,x:32573,y:34124,varname:node_5563,prsc:2|A-3505-OUT,B-7891-RGB;n:type:ShaderForge.SFN_Tex2d,id:7891,x:32362,y:34305,ptovrint:False,ptlb:node_7891,ptin:_node_7891,varname:node_7891,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6982-OUT;n:type:ShaderForge.SFN_Multiply,id:6982,x:32078,y:34383,varname:node_6982,prsc:2|A-2643-OUT,B-178-OUT;n:type:ShaderForge.SFN_ValueProperty,id:178,x:32042,y:34574,ptovrint:False,ptlb:VertexNoiseTiling,ptin:_VertexNoiseTiling,varname:node_178,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:779,x:32753,y:34155,varname:node_779,prsc:2|A-5563-OUT,B-4325-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4325,x:32714,y:34484,ptovrint:False,ptlb:VertexHeight,ptin:_VertexHeight,varname:node_4325,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Subtract,id:6509,x:33057,y:33998,varname:node_6509,prsc:2|A-779-OUT,B-4325-OUT;n:type:ShaderForge.SFN_Slider,id:7473,x:32396,y:32706,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:node_7473,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:8704,x:32396,y:32607,ptovrint:False,ptlb:Metalness,ptin:_Metalness,varname:_Roughness_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:2385-3426-4962-36-4428-1721-4021-4120-7750-2164-4796-2856-1808-7891-178-4325-7473-8704;pass:END;sub:END;*/

Shader "Shader Forge/GroundShader" {
    Properties {
        _Grass ("Grass", 2D) = "white" {}
        _Sand ("Sand", 2D) = "white" {}
        _Dirt ("Dirt", 2D) = "white" {}
        _GrassTint ("GrassTint", Color) = (0.585532,0.8382353,0.6081882,1)
        _SandTint ("SandTint", Color) = (0.9264706,0.8975301,0.7766004,1)
        _DirtTint ("DirtTint", Color) = (0.4044118,0.3931326,0.3300714,1)
        _EdgeMask ("EdgeMask", 2D) = "white" {}
        _PatchMask ("PatchMask", 2D) = "white" {}
        _PatchesTiling ("PatchesTiling", Float ) = 1
        _Tiling ("Tiling", Float ) = 1
        _PatchSharpness ("PatchSharpness", Float ) = 1
        _EdgeMaskTiling ("EdgeMaskTiling", Float ) = 1
        _EdgeSharpness ("Edge Sharpness", Float ) = 1
        _node_7891 ("node_7891", 2D) = "white" {}
        _VertexNoiseTiling ("VertexNoiseTiling", Float ) = 1
        _VertexHeight ("VertexHeight", Float ) = 0
        _Roughness ("Roughness", Range(0, 1)) = 0
        _Metalness ("Metalness", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _Grass; uniform float4 _Grass_ST;
            uniform sampler2D _Sand; uniform float4 _Sand_ST;
            uniform float _Tiling;
            uniform sampler2D _EdgeMask; uniform float4 _EdgeMask_ST;
            uniform sampler2D _Dirt; uniform float4 _Dirt_ST;
            uniform float4 _GrassTint;
            uniform float4 _SandTint;
            uniform float4 _DirtTint;
            uniform sampler2D _PatchMask; uniform float4 _PatchMask_ST;
            uniform float _PatchesTiling;
            uniform float _PatchSharpness;
            uniform float _EdgeMaskTiling;
            uniform float _EdgeSharpness;
            uniform sampler2D _node_7891; uniform float4 _node_7891_ST;
            uniform float _VertexNoiseTiling;
            uniform float _VertexHeight;
            uniform float _Roughness;
            uniform float _Metalness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD9;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
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
                float2 node_2643 = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_6982 = (node_2643*_VertexNoiseTiling);
                float4 _node_7891_var = tex2Dlod(_node_7891,float4(TRANSFORM_TEX(node_6982, _node_7891),0.0,0));
                v.vertex.xyz += (((v.normal*_node_7891_var.rgb)*_VertexHeight)-_VertexHeight);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
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
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - _Roughness; // Convert roughness to gloss
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
                float3 specularColor = _Metalness;
                float specularMonochrome;
                float2 node_2643 = float2(i.posWorld.r,i.posWorld.b);
                float2 node_3475 = (node_2643*_Tiling);
                float4 _Grass_var = tex2D(_Grass,TRANSFORM_TEX(node_3475, _Grass));
                float4 _Sand_var = tex2D(_Sand,TRANSFORM_TEX(node_3475, _Sand));
                float2 node_9129 = (node_2643*_EdgeMaskTiling);
                float4 _EdgeMask_var = tex2D(_EdgeMask,TRANSFORM_TEX(node_9129, _EdgeMask));
                float2 node_5815 = (node_2643*_PatchesTiling);
                float4 _PatchMask_var = tex2D(_PatchMask,TRANSFORM_TEX(node_5815, _PatchMask));
                float3 node_9358 = (((i.vertexColor.rgb*2.0+-1.0)+(_EdgeMask_var.rgb*_EdgeSharpness)+(_PatchMask_var.rgb*_PatchSharpness))*0.5+0.0);
                float3 node_3611 = round(node_9358).rgb;
                float4 _Dirt_var = tex2D(_Dirt,TRANSFORM_TEX(node_3475, _Dirt));
                float node_2018 = 3.0;
                float node_8362 = floor(node_9358.r * node_2018) / (node_2018 - 1);
                float node_3346 = 1.0;
                float node_2609_if_leA = step(node_8362,node_3346);
                float node_2609_if_leB = step(node_3346,node_8362);
                float node_2562 = 0.0;
                float node_2609 = lerp((node_2609_if_leA*node_2562)+(node_2609_if_leB*node_3346),node_2562,node_2609_if_leA*node_2609_if_leB);
                float3 diffuseColor = (lerp(lerp((_Grass_var.rgb*_GrassTint.rgb),(_SandTint.rgb*_Sand_var.rgb),node_3611.r),(_Dirt_var.rgb*_DirtTint.rgb),node_3611.b)+node_2609); // Need this for specular when using metallic
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
                fixed4 finalRGBA = fixed4(finalColor,1);
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
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _Grass; uniform float4 _Grass_ST;
            uniform sampler2D _Sand; uniform float4 _Sand_ST;
            uniform float _Tiling;
            uniform sampler2D _EdgeMask; uniform float4 _EdgeMask_ST;
            uniform sampler2D _Dirt; uniform float4 _Dirt_ST;
            uniform float4 _GrassTint;
            uniform float4 _SandTint;
            uniform float4 _DirtTint;
            uniform sampler2D _PatchMask; uniform float4 _PatchMask_ST;
            uniform float _PatchesTiling;
            uniform float _PatchSharpness;
            uniform float _EdgeMaskTiling;
            uniform float _EdgeSharpness;
            uniform sampler2D _node_7891; uniform float4 _node_7891_ST;
            uniform float _VertexNoiseTiling;
            uniform float _VertexHeight;
            uniform float _Roughness;
            uniform float _Metalness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float2 node_2643 = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_6982 = (node_2643*_VertexNoiseTiling);
                float4 _node_7891_var = tex2Dlod(_node_7891,float4(TRANSFORM_TEX(node_6982, _node_7891),0.0,0));
                v.vertex.xyz += (((v.normal*_node_7891_var.rgb)*_VertexHeight)-_VertexHeight);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
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
                float gloss = 1.0 - _Roughness; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metalness;
                float specularMonochrome;
                float2 node_2643 = float2(i.posWorld.r,i.posWorld.b);
                float2 node_3475 = (node_2643*_Tiling);
                float4 _Grass_var = tex2D(_Grass,TRANSFORM_TEX(node_3475, _Grass));
                float4 _Sand_var = tex2D(_Sand,TRANSFORM_TEX(node_3475, _Sand));
                float2 node_9129 = (node_2643*_EdgeMaskTiling);
                float4 _EdgeMask_var = tex2D(_EdgeMask,TRANSFORM_TEX(node_9129, _EdgeMask));
                float2 node_5815 = (node_2643*_PatchesTiling);
                float4 _PatchMask_var = tex2D(_PatchMask,TRANSFORM_TEX(node_5815, _PatchMask));
                float3 node_9358 = (((i.vertexColor.rgb*2.0+-1.0)+(_EdgeMask_var.rgb*_EdgeSharpness)+(_PatchMask_var.rgb*_PatchSharpness))*0.5+0.0);
                float3 node_3611 = round(node_9358).rgb;
                float4 _Dirt_var = tex2D(_Dirt,TRANSFORM_TEX(node_3475, _Dirt));
                float node_2018 = 3.0;
                float node_8362 = floor(node_9358.r * node_2018) / (node_2018 - 1);
                float node_3346 = 1.0;
                float node_2609_if_leA = step(node_8362,node_3346);
                float node_2609_if_leB = step(node_3346,node_8362);
                float node_2562 = 0.0;
                float node_2609 = lerp((node_2609_if_leA*node_2562)+(node_2609_if_leB*node_3346),node_2562,node_2609_if_leA*node_2609_if_leB);
                float3 diffuseColor = (lerp(lerp((_Grass_var.rgb*_GrassTint.rgb),(_SandTint.rgb*_Sand_var.rgb),node_3611.r),(_Dirt_var.rgb*_DirtTint.rgb),node_3611.b)+node_2609); // Need this for specular when using metallic
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
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
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
            uniform sampler2D _node_7891; uniform float4 _node_7891_ST;
            uniform float _VertexNoiseTiling;
            uniform float _VertexHeight;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float2 node_2643 = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_6982 = (node_2643*_VertexNoiseTiling);
                float4 _node_7891_var = tex2Dlod(_node_7891,float4(TRANSFORM_TEX(node_6982, _node_7891),0.0,0));
                v.vertex.xyz += (((v.normal*_node_7891_var.rgb)*_VertexHeight)-_VertexHeight);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
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
            uniform sampler2D _Grass; uniform float4 _Grass_ST;
            uniform sampler2D _Sand; uniform float4 _Sand_ST;
            uniform float _Tiling;
            uniform sampler2D _EdgeMask; uniform float4 _EdgeMask_ST;
            uniform sampler2D _Dirt; uniform float4 _Dirt_ST;
            uniform float4 _GrassTint;
            uniform float4 _SandTint;
            uniform float4 _DirtTint;
            uniform sampler2D _PatchMask; uniform float4 _PatchMask_ST;
            uniform float _PatchesTiling;
            uniform float _PatchSharpness;
            uniform float _EdgeMaskTiling;
            uniform float _EdgeSharpness;
            uniform sampler2D _node_7891; uniform float4 _node_7891_ST;
            uniform float _VertexNoiseTiling;
            uniform float _VertexHeight;
            uniform float _Roughness;
            uniform float _Metalness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float2 node_2643 = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_6982 = (node_2643*_VertexNoiseTiling);
                float4 _node_7891_var = tex2Dlod(_node_7891,float4(TRANSFORM_TEX(node_6982, _node_7891),0.0,0));
                v.vertex.xyz += (((v.normal*_node_7891_var.rgb)*_VertexHeight)-_VertexHeight);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float2 node_2643 = float2(i.posWorld.r,i.posWorld.b);
                float2 node_3475 = (node_2643*_Tiling);
                float4 _Grass_var = tex2D(_Grass,TRANSFORM_TEX(node_3475, _Grass));
                float4 _Sand_var = tex2D(_Sand,TRANSFORM_TEX(node_3475, _Sand));
                float2 node_9129 = (node_2643*_EdgeMaskTiling);
                float4 _EdgeMask_var = tex2D(_EdgeMask,TRANSFORM_TEX(node_9129, _EdgeMask));
                float2 node_5815 = (node_2643*_PatchesTiling);
                float4 _PatchMask_var = tex2D(_PatchMask,TRANSFORM_TEX(node_5815, _PatchMask));
                float3 node_9358 = (((i.vertexColor.rgb*2.0+-1.0)+(_EdgeMask_var.rgb*_EdgeSharpness)+(_PatchMask_var.rgb*_PatchSharpness))*0.5+0.0);
                float3 node_3611 = round(node_9358).rgb;
                float4 _Dirt_var = tex2D(_Dirt,TRANSFORM_TEX(node_3475, _Dirt));
                float node_2018 = 3.0;
                float node_8362 = floor(node_9358.r * node_2018) / (node_2018 - 1);
                float node_3346 = 1.0;
                float node_2609_if_leA = step(node_8362,node_3346);
                float node_2609_if_leB = step(node_3346,node_8362);
                float node_2562 = 0.0;
                float node_2609 = lerp((node_2609_if_leA*node_2562)+(node_2609_if_leB*node_3346),node_2562,node_2609_if_leA*node_2609_if_leB);
                float3 diffColor = (lerp(lerp((_Grass_var.rgb*_GrassTint.rgb),(_SandTint.rgb*_Sand_var.rgb),node_3611.r),(_Dirt_var.rgb*_DirtTint.rgb),node_3611.b)+node_2609);
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metalness, specColor, specularMonochrome );
                float roughness = _Roughness;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
