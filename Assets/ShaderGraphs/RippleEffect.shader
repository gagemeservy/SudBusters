Shader "Custom/RippleEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RippleOrigin ("Ripple Origin", Vector) = (0.5, 0.5, 0, 0)
        _RippleStrength ("Ripple Strength", Float) = 0.0
        _RippleDiameter ("Ripple Diameter", Float) = 1
        _RippleSpeed ("Ripple Speed", Float) = 10
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float2 _RippleOrigin;
            float _RippleStrength;
            float _RippleDiameter;
            float _RippleSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 dir = i.uv - _RippleOrigin;
                float dist = length(dir);

                if (dist < 1e-4) return tex2D(_MainTex, i.uv);

                float2 n = dir / dist;

                // Adjust wave frequency and spread using diameter and speed
                float waveFrequency = lerp(60.0, 20.0, saturate(_RippleDiameter)); // bigger diameter = slower waves
                float waveSpeed = _RippleSpeed * 0.5; // controls outward motion

                // The wave moves outward with time
                float wave = sin(dist * waveFrequency - _Time.y * waveSpeed) * 0.03;

                // Limit how far ripples can affect (diameter as cutoff)
                float fade = smoothstep(_RippleDiameter * 0.5, 0.0, dist);

                // Apply the ripple
                float ripple = dist + wave * _RippleStrength * fade;

                float2 uv = _RippleOrigin + n * ripple;
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
