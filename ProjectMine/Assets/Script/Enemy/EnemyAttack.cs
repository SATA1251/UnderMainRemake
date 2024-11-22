using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // �� ĳ���� ������ ����� ��ũ��Ʈ
    // �÷��̾ ���⸦ �޾Ƽ� ���ų� ���⼭ �÷��̾��� ü���� �ްų�

    [SerializeField]
    private PlayerController playerController;

    private const string playerTag = "Player";

    private EnemyAI enemyAi;

    // [SerializeField] GameObject goThis;


    void Start()
    {
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyAi = GetComponent<EnemyAI>();
    }

 
    // Update is called once per frame
    void Update()
    {
       
    }


    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == playerTag && enemyAi.state == EnemyAI.State.ATTACK)
        {
            playerController.BeAttacked();
            //�÷��̾�� ���Ͱ� �浹�߰� ���Ͱ� ���� ������ ��� �÷��̾��� ü���� �����Ѵ�
        }
        
    }
}
