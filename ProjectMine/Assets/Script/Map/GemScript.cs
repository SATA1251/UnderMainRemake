using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public void DestroyGem()
    {        
        if (gameObject.CompareTag("GemS"))
        {
            player.GetComponent<PlayerController>().oresSamount++;
        }
        else if (gameObject.CompareTag("GemA"))
        {
            player.GetComponent<PlayerController>().oresAamount++;
        }
        else if (gameObject.CompareTag("GemB"))
        {
            player.GetComponent<PlayerController>().oresBamount++;
        }
        Destroy(gameObject);
    }
}
