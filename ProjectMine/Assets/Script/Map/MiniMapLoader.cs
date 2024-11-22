using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class MiniMapLoader : MonoBehaviour
{
    public Object[,,] wallArray = new Object[10, 10, 6];

    public GameObject wall1;
    public GameObject wall2;
    public GameObject hardWall;

    const int width = 5;
    const int height = 5;

    void Start()
    {
        StartSceneCreateOutLine();
        StartSceneCreateCeil();
    }

    void CreateWall(int x, int z, int n, Object objWall)
    {
        for (int i = 1; i < n+1; i++)
        {
            int randomNum = Random.Range(1, 3);
            if (randomNum == 1 && objWall == wall1)
            {
                wallArray[x, z, i-1] = Instantiate(wall1, new Vector3(gameObject.transform.position.x - width+0.5f +x, gameObject.transform.position.y-0.5f +i, gameObject.transform.position.z - height+0.5f+z), Quaternion.identity);
            }
            else if (randomNum > 1 && objWall == wall1)
            {
                wallArray[x, z, i-1] = Instantiate(wall2, new Vector3(gameObject.transform.position.x - width+0.5f +x, gameObject.transform.position.y-0.5f +i, gameObject.transform.position.z - height+0.5f+z), Quaternion.identity);
            }
            else if (objWall != wall1)
            {
                wallArray[x, z, i-1] = Instantiate(objWall, new Vector3(gameObject.transform.position.x - width+0.5f +x, gameObject.transform.position.y-0.5f +i, gameObject.transform.position.z - height+0.5f+z), Quaternion.identity);
            }
        }
    }

    void StartSceneCreateOutLine()
    {
        for (int i = 0; i < width*2; i++)
        {
            CreateWall(i, 0, 5, wall1);
            CreateWall(i, width*2-1, 5, wall1);
        }
        for (int i = 1; i < height*2-1; i++)
        {
            CreateWall(0, i, 5, wall1);
            CreateWall(width*2-1, i, 5, wall1);
        }
    }

    void StartSceneCreateCeil()
    {
        for (int i = 0; i < width*2; i++)
        {
            for (int j = 0; j < height*2; j++)
            {
                wallArray[i, j, 5] = Instantiate(hardWall, new Vector3(gameObject.transform.position.x - width+0.5f +i, gameObject.transform.position.y + 5.5f, gameObject.transform.position.z - height+0.5f+j), Quaternion.identity);

            }
        }
    }
}
