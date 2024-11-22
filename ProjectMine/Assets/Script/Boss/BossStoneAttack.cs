using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStoneAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    // �÷��̾��� ��ġ�� ������ ����

    private Vector3 originalPosition;

    // ���� �÷��̾��� ��ġ
    private Transform throwPoint;

    private BossAi bossAi;

    private const string playerTag = "Player";

    private Transform playerTransform;  // ��ǥ

    float throwForce = 30.0f;

    void Start()
    {
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        bossAi = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossAi>();


        originalPosition = gameObject.transform.position;
    }

    private void Update()
    {
    }
    public void StoneAttack()
    {
        originalPosition = gameObject.transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Vector3 throwDirection = (playerTransform.position - transform.position).normalized;
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
            gameObject.transform.position = originalPosition;
        }
          
    }

    public void OnCollisionEnter(Collision collision)
    {
        // ���� ���� ���
        if(collision.collider.tag == playerTag && bossAi.state == BossAi.State.STONE_ATTACK)
        {
            playerController.BeAttackedBossStone();
        }
    }
}
