// Niebo z panoramy, dolna połowa to odbicie górnej (niebo też pod torem)
Shader "Ballance/NieboOdbite"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Panorama (HDR)", 2D) = "grey" {}
        _Exposure ("Exposure", Range(0, 8)) = 1
        _Rotation ("Rotation", Range(0, 360)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            half _Exposure;
            float _Rotation;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 kierunek : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.kierunek = v.vertex.xyz;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float3 d = normalize(i.kierunek);
                d.y = abs(d.y); // dół = odbicie góry

                float dlugosc = atan2(d.x, d.z) + radians(_Rotation);
                float szerokosc = asin(d.y);
                float2 uv = float2(0.5 + dlugosc / (2 * UNITY_PI), 0.5 + szerokosc / UNITY_PI);

                half3 kolor = tex2Dlod(_MainTex, float4(uv, 0, 0)).rgb * _Exposure;
                return half4(kolor, 1);
            }
            ENDCG
        }
    }
    Fallback Off
}
