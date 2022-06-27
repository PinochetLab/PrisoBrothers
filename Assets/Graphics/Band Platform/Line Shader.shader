// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Line Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Scale("Tiling X", Float) = 1.0
		_Offset("Tiling X", Float) = 0.0
		_Speed("Speed", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog


            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD2;
				float4 dir : TEXCOORD3;
				float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float2 worldPos : TEXCOORD2;
				float2 dir : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Scale;
			float _Offset;
			float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				// o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
				float4 vec = normalize(mul(unity_ObjectToWorld, v.vertex + v.uv) - mul(unity_ObjectToWorld, v.vertex));
				o.dir = mul(unity_ObjectToWorld, v.tangent).xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 pos = i.worldPos;
				i.uv.x = (dot(pos, i.dir) + _Offset) * _Scale - _Speed * _Time.y * abs(_Scale);
				
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                return col;
            }
            ENDCG
        }
    }
}
