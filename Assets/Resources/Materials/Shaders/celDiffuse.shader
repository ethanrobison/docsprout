Shader "Custom/celDiffuse" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _LightMultiplier ("Light Multiplier", Float) = .7
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Cel fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        #include "celLighting.cginc"
        

		struct Input {
			float2 uv_MainTex;
		};
        

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(sampler2D, _MainTex)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = (tex2D (UNITY_ACCESS_INSTANCED_PROP(_MainTex_arr, _MainTex), IN.uv_MainTex) * UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color)).rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
