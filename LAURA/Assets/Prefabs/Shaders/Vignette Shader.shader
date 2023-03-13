Shader "Custom/Vignette" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Range(0.0, 1.0)) = 0.5
        _Softness ("Softness", Range(0.0, 1.0)) = 0.5
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Radius;
            float _Softness;
            sampler2D _MainTex;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                float4 center = float4(0.5, 0.5, 0, 0);
                float4 dist = float4(i.uv - center.xy, 0, 0);
                float vignette = 1.0 - smoothstep(_Radius, _Radius - _Softness, length(dist));
                return col * _Color * vignette;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
