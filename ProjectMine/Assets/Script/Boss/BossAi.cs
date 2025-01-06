using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{

    public enum State
    {
        IDLE,
        MOVE,
        NORMAL_ATTACK,
        STONE_ATTACK,
        DIE
    }

    // ���¸� ������ ����

    public State state = State.MOVE;

    // �÷��̾��� ��ġ�� ������ ����
    private Transform playerTransform;

    // ���� ��ġ�� ���� �� ����
    private Transform bossTransform;

    // ���� �����Ÿ�
    public float attackDist = 5.0f;

    public float stoneAttackDist = 50.0f;

    // ���� �����Ÿ�
    public float traceDist = 100.0f;

    //������ ü�¹� ������ ������ ����
    public EnemyDamage enemyDamage;
    // ��� ���θ� �Ǵ��� ����
    public bool isDie = false;

    // �ڷ�ƾ���� ����� �����ð� ����
    private WaitForSeconds ws;

    // �̵� ���� Ŭ���� ȣ��
    private BossMoveAgent moveAgent;

    private BossAttack bossAttack;

    private BossStoneAttack stoneAttack;

    // �ִϸ����� ������Ʈ�� ������ ����

    private Animator animator;

    // �Ϲ� ���� üũ�� ����
    public bool normalAttackCheck = false;

    public bool isAttack = false;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashStoneAttack = Animator.StringToHash("IsStoneAttack");
    private readonly int hashDead = Animator.StringToHash("IsDead");

    //������ ����� ���õ� ������
    public AudioClip bossHowling;
    public AudioClip bossMove;
    public AudioClip bossNormalAttack;
    public AudioClip bossStoneAttack;
    public AudioClip bossDead;

    private AudioSource audioSouce;

    private float hitCooldown = 1.0f; // ��ٿ� �ð�
    private float lastHitTime = 0.0f;

    void Awake()
    {
        // �÷��̾� ���� ������Ʈ ���� // ����Ƽ ������ ���� �����Ÿ� ���ʿ��ҵ�
        var player = GameObject.FindGameObjectWithTag("Player");


        // �÷��̾��� Transform ������Ʈ ����
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        }

        // ���� Transform ������Ʈ ����
        bossTransform = GetComponent<Transform>();

        moveAgent = GetComponent<BossMoveAgent>();

        bossAttack = GameObject.Find("NormalAttack").GetComponent<BossAttack>();

        stoneAttack = GameObject.FindGameObjectWithTag("Rock").GetComponent<BossStoneAttack>();

        stoneAttack.gameObject.SetActive(false);

        animator = GetComponent<Animator>();

        enemyDamage = GetComponent<EnemyDamage>();

        audioSouce = GetComponent<AudioSource>();
        // �ڷ�ƾ ���� �ð� ����
    }

    void PlaySound (string action)
    {
        switch(action)
        {
            case "HOWLING":
                audioSouce.clip = bossHowling;
                break;
            case "MOVE":
                audioSouce.clip = bossMove;
                break;
            case "NORMAL_ATTACK":
                audioSouce.clip = bossNormalAttack;
                break;
            case "STONE_ATTACK":
                audioSouce.clip = bossStoneAttack;
                break;
            case "DEAD":
                audioSouce.clip = bossDead;
                break;
        }
        audioSouce.Play();
    }

    // Update is called once per frame
    void OnEnable()
    {
        StartCoroutine(CheckState());

        StartCoroutine(Action());

    }

    // �� ĳ������ ���¸� �˻��ϴ� �ڷ�ƾ �Լ�
    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);

        normalAttackCheck = false;
        bool isStoneAttack = false;

        while (!isDie)
        {
            // ���°� �����̸� �ڷ�ƾ �Լ��� ����
            if (state == State.DIE)
            {

                yield break;
            }


            // �÷��̾�� ������ �Ÿ��� ���           
            float dist = Vector3.Distance(playerTransform.position, bossTransform.position);


            // ���� �����Ÿ� �̳��� ���
            if (enemyDamage.hp > 490)
            {
                state = State.IDLE;
                yield return new WaitForSeconds(1.0f);
            }
            else if (dist <= attackDist)
            {
                // ���� Ʈ���� Ȱ��ȭ
                state = State.NORMAL_ATTACK;
                animator.SetTrigger("NormalAttack");
               // yield return new WaitForSeconds(0.4f);

                bossAttack.gameObject.SetActive(true);
               
              //  yield return new WaitForSeconds(0.15f);

                // ���� ������ �� �ٽ� ���� ����
                bossAttack.gameObject.SetActive(false);
                state = State.IDLE;

               // yield return new WaitForSeconds(2.0f);
            }
            else if (dist <= stoneAttackDist) //&& isStoneAttack == false)
            {
                if (!isStoneAttack)
                {
                    state = State.STONE_ATTACK;
                    animator.SetTrigger("StoneAttack");
                    yield return new WaitForSeconds(1.7f);

                    stoneAttack.gameObject.SetActive(true);
                    moveAgent.attackTarget = playerTransform.position;
                    PlaySound("STONE_ATTACK");

                    stoneAttack.StoneAttack();
                    yield return new WaitForSeconds(0.6f);

                    stoneAttack.gameObject.SetActive(false);
                    stoneAttack.StoneAttackEnd();

                    yield return new WaitForSeconds(2.6f);
                    isStoneAttack = true;
                }
            }
            else if (isStoneAttack == true)
            {
                state = State.IDLE;
                isStoneAttack = false;
            }
            else
            {
                state = State.MOVE;
            }

            //0.3�� ���� ����ϴ��߿��� ������� �纸?
            yield return ws;
        }

    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            // ���¿� ���� �б� ó��

            switch (state)
            {
                case State.IDLE:
                    animator.SetTrigger("Idle");
                    break;

                case State.MOVE:
                    animator.SetTrigger("Move");
                    moveAgent.traceTarget = playerTransform.position;
                    PlaySound("MOVE");
                    break;
                case State.NORMAL_ATTACK:
                    moveAgent.Stop();
                    animator.SetTrigger("NormalAttack");
                    moveAgent.attackTarget = playerTransform.position;
                    PlaySound("NORMAL_ATTACK");
                    // ���� �ִϸ��̼� ���� �� ���� ����
                    AnimatorStateInfo stateInfoHit = animator.GetCurrentAnimatorStateInfo(0);
                    if (stateInfoHit.normalizedTime >= 1.0f)
                    {
                        Debug.Log("��Ʈ �ִϸ��̼��� ���������� �������ϴ�.");
                        state = State.IDLE;
                    }
                    break;

                case State.STONE_ATTACK:
                    moveAgent.Stop();
                    animator.SetTrigger("StoneAttack");
                    moveAgent.attackTarget = playerTransform.position;

                    // Ư�� ����, ���ʿ� �ѹ� ���� �Ȱ��� �������� ������ ���� ������
                    break;
                case State.DIE:
                   moveAgent.Stop();
                animator.SetTrigger("Dead");
                yield return new WaitForSeconds(4.8f);  // ���� ó�� ���
                isDie = true;

                if (isDie)
                {
                    GetComponent<Animator>().speed = 0.0f;
                    PlaySound("DEAD");
                }
                    break;
            }
        }
    }


}
