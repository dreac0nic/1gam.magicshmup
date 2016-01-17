Shader "Cookbook/ScrollingTexture" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_ScrollXSpeed("X Scroll Speed", Range(0, 10)) = 2
		_ScrollYSpeed("Y Scroll Speed", Range(0, 10)) = 2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;
		sampler2D _MainTex;

		void surf(Input IN, inout SurfaceOutput output)
		{
			fixed x_offset = _ScrollXSpeed*_Time;
			fixed y_offset = _ScrollYSpeed*_Time;
			fixed2  scrolledUV = IN.uv_MainTex + fixed2(x_offset, y_offset);
			fixed4 color = tex2D(_MainTex, scrolledUV)*_Color;
			output.Albedo = color.rgb;
			output.Alpha = color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
