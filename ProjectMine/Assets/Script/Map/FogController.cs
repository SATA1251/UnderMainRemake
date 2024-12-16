using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    public Camera minimapCamera;
    public Texture2D fogTexture; //어둠 텍스쳐
    public int textureSize = 512;
    public int viewRadius = 50;
    public Vector3 playerWorldPos;

    private Color[] fogColors;
    void Start()
    {
        fogTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
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
            fogMaterial.SetTexture("_FogTexture", fogTexture);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float cameraSize = minimapCamera.orthographicSize;
        float aspectRatio = minimapCamera.aspect;
        playerWorldPos = transform.position;

        Vector2Int texCoord = WorldToTextureCoord(playerWorldPos);

        //int radius = 10; // 밝히는 영역 반지름
        //for (int x = -radius; x <= radius; x++)
        //{
        //    for (int y = -radius; y <= radius; y++)
        //    {
        //        int px = Mathf.Clamp(texCoord.x + x, 0, textureSize - 1);
        //        int py = Mathf.Clamp(texCoord.y + y, 0, textureSize - 1);
        //        fogColors[py * textureSize + px] = Color.clear; // 탐험한 부분 투명 처리
        //    }
        //}

        //어둠 텍스처 갱신
        //fogTexture.SetPixels(fogColors);
        //fogTexture.Apply();

        UpdateFog(texCoord, viewRadius);
    }

    Vector2Int WorldToTextureCoord (Vector3 worldPos)
    {
        float x = Mathf.InverseLerp(0f, 100f, worldPos.x) * textureSize; // 월드 범위 (-50, 50) 예시
        float y = Mathf.InverseLerp(0f, 100f, worldPos.z) * textureSize;
        x = textureSize - x;
        y = textureSize - y;
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
        fogTexture.Apply(); // 텍스쳐 업데이트
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
