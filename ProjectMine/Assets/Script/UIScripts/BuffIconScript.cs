using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIconScript : MonoBehaviour
{
    [SerializeField]
    private GameObject buffImage;
    [SerializeField]
    GameObject player;

    float isBuff;

    private void Start()
    {
        //isBuff = player.GetComponent<PlayerController>().ox;
    }
    void Update()
    {
        if (isBuff > 0)
        {
            buffImage.SetActive(true);
        }
        else if (isBuff <= 0)
        {
            buffImage.SetActive(false);
        }

    }
}
