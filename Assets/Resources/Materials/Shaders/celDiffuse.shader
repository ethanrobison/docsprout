Shader "Custom/celDiffuse" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _LightMultiplier ("Light Multiplier", Float) = .7
        _PixelSkip ("Pixel Skip", Float) = 0
        _PixelSize ("Pixel Size", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Cel fullforwardshadows nometa

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        float _LightMultiplier;

        half4 LightingCel (SurfaceOutput s, half3 lightDir, half atten) {
            half NdotL = dot (s.Normal, lightDir);
            NdotL = min(1, max(0, NdotL)*20)*_LightMultiplier;
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
            c.a = s.Alpha;
            return c;
        }

        float2 _ScreenSize;
        float _PixelSize;

		struct Input {
			float2 uv_MainTex;
            //float2 pixPos;
            float4 screenPos;
		};

        //void vert(inout appdata_full v, out Input o) {
            //float4 pixPos = ComputeScreenPos(UnityObjectToClipPos(v.vertex));
            //UNITY_INITIALIZE_OUTPUT(Input,o);
            //o.pixPos = pixPos.xy;
        //}
        

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(sampler2D, _MainTex)
            UNITY_DEFINE_INSTANCED_PROP(float, _PixelSkip)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
            if(_PixelSkip > 0) {
                //float4 sp = mul(UNITY_MATRIX_VP, float4(IN.worldPos, 1));
                int2 pixPos = IN.screenPos.xy*_ScreenSize/IN.screenPos.w/_PixelSize;
                bool dontSkip = (pixPos.x%(int)_PixelSkip) < 1 && (pixPos.y%(int)_PixelSkip) < 1;
                //bool dontSkip = IN.pixPos.x < .5;
                dontSkip = dontSkip || ((pixPos.x + ((int)_PixelSkip >> 1))%(int)_PixelSkip) == 0 && ((pixPos.y + ((int)_PixelSkip>>1))%(int)_PixelSkip) == 0;
                //o.Alpha = dontSkip -1;
                clip(dontSkip -1);
            }

			o.Albedo = (tex2D (UNITY_ACCESS_INSTANCED_PROP(_MainTex_arr, _MainTex), IN.uv_MainTex) * 
                        UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color)).rgb;

            //o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
