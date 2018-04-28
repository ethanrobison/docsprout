Shader "Custom/celDiffuse" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _LightMultiplier ("Light Multiplier", Float) = .7
        _Alpha ("Alpha", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Cel

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        #include "celLighting.cginc"

        float2 _ScreenSize;
        float _PixelSize;

		struct Input {
			float2 uv_MainTex;
            float4 screenPos;
		};
        

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(sampler2D, _MainTex)
            UNITY_DEFINE_INSTANCED_PROP(float, _Alpha)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
            if(_Alpha < 1){
                int2 pixPos = IN.screenPos.xy*_ScreenSize/IN.screenPos.w;

                float4x4 thresholdMatrix =
                {
                1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                };

                clip(UNITY_ACCESS_INSTANCED_PROP(_Alpha_arr, _Alpha) - thresholdMatrix[pixPos.x % 4][pixPos.y % 4]);
            }

			o.Albedo = (tex2D (UNITY_ACCESS_INSTANCED_PROP(_MainTex_arr, _MainTex), IN.uv_MainTex) * 
                        UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color)).rgb;

            //o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
