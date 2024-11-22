using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField]
    public GameObject planeData;
    public void DestroyWall()
    {
        Destroy(gameObject);        
        //DestroyWallLine(0,0);
    }

    private void DestroyWallLine(int x, int z)
    {
        for (int i = 0; i < 4; i++)
        {
            Destroy(planeData.GetComponent<MapLoader>().wallArray[(int)(gameObject.transform.position.x-0.5f)+x, (int)(gameObject.transform.position.z-0.5f)+z, i]);
        }
    }

}
