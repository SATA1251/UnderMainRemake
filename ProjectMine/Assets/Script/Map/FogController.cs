using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{

    public Texture2D fogTexture; //어둠 텍스쳐
    public int textureSize = 512;
    public int viewRadius = 10;

    private Color[] fogColors;
    void Start()
    {
        fogTexture = new Texture2D(textureSize, textureSize);
        fogColors = new Color[textureSize * textureSize];

        // 초기화: 모든 영역을 어둡게 설정
        for (int i = 0; i < fogColors.Length; i++)
            fogColors[i] = Color.black;

        fogTexture.SetPixels(fogColors);
        fogTexture.Apply();

        // 어둠 Plane의 머티리얼에 텍스처 연결
        Renderer fogRenderer = GameObject.Find("MiniMapFog").GetComponent<Renderer>();
        if (fogRenderer != null)
        {
            Material fogMaterial = fogRenderer.material;
            fogMaterial.SetTexture("MiniMapFog", fogTexture);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerWorldPos = transform.position;

        Vector2Int playerTexPos = WorldToTextureCoord(playerWorldPos);

        UpdateFog(playerTexPos, viewRadius);
    }

    Vector2Int WorldToTextureCoord (Vector3 worldPos)
    {
        float x = Mathf.InverseLerp(-50f, 50f, worldPos.x) * textureSize;
        float y = Mathf.InverseLerp(-50f, 50f, worldPos.z) * textureSize;
        return new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
    }

    void UpdateFog(Vector2Int playerPos, int radius)
    {
        for(int y = -radius; y <= radius; y++)
        {
            for(int x = -radius; x <= radius; x++)
            {
                Vector2Int pos = playerPos + new Vector2Int(x, y);
                if(isInsideBounds(pos) && IsWithinRadius(playerPos, pos, radius))
                {
                    int index = pos.y * textureSize + pos.x;
                    if (fogColors[index] == Color.black) 
                    {
                        fogColors[index] = Color.clear;
                    }
                }
            }
        }

        fogTexture.SetPixels(fogColors);
        fogTexture.Apply();
    }

    bool isInsideBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < textureSize && pos.y >= 0 && pos.y < textureSize;
    }

    bool IsWithinRadius(Vector2Int center, Vector2Int point, int radius)
    {
        return (center - point).sqrMagnitude <= radius * radius;
    }
}
