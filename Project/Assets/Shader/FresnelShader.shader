Shader "Custom/FresnelShader" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Bias ("Bias",	Float) = 0.5
		_Scale ("Scale", Float) = 0.5
		_Power ("Power", Float) = 1.0	
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader 
	{
		Tags 
		{ 
			"RenderType"="Opaque" 
		}
		LOD 200
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _Color; 
			uniform float _Bias;
			uniform float _Scale;
			uniform float _Power;

			struct v2f
			{
				float4 pos		: SV_POSITION;
				fixed4 color	: COLOR;
				float3 normal	: NORMAL;
				float4 uv		: TEXCOORD0;
				float fresnel : FLOAT;
			};

			struct appdata 
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
				
			};

			v2f vert (appdata v)
			{
				v2f outData;

				outData.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				outData.normal = v.normal;
				outData.uv = v.texcoord;

				float3 posWorld = mul(_Object2World, v.vertex).xyz;
				float3 normWorld =  mul(_Object2World, v.normal).xyz;

				float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
				outData.fresnel = _Bias + _Scale * pow(1.0 + dot(I, normWorld), _Power);

				return outData;
			}

			fixed4 frag(v2f inData) : COLOR0
			{	
				float4 col = tex2D(_MainTex, inData.uv);
				return lerp(col, _Color, inData.fresnel);
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
