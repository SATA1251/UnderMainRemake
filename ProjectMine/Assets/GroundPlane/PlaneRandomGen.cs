using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRandomGen : MonoBehaviour
{
    private Mesh Plane;
    private MeshCollider col;
    private Color[] vertColor;

    private int rangeX;
    private int rangeY;

    void Start()
    {
        Plane = gameObject.GetComponent<MeshFilter>().mesh;
        //col = gameObject.AddComponent<MeshCollider> (); 
        Vector3 [] vert = Plane.vertices;
        vertColor = new Color[vert.Length];
        rangeX = Random.Range (0,100);
        rangeY = Random.Range (0,100);

        noiseGen(vert);

    
    }

    void noiseGen (Vector3 [] vert) 
    {
        for (int i=0; i<vert.Length; i++) 
        {
            //vert[i] = new Vector3 (vert[i].x, vert[i].y + Mathf.Clamp (Mathf.PerlinNoise(Mathf.Abs(vert[i].x) * rangeX, Mathf.Abs(vert[i].y) * rangeY), 0, 1), vert[i].z);
            if (Random.Range(0f,1f) > 0.8f) vertColor[i] = Color.red;
            else vertColor[i] = Color.black;

            Plane.SetVertices(vert);

            //Debug.Log (vertColor[i].r);
        }
        Plane.SetColors (vertColor);
        //col.sharedMesh = Plane;
    }

    

}
