Shader "Cookbook/AnimatedSpriteSheet" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Sprite Sheet (RGB)", 2D) = "white" {}

		_TexWidth("Sheet Width", float) = 0.0
		_CellCount("Cell Count", float) = 0.0
		_Speed("Speed", Range(0.01, 32)) = 12
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		float _TexWidth;
		float _CellCount;
		float _Speed;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float cellPixelWidth = _TexWidth/_CellCount;
			float cellUVPercentage = cellPixelWidth/_TexWidth;
			float2 spriteUV = IN.uv_MainTex;
			float timeVal = ceil(fmod(_Time.y*_Speed, _CellCount));
			float xValue = (spriteUV.x + cellUVPercentage*timeVal*_CellCount)*cellUVPercentage;

			// Albedo comes from a sheet tinted by color
			fixed4 c = tex2D (_MainTex, spriteUV)*_Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
