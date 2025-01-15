using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossStoneAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    // 플레이어의 위치를 저장할 변수

    GameObject rockPoint;

    private Transform originalPosition;

    // 던질 플레이어의 위치
    private Transform throwPoint;

    private BossAi bossAi;

    private const string playerTag = "Player";

    public Transform playerTransform;  // 목표

    private Vector3 targetPosition;  // 목표

    float throwForce = 10.0f;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        bossAi = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossAi>();

        //playerTransform = GameObject.Find("Player").GetComponent<Transform>();

        rockPoint = GameObject.Find("RockSpownPosition");

        if(rockPoint != null)
        {
            originalPosition = rockPoint.GetComponent<Transform>();
        }
    }

    private void Update()
    {
    }

    public void GetPlayerPosition(Vector3 playerPosition)
    {
        targetPosition = playerPosition;
    }
    public void StoneAttack()
    {
       // originalPosition = gameObject.transform.position;    
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Vector3 throwDirection = (targetPosition - transform.position).normalized;
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }

    }

    public void StoneAttackEnd()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            gameObject.transform.position = originalPosition.position;
        }
          
    }

    public void OnCollisionEnter(Collision collision)
    {
        // 돌에 맞을 경우
        if(collision.collider.tag == playerTag)
        {
            playerController.BeAttackedBossStone();
        }
    }
}
