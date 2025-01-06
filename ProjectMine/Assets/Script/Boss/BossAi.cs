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

    public BossStoneAttack stoneAttack;

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

    private bool normalAttackCooldown = false;
    private float normalAttackCoolTime= 7.0f;

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
            else if (dist <= attackDist && !normalAttackCooldown)
            {
                // ���� Ʈ���� Ȱ��ȭ
                state = State.NORMAL_ATTACK;

                normalAttackCooldown = true;
                StartCoroutine(CoolsownRoutine());

            }
            else if (dist <= stoneAttackDist) //&& isStoneAttack == false)
            {
                if (!isStoneAttack)
                {
                    state = State.STONE_ATTACK;


                    yield return new WaitForSeconds(2.6f);
                    isStoneAttack = true;
                }
            }
            else if (isStoneAttack == true)
            {
                state = State.IDLE;
                isStoneAttack = false;
                //animator.SetBool("IsAttack", false);
                //animator.SetBool("IsStoneAttack", false);
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
                    moveAgent.Stop();
                    TriggerHit("Idle");
                    break;

                case State.MOVE:
                    TriggerHit("Move");
                    moveAgent.traceTarget = playerTransform.position;
                    PlaySound("MOVE");
                    break;
                case State.NORMAL_ATTACK:
                    moveAgent.Stop();
                    TriggerHit("NormalAttack");
                    //animator.SetTrigger("NormalAttack");
                    moveAgent.attackTarget = playerTransform.position;
                    PlaySound("NORMAL_ATTACK");
                    // ���� �ִϸ��̼� ���� �� ���� ����
                    AnimatorStateInfo stateInfoHit = animator.GetCurrentAnimatorStateInfo(0);
                    float currentTimeNormal = stateInfoHit.normalizedTime % 1.0f;

                    if (currentTimeNormal >= 0.6f && currentTimeNormal <= 0.8f)
                    {
                        bossAttack.gameObject.SetActive(true);
                    }
                    else if (currentTimeNormal >= 0.9f)
                    {
                        bossAttack.gameObject.SetActive(false);
                    }

                    if (currentTimeNormal >= 1.0f)
                    {
                        Debug.Log("��Ʈ �ִϸ��̼��� ���������� �������ϴ�.");
                        state = State.IDLE;
                       // animator.SetBool("IsAttack", false);
                    }
                    break; 

                case State.STONE_ATTACK:
                    moveAgent.Stop();
                    TriggerHit("StoneAttack");
                   // animator.SetTrigger("StoneAttack");
                    moveAgent.attackTarget = playerTransform.position;
                    PlaySound("STONE_ATTACK");


                    AnimatorStateInfo stateInfoStoneHit = animator.GetCurrentAnimatorStateInfo(0);
                    float currentTimeStone = stateInfoStoneHit.normalizedTime % 1.0f; // ������ ����� �ð�
                    Debug.Log("normalizedTime: " + currentTimeStone);
                    if(currentTimeStone <= 0.2f)
                    {
                        stoneAttack.GetPlayerPosition();
                    }
                    if (currentTimeStone >= 0.5f && currentTimeStone <= 0.7f)
                    {
                        stoneAttack.gameObject.SetActive(true);
                        stoneAttack.StoneAttack();
                        Debug.Log("�� ������ ����");
                    }
                    else
                    {
                        stoneAttack.gameObject.SetActive(false);
                        stoneAttack.StoneAttackEnd();
                    }



                    break;
                case State.DIE:
                    moveAgent.Stop();
                     TriggerHit("Dead");
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

    public void TriggerHit(string triggerName)
    {
        if (Time.time - lastHitTime > hitCooldown)
        {
            animator.SetTrigger(triggerName);
            lastHitTime = Time.time;
        }
    }

    private IEnumerator CoolsownRoutine()
    {
        yield return new WaitForSeconds(normalAttackCoolTime);
        normalAttackCooldown = false;
    }
}
