// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "_Custom/sdCustomShader"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
	_DissolveTex("Dissolve Texture", 2D) = "white" {}
	_DissolveAmount("Dissolve Amount", Range(0.0, 1.0)) = 0.0
		_DissolveDepth("Dissolve Depth", Range(0.0, 1.0)) = 0.0
		_DissolveColor("Dissolve Color", Color) = (1, 1, 1, 1)
		_GradientRatio("Gradient Ratio", Range(0.0, 1.0)) = 0.5
	}
		SubShader
	{
		Cull Off

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 mainTex : TEXCOORD0;
		float2 dissolveTex : TEXCOORD1;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float2 mainTex : TEXCOORD0;
		float2 dissolveTex : TEXCOORD1;
	};

	sampler2D _MainTex;
	sampler2D _DissolveTex;
	float4 _MainTex_ST;
	float4 _DissolveTex_ST;
	float4 _DissolveColor;
	float _DissolveAmount;
	float _DissolveDepth;
	float _NormalAmount;
	float _GradientRatio;

	v2f vert(appdata IN)
	{
		v2f OUT;

		OUT.mainTex = TRANSFORM_TEX(IN.mainTex, _MainTex);
		OUT.dissolveTex = TRANSFORM_TEX(IN.dissolveTex, _DissolveTex);
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		return OUT;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 mainTex = tex2D(_MainTex, IN.mainTex);
	if (_DissolveAmount <= 0.0)
		return mainTex;

	fixed4 dissolveTex = tex2D(_DissolveTex, IN.dissolveTex);



	fixed3 dissolveClip = dissolveTex.rgb - _DissolveAmount;
	clip(dissolveClip);

	float dissolveClipAve = (dissolveClip.r + dissolveClip.g + dissolveClip.b) * 0.333333333333;
	if (dissolveClipAve > 0.0 && dissolveClipAve < _DissolveDepth) {
		float u = dissolveClipAve / _DissolveDepth;
		float _u = 1.0 - u;
		fixed3 interpolationRGB = _DissolveColor*_GradientRatio + mainTex.rgb*(1.0 - _GradientRatio);
		fixed3 _1 = mainTex.rgb*u + interpolationRGB*_u;
		fixed3 _2 = interpolationRGB*u + _DissolveColor*_u;
		_1 = _1*u + _2*_u;
		mainTex.rgb = _1;
	}

	return mainTex;
	}
		ENDCG
	}
	}
}
