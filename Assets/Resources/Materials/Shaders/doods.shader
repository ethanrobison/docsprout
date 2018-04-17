Shader "Custom/doods" {
	Properties {
        [PerRendererData] _MainTex("Texture", 2D) = "black" {}
		_Color ("Happy Color", Color) = (1,1,1,1)
        _SickColor ("Sick Color", Color) = (1,1,1,1)
        _Happiness ("Happiness", Range(0, 1)) = 1
        _LightMultiplier ("Ramp Multiplier", Float) = 1
        _HighlightColor("Highlight Color", Color) = (.3, 0, 1, 1)
        _HighlightSize ("Highlight Size", Float) = .1
        
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

        Pass {
            Cull Front

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _HighlightColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _HighlightSize)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                float4 viewNorm = mul(UNITY_MATRIX_IT_MV, float4(v.normal, 0));
                float4 viewPos = float4(UnityObjectToViewPos(v.vertex), 1);
                viewPos += viewNorm*UNITY_ACCESS_INSTANCED_PROP(Props, _HighlightSize);
                o.pos = mul(UNITY_MATRIX_P, viewPos);
                return o;
            }

            half4 frag(v2f i) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(i);
                return UNITY_ACCESS_INSTANCED_PROP(Props, _HighlightColor);
            }

            ENDCG
        }

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Cel fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        #include "celLighting.cginc"

        sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};
        
        //float _LightMultiplier;
        fixed4 _SickColor; //sick color, dood

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(fixed, _Happiness)
		UNITY_INSTANCING_BUFFER_END(Props)

        /*
        half4 LightingCel (SurfaceOutput s, half3 lightDir, half atten) {
            half NdotL = dot (s.Normal, lightDir);
            NdotL = min(1, max(0, NdotL)*20)*_LightMultiplier;
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
            c.a = s.Alpha;
            return c;
        }*/

		void surf (Input IN, inout SurfaceOutput o) {
            fixed4 texCol = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 c = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            fixed h = UNITY_ACCESS_INSTANCED_PROP(Props, _Happiness);
			o.Albedo = lerp(lerp(_SickColor.rgb, c.rgb, h), texCol.rgb, texCol.a);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
