Shader "Custom/MinimapFog"
{
    Properties
    {
        _FogTexture ("Fog Texture", 2D) = "white" {} // 어둠 데이터를 저장할 텍스처
    }
    SubShader
    {
        Tags { "Queue"="Overlay" } // 항상 화면 위에 렌더링
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 (투명도 지원)
            Cull Off // 양면 렌더링

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _FogTexture; // 텍스처 샘플러

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; // UV 좌표
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; // UV 좌표
                float4 vertex : SV_POSITION; // 클립 좌표
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 월드 좌표를 클립 공간으로 변환
                o.uv = v.uv; // UV 좌표 유지
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // FogTexture에서 픽셀 색상을 가져와 렌더링
                return tex2D(_FogTexture, i.uv);
            }
            ENDCG
        }
    }
}