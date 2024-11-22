using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalScript : MonoBehaviour
{
    //public GameObject plane;
    [SerializeField]
    private Transform arrivalTransform;

    [SerializeField]
    private GameObject gamePlayer;
    //public GameObject startMonster;
    //private Transform playerTransform;

    private const string playerTag = "Player";
    //public Vector3 playerMoveVector3;

    private void Start()
    {
       
        //playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == playerTag)
        {
            gamePlayer.transform.localPosition = arrivalTransform.position;
        }

    }

    public void PlayerMoveMine()
    {        
       //playerTransform.transform.Translate(-205, 0, -4, Space.World);
       //playerTransform.transform.Translate(startMonster.GetComponent<Transform>().position.x, 0, startMonster.GetComponent<Transform>().position.z,Space.World);

        //playerTransform.transform.Translate(plane.GetComponent<MapLoader>().playerMoveVector);
    }
}
