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
    public float attackDist = 3.0f;

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

    public bool normalAttackCooldown = false;
    private float normalAttackCoolTime= 2.0f;

    public bool normalAttackTrace = false;

    public bool stoneAttackCooldown = false;

    private bool isActionInProgress = false;

    bool isNormalAttackInitialized = false; // �ִϸ��̼� �ʱ�ȭ �÷���

    bool isStoneAttackInitialized = false; // �ִϸ��̼� �ʱ�ȭ �÷���

    MeshRenderer stoneMeshRenderer;

    UnityEngine.Vector3 playerPosition;
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

        bossAttack.gameObject.SetActive(false);

        stoneAttack.gameObject.SetActive(false);

       // stoneMeshRenderer = stoneAttack.GetComponent<MeshRenderer>();

        //if(stoneMeshRenderer != null)
        //{
        //    stoneMeshRenderer.enabled = false;
        //}

        animator = GetComponent<Animator>();

        enemyDamage = GetComponent<EnemyDamage>();

        audioSouce = GetComponent<AudioSource>();
        // �ڷ�ƾ ���� �ð� ����
    }

    // ���� �׼ǿ� ���� Ŭ�� �̸��� ��ȯ
    string GetClipName(string action)
    {
        return action switch
        {
            "HOWLING" => bossHowling.name,
            "MOVE" => bossMove.name,
            "NORMAL_ATTACK" => bossNormalAttack.name,
            "STONE_ATTACK" => bossStoneAttack.name,
            "DEAD" => bossDead.name,
            _ => null
        };
    }

    void PlaySound (string action)
    {
        if (audioSouce.isPlaying)
        {
            // �̹� ��� ���� Ŭ���� ������ ���, ������� ����
            if (audioSouce.clip != null && audioSouce.clip.name == GetClipName(action))
                return;
        }

        switch (action)
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

            if (!isActionInProgress)
            {
                if (enemyDamage.hp > 990)
                {
                    state = State.IDLE;
                    isActionInProgress = true;
                    yield return new WaitForSeconds(2.0f);
                    isActionInProgress = false;
                }
                else
                {
                    if (dist <= stoneAttackDist && !stoneAttackCooldown && dist >= attackDist)
                    {
                        state = State.STONE_ATTACK;
                        isActionInProgress = true;
                        yield return new WaitForSeconds(2.0f);
                        isActionInProgress = false;
                    }
                    else if (dist <= attackDist)
                    {
                        state = State.NORMAL_ATTACK;
                        isActionInProgress = true;
                        //CoolsownRoutine();
                        yield return new WaitForSeconds(2.0f);
                        isActionInProgress = false;
                    }
                    else if (dist > attackDist)
                    {
                        state = State.MOVE;
                    }
                }
            }

            //// ���� �����Ÿ� �̳��� ���
            //if (enemyDamage.hp > 990)
            //{
            //    state = State.IDLE;
            //    yield return new WaitForSeconds(1.0f);
            //}
            //else
            //{

            //    if (dist > attackDist) // ��Ÿ� ���̸� �̵� �� ����
            //    {
            //        state = State.MOVE;
            //    }
            //    else if (dist <= attackDist)//&& !normalAttackCooldown) // ��Ÿ� ���̸� ����
            //    {
            //        state = State.NORMAL_ATTACK;
            //        CoolsownRoutine();
            //    }

            //    if (dist <= stoneAttackDist && !stoneAttackCooldown) // ���� ���� ��Ÿ� ���̸� ����
            //    {
            //        state = State.STONE_ATTACK;
            //        yield return new WaitForSeconds(1.0f); // ���� ���� �ִϸ��̼� �ð�
            //                                               //isStoneAttack = true;
            //    }
            //}

            //0.3�� ���� ����ϴ��߿��� ������� �纸?
            yield return ws;

            if (enemyDamage.hp <= 0)
            {
                state = State.DIE;
            }
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
                    animator.SetBool("IsStoneAttack", false);
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsMove", false);
                    TriggerHit("Idle");
                    yield return new WaitForSeconds(1.0f);
                    break;

                case State.MOVE:
                    animator.SetBool("IsMove", true);
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsStoneAttack", false);
                    moveAgent.traceTarget = playerTransform.position;
                    normalAttackCooldown = false;
                    PlaySound("MOVE");
                    break;
                case State.NORMAL_ATTACK:

                    animator.SetBool("IsAttack", true);
                    animator.SetBool("IsStoneAttack", false);
                    animator.SetBool("IsMove", false);
                    moveAgent.attackTarget = playerTransform.position;
                    moveAgent.Stop();


                    // ���� �ִϸ��̼� ���� �� ���� ����
                    AnimatorStateInfo stateInfoHit = animator.GetCurrentAnimatorStateInfo(0);
                    if (stateInfoHit.IsName("rig|Attack_Stamp"))
                    {
                        float currentTimeNormal = stateInfoHit.normalizedTime % 1.0f;
                        Debug.Log("normalTime: " + currentTimeNormal);

                        //if (!isNormalAttackInitialized && currentTimeNormal < 0.1f)
                        //{
                        //    // �ִϸ��̼� ���� �ʱ�ȭ
                        //    isNormalAttackInitialized = true;
                        //    Debug.Log("NormalAttack animation initialized.");
                        //}

                        if (currentTimeNormal >= 0.75f && currentTimeNormal <= 0.8f)// && isNormalAttackInitialized)
                        {
                            bossAttack.gameObject.SetActive(true);
                            PlaySound("NORMAL_ATTACK");
                        }
                        //else if (currentTimeNormal >= 0.7f && currentTimeNormal <= 0.85f)
                        //{

                        //    Debug.Log("��Ʈ �ִϸ��̼��� ���������� �������ϴ�.");
                        //    // animator.SetBool("IsAttack", false);
                        //   // CoolsownRoutine();
                        //}
                        else if (currentTimeNormal >= 0.85f)// && isNormalAttackInitialized)
                        {
                            bossAttack.gameObject.SetActive(false);
                            animator.SetBool("IsAttack", false);
                            normalAttackTrace = true;
                            stoneAttackCooldown = false;
                            isNormalAttackInitialized = false;
                          
                            state = State.IDLE;

                            // �ʱ�ȭ ���� ����
                            Debug.Log("NormalAttack animation ended, state reset.");
                        }
                    }
                    else
                    {
                        // �ִϸ��̼� ���°� ����Ǿ����Ƿ� �ʱ�ȭ �÷��� ����
                        isNormalAttackInitialized = false;
                    }
                        break;

                case State.STONE_ATTACK:
                    animator.SetBool("IsStoneAttack", true);
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsMove", false);
                    moveAgent.attackTarget = playerTransform.position;
                    moveAgent.Stop();

                    AnimatorStateInfo stateInfoStoneHit = animator.GetCurrentAnimatorStateInfo(0);
                    float currentTimeStone = stateInfoStoneHit.normalizedTime % 1.0f; // ������ ����� �ð�
                    Debug.Log("stoneTime: " + currentTimeStone);
                   

                    if (stateInfoStoneHit.IsName("rig_Attack_Throw"))
                    {
                        // �ʱ�ȭ Ȯ�� �� ó��
                        if (!isStoneAttackInitialized && currentTimeStone < 0.1f)
                        {
                            isStoneAttackInitialized = true;
                            Debug.Log("StoneAttack animation initialized.");
                        }

                        if (currentTimeStone >= 0.4f && currentTimeStone <= 0.5f)
                        {                           
                            playerPosition = playerTransform.position;
                        }

                        if (currentTimeStone >= 0.7f && currentTimeStone <= 0.85f)// && isStoneAttackInitialized)
                        {
                            PlaySound("STONE_ATTACK");
                            //stoneMeshRenderer.enabled = true;
                            stoneAttack.gameObject.SetActive(true);
                            stoneAttack.GetPlayerPosition(playerPosition);
                            stoneAttack.StoneAttack();
                            Debug.Log("�� ������ ����");
                        }
                        else if (currentTimeStone >= 0.95f)// && isStoneAttackInitialized)
                        {
                            animator.SetBool("IsStoneAttack", false); // �ִϸ��̼� ���� �÷��� ����
                            stoneAttackCooldown = true;
                            stoneAttack.StoneAttackEnd();
                            //stoneMeshRenderer.enabled = false;
                            stoneAttack.gameObject.SetActive(false);
                            state = State.IDLE;

                            // �ʱ�ȭ ���� ����
                            isStoneAttackInitialized = false;
                            Debug.Log("StoneAttack animation ended, state reset.");
                        }
                    }
                    else
                    {
                        // �ִϸ��̼� ���°� ����Ǿ����Ƿ� �ʱ�ȭ �÷��� ����
                        isStoneAttackInitialized = false;
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
