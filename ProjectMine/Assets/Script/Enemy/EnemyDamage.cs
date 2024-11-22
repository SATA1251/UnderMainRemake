using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float hp = 300.0f;

    public float enemyDamage = 30.0f;

    public float bossNormalDamage = 100.0f;

    public float bossThrowDamage = 70.0f;
    //public bool isHit = false;

    public HandController handController;
    public Hand hand;
    // Start is called before the first frame update

    private EnemyCtrl enemyCtrl;
    void Start()
    {
        handController = GameObject.Find("Holder").GetComponent<HandController>();
        hand = GameObject.Find("Hand").GetComponent<Hand>();
        enemyCtrl = GameObject.Find("EnemyCtrl").GetComponent<EnemyCtrl>();
    }

    public void EnemyHit()
    {
        hp -= hand.applyDamage;
        enemyCtrl.isHit1st = true;
        GetComponent<EnemyAI>().state = EnemyAI.State.HIT;
        if (hp <= 0.0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        }

    }
    public void EnemyHit2()
    {
        hp -= hand.applyDamage;
        enemyCtrl.isHit2nd = true;
        GetComponent<EnemyAI>().state = EnemyAI.State.HIT;
        if (hp <= 0.0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        }

    }

    public void BossHit()
    {
        hp -= hand.applyDamage;

        if (hp <= 0.0f)
        {
            GetComponent<BossAi>().state = BossAi.State.DIE;
        }
    }

    public void BossNormalAttack()
    {
        hp -= hand.applyDamage;
        if (hp <= 0.0f)  // �ڵ忡 ���ݷ� �޾ƿ��� ���� �߰� ��û
        {
            GetComponent<BossAi>().state = BossAi.State.DIE;
        }
    }

    public void BossStoneAttack()
    {
        hp -= hand.applyDamage;  // �ڵ忡 ���ݷ� �޾ƿ��� ���� �߰� ��û
        if (hp <= 0.0f)
        {
            GetComponent<BossAi>().state = BossAi.State.DIE;
        }
    }

    // �ǰ� ȿ���� �����ϴ� �Լ�
    void ShowHitEffect(Collision collision)
    {
        //����� �浹�� ����
        Vector3 pos = collision.contacts[0].point;

        // �浹 ���� ���� ���� ����
        Vector3 _normal = collision.contacts[0].normal;
        
        // �浹 �� ���� ������ ȸ���� ���
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
    }
}
