using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // 적 캐릭터 공격을 담당할 스크립트
    // 플레이어가 여기를 받아서 쓰거나 여기서 플레이어의 체력을 받거나

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
            //플레이어와 몬스터가 충돌했고 몬스터가 공격 상태일 경우 플레이어의 체력을 감소한다
        }
        
    }
}
