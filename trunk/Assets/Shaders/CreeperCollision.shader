Shader "Custom/CreeperCollision" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}
	}
	SubShader {
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
		
			CGPROGRAM
			#pragma exclude_renderers gles
			#pragma fragment frag
			#include "UnityCG.cginc"
	
			sampler2D _MainTex;
	
			struct v2f {
				float4 pos : POSITION;
				float4 uv : TEXCOORD0;
			};
	
			half4 frag (v2f i) : COLOR {
				half4 color = tex2D(_MainTex, i.uv.xy);
				return half4(1,0,0,1);
			}
			ENDCG
		}
	} 
	FallBack off
}
