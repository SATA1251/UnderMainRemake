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


            // ���� �����Ÿ� �̳��� ���
            if (enemyDamage.hp > 490)
            {
                state = State.IDLE;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {          
                
                    if (dist > attackDist) // ��Ÿ� ���̸� �̵� �� ����
                    {
                        state = State.MOVE;                                 
                    }
                    else if (dist <= attackDist)//&& !normalAttackCooldown) // ��Ÿ� ���̸� ����
                    {
                        state = State.NORMAL_ATTACK;
                        CoolsownRoutine();
                     }         
                    
                    if (dist <= stoneAttackDist && !stoneAttackCooldown) // ���� ���� ��Ÿ� ���̸� ����
                    {
                        state = State.STONE_ATTACK;
                        yield return new WaitForSeconds(1.0f); // ���� ���� �ִϸ��̼� �ð�
                                                               //isStoneAttack = true;
                    } 
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
                    animator.SetBool("IsStoneAttack", false);
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsMove", false);
                    TriggerHit("Idle");
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
                    moveAgent.Stop();

                    animator.SetBool("IsAttack", true);
                    animator.SetBool("IsStoneAttack", false);
                    animator.SetBool("IsMove", false);
                    moveAgent.attackTarget = playerTransform.position;


                    // ���� �ִϸ��̼� ���� �� ���� ����
                    AnimatorStateInfo stateInfoHit = animator.GetCurrentAnimatorStateInfo(0);
                    float currentTimeNormal = stateInfoHit.normalizedTime % 1.0f;

                    if (currentTimeNormal >= 0.6f && currentTimeNormal <= 0.8f)
                    {
                        bossAttack.gameObject.SetActive(true);
                        PlaySound("NORMAL_ATTACK");
                    }
                    else if (currentTimeNormal >= 0.8f)
                    {
                        bossAttack.gameObject.SetActive(false);
                        Debug.Log("��Ʈ �ִϸ��̼��� ���������� �������ϴ�.");
                        // animator.SetBool("IsAttack", false);
                       // CoolsownRoutine();
                        normalAttackTrace = true;
                        stoneAttackCooldown = false;
                        state = State.IDLE;
                    }
                    break;

                case State.STONE_ATTACK:
                    moveAgent.Stop();
                    //TriggerHit("StoneAttack");
                    // animator.SetTrigger("StoneAttack
                    animator.SetBool("IsStoneAttack", true);
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsMove", false);
                    moveAgent.attackTarget = playerTransform.position;                

                    AnimatorStateInfo stateInfoStoneHit = animator.GetCurrentAnimatorStateInfo(0);
                    float currentTimeStone = stateInfoStoneHit.normalizedTime % 1.0f; // ������ ����� �ð�
                    Debug.Log("normalizedTime: " + currentTimeStone);
                    if(currentTimeStone <= 0.2f)
                    {
                        stoneAttack.GetPlayerPosition(playerTransform.position); // ������Ʈ�� �������� �� �������� �ؼ� ������ �����
                    }

                    if (currentTimeStone >= 0.5f && currentTimeStone <= 0.7f)
                    {
                        PlaySound("STONE_ATTACK");
                        stoneAttack.gameObject.SetActive(true);
                        stoneAttack.StoneAttack();
                        Debug.Log("�� ������ ����");
                    }
                    else if(currentTimeStone >= 0.9f)
                    {
                        stoneAttackCooldown = true;
                        stoneAttack.StoneAttackEnd();
                        stoneAttack.gameObject.SetActive(false);
                        state = State.IDLE;
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
