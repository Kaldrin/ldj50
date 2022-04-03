Shader "Unlit/ScrollingWorldAlignedText"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tiling ("Tiling", Vector) = (1, 1, 1, 1)
        _Speed ("Speed", Vector) = (1, 1, 1, 1)
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        { 
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            //Cull Off
            //ZWrite Off
            //Blend SrcAlpha One // Blend mode operation, here it's additive
            
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                
                

                struct appdata
                {
                    float3 vertex : POSITION;
                    float3 worldPos : TEXCOORD1;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float3 worldPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Tiling;
                float4 _Speed;
                float4 _Color;

                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.worldPos = mul (unity_ObjectToWorld, float4(v.vertex, 1));
                    return o;
                }

                fixed4 frag (v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.worldPos.xy + _Time.y * _Speed.xy);
                    //col *= _Color;
                    //clip(frac(i.worldPos.y + _Time.y * _Speed.y) - 0.5);
                    clip(col.a - 0.2);
                    //col.a = 1;
                    //fixed4 col;
                    //col.rgb = i.worldPos;
                    //col.r = 0;
                    //col.a = 1;
                    //clip (col.r - 0.5);
                    //col.rgb = i.worldPos;
                    col *= _Color;
                    return col;
                }
            ENDCG
        }
    }
}
