// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/SpriteWorld"
{
    Properties
    {
		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
		_ScaleX("Tiling X", Float) = 1.0
		_ScaleY("Tiling Y", Float) = 1.0
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 100

		Blend SrcAlpha OneMinusSrcAlpha
		Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

			float _ScaleX;
			float _ScaleY;
			fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float2 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				// o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
				//o.worldPos = ComputeScreenPos(v.vertex).xy;
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				i.worldPos.x *= _ScaleX;
				i.worldPos.y *= _ScaleY;
				i.uv = i.worldPos - float2(floor(i.worldPos.x), floor(i.worldPos.y));

				
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col *= _Color;
                return col;
            }
            ENDCG
        }
    }
}
