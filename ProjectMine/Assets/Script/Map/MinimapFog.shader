Shader "Custom/MinimapFog"
{
    Properties
    {
        _FogTexture ("Fog Texture", 2D) = "white" {} // ��� �����͸� ������ �ؽ�ó
    }
    SubShader
    {
        Tags { "Queue"="Overlay" } // �׻� ȭ�� ���� ������
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha // ���� ���� (���� ����)
            Cull Off // ��� ������

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _FogTexture; // �ؽ�ó ���÷�

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; // UV ��ǥ
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; // UV ��ǥ
                float4 vertex : SV_POSITION; // Ŭ�� ��ǥ
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // ���� ��ǥ�� Ŭ�� �������� ��ȯ
                o.uv = v.uv; // UV ��ǥ ����
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // FogTexture���� �ȼ� ������ ������ ������
                return tex2D(_FogTexture, i.uv);
            }
            ENDCG
        }
    }
}