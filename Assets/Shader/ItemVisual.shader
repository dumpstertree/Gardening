// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-3316-RGB,alpha-6714-OUT,voffset-7975-OUT;n:type:ShaderForge.SFN_Fresnel,id:2951,x:32245,y:32869,varname:node_2951,prsc:2|EXP-5497-OUT;n:type:ShaderForge.SFN_Color,id:3316,x:32245,y:32702,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_3316,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:6714,x:32453,y:32869,varname:node_6714,prsc:2|A-2951-OUT,B-9347-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9347,x:32051,y:32983,ptovrint:False,ptlb:BorderStrength,ptin:_BorderStrength,varname:node_9347,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:5497,x:32051,y:32903,ptovrint:False,ptlb:BorderTightness,ptin:_BorderTightness,varname:node_5497,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Time,id:4800,x:31747,y:33050,varname:node_4800,prsc:2;n:type:ShaderForge.SFN_Sin,id:1029,x:32037,y:33058,varname:node_1029,prsc:2|IN-6581-OUT;n:type:ShaderForge.SFN_Abs,id:6728,x:32245,y:33058,varname:node_6728,prsc:2|IN-1029-OUT;n:type:ShaderForge.SFN_Vector1,id:1294,x:32245,y:33208,varname:node_1294,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:7975,x:32425,y:33042,varname:node_7975,prsc:2|A-6728-OUT,B-925-OUT,C-1294-OUT;n:type:ShaderForge.SFN_NormalVector,id:925,x:32425,y:33184,prsc:2,pt:False;n:type:ShaderForge.SFN_ValueProperty,id:5944,x:31747,y:33251,ptovrint:False,ptlb:SpeedMult,ptin:_SpeedMult,varname:node_5944,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:6581,x:31949,y:33217,varname:node_6581,prsc:2|A-4800-T,B-5944-OUT;proporder:3316-9347-5497-5944;pass:END;sub:END;*/

Shader "Shader Forge/ItemVisual" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _BorderStrength ("BorderStrength", Float ) = 1
        _BorderTightness ("BorderTightness", Float ) = 2
        _SpeedMult ("SpeedMult", Float ) = 1
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
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _BorderStrength;
            uniform float _BorderTightness;
            uniform float _SpeedMult;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4800 = _Time + _TimeEditor;
                v.vertex.xyz += (abs(sin((node_4800.g*_SpeedMult)))*v.normal*0.1);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 emissive = _Color.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,(pow(1.0-max(0,dot(normalDirection, viewDirection)),_BorderTightness)*_BorderStrength));
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
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _SpeedMult;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4800 = _Time + _TimeEditor;
                v.vertex.xyz += (abs(sin((node_4800.g*_SpeedMult)))*v.normal*0.1);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
