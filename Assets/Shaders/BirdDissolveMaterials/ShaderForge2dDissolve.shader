// Shader created with Shader Forge v1.38
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:0,bdst:7,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,atwp:True,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-8691-OUT,clip-6589-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32439,y:32644,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,tex:6bd31720de4d4bb4092705429670a04e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:3611,x:31591,y:32621,ptovrint:False,ptlb:DissolveAmount,ptin:_DissolveAmount,varname:node_3611,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.52062,max:1;n:type:ShaderForge.SFN_Color,id:6413,x:33024,y:33457,ptovrint:False,ptlb:EdgeColor,ptin:_EdgeColor,varname:node_6413,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:1799,x:32117,y:33053,ptovrint:False,ptlb:DissolveTexture,ptin:_DissolveTexture,varname:node_1799,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bbdb84bc7bed6304d9823318f71be0df,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Step,id:7041,x:32602,y:33128,varname:node_7041,prsc:2|A-2410-OUT,B-1799-R;n:type:ShaderForge.SFN_Add,id:5789,x:32349,y:33276,varname:node_5789,prsc:2|A-1799-R,B-5299-OUT;n:type:ShaderForge.SFN_Step,id:2074,x:32580,y:33276,varname:node_2074,prsc:2|A-2410-OUT,B-5789-OUT;n:type:ShaderForge.SFN_Subtract,id:9823,x:32797,y:33241,varname:node_9823,prsc:2|A-2074-OUT,B-7041-OUT;n:type:ShaderForge.SFN_Subtract,id:4078,x:33024,y:33272,varname:node_4078,prsc:2|A-4751-OUT,B-9823-OUT;n:type:ShaderForge.SFN_Vector1,id:4751,x:32797,y:33182,varname:node_4751,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:1796,x:33215,y:33284,varname:node_1796,prsc:2|A-4078-OUT,B-6413-RGB;n:type:ShaderForge.SFN_Multiply,id:5880,x:32880,y:32871,varname:node_5880,prsc:2|A-9823-OUT,B-1796-OUT;n:type:ShaderForge.SFN_Add,id:6034,x:32699,y:32722,varname:node_6034,prsc:2|A-4805-RGB,B-8689-OUT;n:type:ShaderForge.SFN_Subtract,id:9699,x:32699,y:32524,varname:node_9699,prsc:2|A-468-OUT,B-8689-OUT;n:type:ShaderForge.SFN_Vector1,id:468,x:32550,y:32434,varname:node_468,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:5116,x:32876,y:32577,varname:node_5116,prsc:2|A-9699-OUT,B-6034-OUT;n:type:ShaderForge.SFN_Add,id:8691,x:33074,y:32651,varname:node_8691,prsc:2|A-5116-OUT,B-5880-OUT;n:type:ShaderForge.SFN_Subtract,id:8689,x:32592,y:33460,varname:node_8689,prsc:2|A-2222-OUT,B-7041-OUT;n:type:ShaderForge.SFN_Vector1,id:2222,x:32349,y:33449,varname:node_2222,prsc:2,v1:1;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:2410,x:32045,y:32744,varname:node_2410,prsc:2|IN-3611-OUT,IMIN-1830-OUT,IMAX-4009-OUT,OMIN-1830-OUT,OMAX-6125-OUT;n:type:ShaderForge.SFN_Vector1,id:4541,x:31535,y:32892,varname:node_4541,prsc:2,v1:1.01;n:type:ShaderForge.SFN_Add,id:6125,x:31786,y:32926,varname:node_6125,prsc:2|A-4541-OUT,B-5299-OUT;n:type:ShaderForge.SFN_Vector1,id:1830,x:31703,y:32717,varname:node_1830,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:4009,x:31703,y:32797,varname:node_4009,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:5299,x:31693,y:33245,ptovrint:False,ptlb:EdgeSize,ptin:_EdgeSize,varname:node_5299,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:6589,x:33022,y:32995,varname:node_6589,prsc:2|A-4805-A,B-2074-OUT;proporder:4805-1799-3611-6413-5299;pass:END;sub:END;*/

Shader "Shader Forge/ShaderForge2dSprite" {
   Properties {
       [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
       _DissolveTexture ("DissolveTexture", 2D) = "black" {}
       _DissolveAmount ("DissolveAmount", Range(0, 1)) = 0.52062
       _EdgeColor ("EdgeColor", Color) = (1,0,0,1)
       _EdgeSize ("EdgeSize", Float ) = 0.1
       [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
       [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
       _Stencil ("Stencil ID", Float) = 0
       _StencilReadMask ("Stencil Read Mask", Float) = 255
       _StencilWriteMask ("Stencil Write Mask", Float) = 255
       _StencilComp ("Stencil Comparison", Float) = 8
       _StencilOp ("Stencil Operation", Float) = 0
       _StencilOpFail ("Stencil Fail Operation", Float) = 0
       _StencilOpZFail ("Stencil Z-Fail Operation", Float) = 0
   }
   SubShader {
       Tags {
           "Queue"="AlphaTest"
           "RenderType"="TransparentCutout"
           "CanUseSpriteAtlas"="True"
           "PreviewType"="Plane"
       }
       Pass {
           Name "FORWARD"
           Tags {
               "LightMode"="ForwardBase"
           }
           Blend One OneMinusSrcAlpha
           Cull Off
          
          
           Stencil {
               Ref [_Stencil]
               ReadMask [_StencilReadMask]
               WriteMask [_StencilWriteMask]
               Comp [_StencilComp]
               Pass [_StencilOp]
               Fail [_StencilOpFail]
               ZFail [_StencilOpZFail]
           }
           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #define UNITY_PASS_FORWARDBASE
           #pragma multi_compile _ PIXELSNAP_ON
           #include "UnityCG.cginc"
           #pragma multi_compile_fwdbase_fullshadows
           #pragma only_renderers d3d9 d3d11 glcore gles
           #pragma target 3.0
           uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
           uniform float _DissolveAmount;
           uniform float4 _EdgeColor;
           uniform sampler2D _DissolveTexture; uniform float4 _DissolveTexture_ST;
           uniform float _EdgeSize;
           struct VertexInput {
               float4 vertex : POSITION;
               float2 texcoord0 : TEXCOORD0;
           };
           struct VertexOutput {
               float4 pos : SV_POSITION;
               float2 uv0 : TEXCOORD0;
           };
           VertexOutput vert (VertexInput v) {
               VertexOutput o = (VertexOutput)0;
               o.uv0 = v.texcoord0;
               o.pos = UnityObjectToClipPos( v.vertex );
               #ifdef PIXELSNAP_ON
                   o.pos = UnityPixelSnap(o.pos);
               #endif
               return o;
           }
           float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
               float isFrontFace = ( facing >= 0 ? 1 : 0 );
               float faceSign = ( facing >= 0 ? 1 : -1 );
               float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
               float node_1830 = 0.0;
               float node_2410 = (node_1830 + ( (_DissolveAmount - node_1830) * ((1.01+_EdgeSize) - node_1830) ) / (1.0 - node_1830));
               float4 _DissolveTexture_var = tex2D(_DissolveTexture,TRANSFORM_TEX(i.uv0, _DissolveTexture));
               float node_2074 = step(node_2410,(_DissolveTexture_var.r+_EdgeSize));
               clip((_MainTex_var.a*node_2074) - 0.5);
////// Lighting:
////// Emissive:
               float node_7041 = step(node_2410,_DissolveTexture_var.r);
               float node_8689 = (1.0-node_7041);
               float node_9823 = (node_2074-node_7041);
               float3 emissive = (((1.0-node_8689)*(_MainTex_var.rgb+node_8689))+(node_9823*((1.0-node_9823)+_EdgeColor.rgb)));
               float3 finalColor = emissive;
               return fixed4(finalColor,1);
           }
           ENDCG
       }
       Pass {
           Name "ShadowCaster"
           Tags {
               "LightMode"="ShadowCaster"
           }
           Offset 1, 1
           Cull Off
          
           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #define UNITY_PASS_SHADOWCASTER
           #pragma multi_compile _ PIXELSNAP_ON
           #include "UnityCG.cginc"
           #include "Lighting.cginc"
           #pragma fragmentoption ARB_precision_hint_fastest
           #pragma multi_compile_shadowcaster
           #pragma only_renderers d3d9 d3d11 glcore gles
           #pragma target 3.0
           uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
           uniform float _DissolveAmount;
           uniform sampler2D _DissolveTexture; uniform float4 _DissolveTexture_ST;
           uniform float _EdgeSize;
           struct VertexInput {
               float4 vertex : POSITION;
               float2 texcoord0 : TEXCOORD0;
           };
           struct VertexOutput {
               V2F_SHADOW_CASTER;
               float2 uv0 : TEXCOORD1;
           };
           VertexOutput vert (VertexInput v) {
               VertexOutput o = (VertexOutput)0;
               o.uv0 = v.texcoord0;
               o.pos = UnityObjectToClipPos( v.vertex );
               #ifdef PIXELSNAP_ON
                   o.pos = UnityPixelSnap(o.pos);
               #endif
               TRANSFER_SHADOW_CASTER(o)
               return o;
           }
           float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
               float isFrontFace = ( facing >= 0 ? 1 : 0 );
               float faceSign = ( facing >= 0 ? 1 : -1 );
               float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
               float node_1830 = 0.0;
               float node_2410 = (node_1830 + ( (_DissolveAmount - node_1830) * ((1.01+_EdgeSize) - node_1830) ) / (1.0 - node_1830));
               float4 _DissolveTexture_var = tex2D(_DissolveTexture,TRANSFORM_TEX(i.uv0, _DissolveTexture));
               float node_2074 = step(node_2410,(_DissolveTexture_var.r+_EdgeSize));
               clip((_MainTex_var.a*node_2074) - 0.5);
               SHADOW_CASTER_FRAGMENT(i)
           }
           ENDCG
       }
   }
   FallBack "Diffuse"
   CustomEditor "ShaderForgeMaterialInspector"
}


