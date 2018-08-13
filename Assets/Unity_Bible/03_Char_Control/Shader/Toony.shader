// Simple Toony Shader

Shader "Custom/Toony"
{
	Properties
	{
		// Inspector
		_Color("Color", Color) = (0.5, 0.5, 0.5, 1.0)
		_HColor("Highlight Color", Color) = (0.6, 0.6, 0.6, 1.0)
		_SColor("Shadow Color", Color) = (0.3, 0.3, 0.3, 1.0)
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM

#pragma surface surf Ramp
#pragma target 2.0

		// Variables
		fixed4 _Color;
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _Ramp;

		struct Input
		{
			float4 color : COLOR;
		};

		inline half4 LightingRamp(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{

			fixed NdotL = dot(s.Normal, lightDir) * 0.5 + 0.5;
			fixed3 ramp = tex2D(_Ramp, fixed2(NdotL, NdotL)) * atten;
			_SColor = lerp(_HColor, _SColor, _SColor.a);
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);

			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.a = s.Alpha;

			return c;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = _Color.rgb;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
