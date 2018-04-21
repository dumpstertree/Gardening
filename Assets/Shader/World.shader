// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33298,y:32511,varname:node_3138,prsc:2|emission-1790-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:5170,x:31562,y:32632,varname:node_5170,prsc:2;n:type:ShaderForge.SFN_Tex2dAsset,id:9132,x:31961,y:32453,ptovrint:False,ptlb:WallTexture,ptin:_WallTexture,varname:node_9132,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:325c89c9066e041a8bb682356f42be16,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:9184,x:32224,y:32519,varname:node_9184,prsc:2|A-1426-B,B-1426-G;n:type:ShaderForge.SFN_Append,id:7520,x:32224,y:32704,varname:node_7520,prsc:2|A-1426-B,B-1426-R;n:type:ShaderForge.SFN_Append,id:4056,x:32224,y:32910,varname:node_4056,prsc:2|A-1426-R,B-1426-G;n:type:ShaderForge.SFN_Lerp,id:5549,x:32761,y:32611,varname:node_5549,prsc:2|A-1774-RGB,B-3231-RGB,T-2653-R;n:type:ShaderForge.SFN_NormalVector,id:4127,x:32173,y:32167,prsc:2,pt:False;n:type:ShaderForge.SFN_Lerp,id:1790,x:33008,y:32710,varname:node_1790,prsc:2|A-5549-OUT,B-2558-RGB,T-2653-G;n:type:ShaderForge.SFN_Tex2d,id:3231,x:32402,y:32519,cmnt:X,varname:node_3231,prsc:2,tex:325c89c9066e041a8bb682356f42be16,ntxv:0,isnm:False|UVIN-9184-OUT,TEX-9132-TEX;n:type:ShaderForge.SFN_Tex2d,id:2558,x:32402,y:32704,cmnt:Y,varname:node_2558,prsc:2,tex:325c89c9066e041a8bb682356f42be16,ntxv:0,isnm:False|UVIN-7520-OUT,TEX-9132-TEX;n:type:ShaderForge.SFN_Tex2d,id:1774,x:32402,y:32910,cmnt:Z,varname:node_1774,prsc:2,tex:325c89c9066e041a8bb682356f42be16,ntxv:0,isnm:False|UVIN-4056-OUT,TEX-9132-TEX;n:type:ShaderForge.SFN_Power,id:8950,x:32561,y:32167,varname:node_8950,prsc:2|VAL-3525-OUT,EXP-7222-OUT;n:type:ShaderForge.SFN_Vector1,id:5423,x:32372,y:32089,varname:node_5423,prsc:2,v1:1.4;n:type:ShaderForge.SFN_ComponentMask,id:2653,x:32998,y:32209,varname:node_2653,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8950-OUT;n:type:ShaderForge.SFN_Vector1,id:7222,x:32561,y:32101,varname:node_7222,prsc:2,v1:4;n:type:ShaderForge.SFN_Multiply,id:3525,x:32372,y:32167,varname:node_3525,prsc:2|A-5423-OUT,B-4127-OUT;n:type:ShaderForge.SFN_Multiply,id:9760,x:31769,y:32680,varname:node_9760,prsc:2|A-5170-XYZ,B-2584-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2584,x:31562,y:32810,ptovrint:False,ptlb:Texture Scale,ptin:_TextureScale,varname:node_2584,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:10;n:type:ShaderForge.SFN_ComponentMask,id:1426,x:31942,y:32680,varname:node_1426,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-9760-OUT;proporder:9132-2584-5048;pass:END;sub:END;*/

Shader "Shader Forge/World" {
    Properties {
        _WallTexture ("WallTexture", 2D) = "white" {}
        _TextureScale ("Texture Scale", Float ) = 10
        _node_5048 ("node_5048", 2D) = "white" {}
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
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _WallTexture; uniform float4 _WallTexture_ST;
            uniform float _TextureScale;
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
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 node_1426 = (i.posWorld.rgb*_TextureScale).rgb;
                float2 node_4056 = float2(node_1426.r,node_1426.g);
                float4 node_1774 = tex2D(_WallTexture,TRANSFORM_TEX(node_4056, _WallTexture)); // Z
                float2 node_9184 = float2(node_1426.b,node_1426.g);
                float4 node_3231 = tex2D(_WallTexture,TRANSFORM_TEX(node_9184, _WallTexture)); // X
                float node_5423 = 1.4;
                float node_7222 = 4.0;
                float3 node_8950 = pow((node_5423*i.normalDir),node_7222);
                float2 node_2653 = node_8950.rg;
                float2 node_7520 = float2(node_1426.b,node_1426.r);
                float4 node_2558 = tex2D(_WallTexture,TRANSFORM_TEX(node_7520, _WallTexture)); // Y
                float3 node_1790 = lerp(lerp(node_1774.rgb,node_3231.rgb,node_2653.r),node_2558.rgb,node_2653.g);
                float3 emissive = node_1790;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
