using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // �� ĳ���� ������ ����� ��ũ��Ʈ
    // �÷��̾ ���⸦ �޾Ƽ� ���ų� ���⼭ �÷��̾��� ü���� �ްų�

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
            playerController.BeAttackedBossNormal();  // ���� �ȿ��� ������ ���� => EnemtDamage ���� ����
            //�÷��̾�� ���Ͱ� �浹�߰� ���Ͱ� ���� ������ ��� �÷��̾��� ü���� �����Ѵ�
        }
    }

}
