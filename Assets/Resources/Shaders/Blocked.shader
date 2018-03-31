// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "DFM/Blocked" {
// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Properties {
	_RimColor ("Rim Color", Color) = (0.0,0.0,0.5,0.5)
  _RimPower ("Rim Power", Range(0.2,3.0)) = 0.2
  _CutValue ("Cut Value", Range(0.0,1.0)) = 0.5
}

SubShader {
	Tags { "RenderType"="Opaque" "Queue"="Transparent-20" }			
	Blend SrcAlpha OneMinusSrcAlpha  
	
	Pass {
	
		ZTest greater
		ZWrite off

		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		#include "UnityCG.cginc"		
		
		uniform float4 _RimColor;
  	uniform float _RimPower;
  	uniform float _CutValue;
  
		struct v2f_full
		{
			half4 pos : POSITION;
			half3 norm : TEXCOORD0;
			half3 viewDir : TEXCOORD2;
		};
		
		v2f_full vert (appdata_full v)
		{
			v2f_full o;
			
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			
			half3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			half3 worldNormal = mul((half3x3)unity_ObjectToWorld, v.normal.xyz);
			
			o.norm = worldNormal;
			o.viewDir = (_WorldSpaceCameraPos.xyz - worldPos);		
			
			return o; 
		}
		
		fixed4 frag (v2f_full i) : COLOR 
		{					
			half4 outColor = _RimColor;
  
			float3 N = normalize(i.norm); 
			float3 V = normalize(i.viewDir);
			float rim = 1.0 - saturate(dot(N, V));

			//return outColor * pow(rim, _RimPower);
  		if(rim<_CutValue)
  		  discard;
			return outColor * pow(rim, _RimPower);
		}
		
		ENDCG
	}
}
}
