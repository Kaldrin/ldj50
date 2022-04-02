Shader "Graph/GridUnlit"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1)
        _Tiling ("Tiling", Range(0.1, 10)) = 1
        _ScrollSpeed ("Scroll speed", Vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        LOD 100

        Pass
        {
            Cull Off // Don't cull anything polygon
            Zwrite Off // Don't write into depth buffer
            //Blend SrcAlpha OneMinusSrcAlpha // Blend mode lerp, transparent
            Blend SrcAlpha One // Blend mode operation, here it's additive


            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float3 vertex : POSITION;
                    float3 worldPos : TEXCOORD0;
                };
//
                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float3 worldPos : TEXCOORD0;
                };


                float4 _ScrollSpeed;
                float _Tiling;
                float4 _Color;


                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos (v.vertex);
                    o.worldPos = mul (unity_ObjectToWorld, float4 (v.vertex, 1));
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Get time
                    float time = _Time.y;

                    // Calculate z grid
                    float zValue = frac (i.worldPos.y * _Tiling + time * _ScrollSpeed.y) - 0.98;
                    float testValue = step (0, zValue);
                    zValue = lerp((0, 0, 0, 0), (1, 1, 1, 1), testValue).x;

                    // Calculate x grid
                    float xValue = frac(i.worldPos.x * _Tiling + time * _ScrollSpeed.x) - 0.98;

                    // Clip if not grid
                    clip (xValue + zValue);


                    // Apply color
                    float4 col = _Color;
                    return col;
                }
            ENDCG
        }
    }
}
