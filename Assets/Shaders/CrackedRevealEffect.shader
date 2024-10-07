Shader "Custom/CircularReveal"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}  // Your main image with transparency
        _Cutoff ("Cutoff", Range(0, 1)) = 0.0       // Cutoff value to reveal the cracks
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }            // Ensure it renders after opaque objects
        Blend SrcAlpha OneMinusSrcAlpha              // Proper blending mode for transparency
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Cutoff;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate the distance from the center of the image
                float distance = length(i.uv - 0.5);

                // Reveal based on the _Cutoff value
                if (distance > _Cutoff)
                    discard;

                // Keep the alpha transparency from the original texture
                col.a *= step(distance, _Cutoff);  // Fade out the pixels based on the reveal

                return col;
            }
            ENDCG
        }
    }
}
