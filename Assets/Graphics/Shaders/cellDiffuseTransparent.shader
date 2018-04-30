Shader "Custom/cellDiffuseTransparent" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _LightMultiplier ("Light Multiplier", Float) = .7
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineSize ("Outline Size", Float) = 0
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		LOD 200
		
		Pass {
            Cull Front
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 position : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _OutlineColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _OutlineSize)
            UNITY_INSTANCING_BUFFER_END(Props)
            
            v2f vert(appdata vert) {
                v2f output; 
                UNITY_SETUP_INSTANCE_ID(vert);
                UNITY_TRANSFER_INSTANCE_ID(vert, output);
                float3 worldNorm = UnityObjectToWorldNormal(vert.normal);
                float4 worldPos = mul(unity_ObjectToWorld, vert.position);
                worldPos.xyz += worldNorm*UNITY_ACCESS_INSTANCED_PROP(Props, _OutlineSize);
                output.pos = mul(UNITY_MATRIX_VP, worldPos);
                return output;
            }
            
            half4 frag(v2f i) : SV_TARGET 
            {
                clip(_OutlineSize - 0.01);
                UNITY_SETUP_INSTANCE_ID(i);
                return UNITY_ACCESS_INSTANCED_PROP(Props, _OutlineColor);
            }
            
            ENDCG
        }

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Cel alpha

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
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
            fixed4 col = (tex2D (UNITY_ACCESS_INSTANCED_PROP(_MainTex_arr, _MainTex), IN.uv_MainTex) * 
                        UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color));
			o.Albedo = col.rgb;
            o.Alpha = col.a;

            //o.Alpha = 1;
		}
		ENDCG
		
	}
	FallBack "Diffuse"
}