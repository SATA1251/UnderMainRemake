using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // 적 캐릭터 공격을 담당할 스크립트
    // 플레이어가 여기를 받아서 쓰거나 여기서 플레이어의 체력을 받거나

    [SerializeField]
    private PlayerController playerController;

    private BossAi bossAi;

    private const string playerTag = "Player";

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        bossAi = GameObject.Find("Prefab_Boss Variant").GetComponent<BossAi>();

        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(playerTag) && bossAi.state == BossAi.State.NORMAL_ATTACK)
        {
            playerController.BeAttackedBossNormal();  // 엔진 안에서 데미지 조절 => EnemtDamage 에서 조절
            //플레이어와 몬스터가 충돌했고 몬스터가 공격 상태일 경우 플레이어의 체력을 감소한다
        }
    }

}
