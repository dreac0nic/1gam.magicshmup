Shader "Cookbook/BasicDiffuse" {
	Properties {
		_EmissiveColor ("Emissive Color", Color) = (1, 1, 1, 1)
		_AmbientColor ("Ambient Color", Color) = (1, 1, 1, 1)
		_MySliderValue("This is a Slider", Range(0, 10)) = 2.5
		_RampTex("Lighting Ramp", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BasicDiffuseHalfLambertBRDF

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float4 _EmissiveColor;
		float4 _AmbientColor;
		float _MySliderValue;
		sampler2D _RampTex;

		inline float4 LightingBasicDiffuse(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = max(0, dot(s.Normal, lightDir));

			float4 col;
			col.rgb = s.Albedo*_LightColor0.rgb*(difLight*atten*2);
			col.a = s.Alpha;

			return col;
		}

		inline float4 LightingBasicDiffuseHalfLambert(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = dot(s.Normal, lightDir);
			float hLambert = 0.5*difLight + 0.5;

			float4 col;
			col.rgb = s.Albedo*_LightColor0.rgb*(hLambert*atten*2);
			col.a = s.Alpha;

			return col;
		}

		inline float4 LightingBasicDiffuseHalfLambertRamp(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = dot(s.Normal, lightDir);
			float hLambert = 0.5*difLight + 0.5;
			float3 ramp = tex2D(_RampTex, float2(hLambert, hLambert)).rgb;

			float4 col;
			col.rgb = s.Albedo*_LightColor0.rgb*(ramp);
			col.a = s.Alpha;

			return col;
		}

		inline float4 LightingBasicDiffuseHalfLambertBRDF(SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			float difLight = dot(s.Normal, lightDir);
			float rimLight = dot(s.Normal, viewDir);
			float hLambert = 0.5*difLight + 0.5;
			float3 ramp = tex2D(_RampTex, float2(hLambert, rimLight)).rgb;

			float4 col;
			col.rgb = s.Albedo*_LightColor0.rgb*(ramp);
			col.a = s.Alpha;

			return col;
		}

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float4 color;

			color = pow((_EmissiveColor + _AmbientColor), _MySliderValue);
			o.Albedo = color.rgb;
			o.Alpha = color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
