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

    // Radius controls how large the ripple area is
    float radius = _RippleDiameter * 0.5;

    // Fade is now a soft gradient that falls off naturally
    float fade = exp(-pow(dist / radius, 2.5)); // smooth Gaussian-like falloff

    // Sinusoidal wave motion with fade-out toward edges
    float wave = sin(dist * 60.0 - _Time.y * _RippleSpeed) * 0.03 * _RippleStrength * fade;

    float ripple = dist + wave;
    float2 uv = _RippleOrigin + normalize(dir) * ripple;

    fixed4 col = tex2D(_MainTex, uv);
    return col;
}
            ENDCG
        }
    }
}
