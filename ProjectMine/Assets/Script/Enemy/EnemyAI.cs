using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State  //������ ���¸� ��Ÿ��
    {
        PATROL,
        TRACE,
        CHARGE,
        ATTACK,
        HIT,
        DIE
    }

    //���� �ε��� ���ϱ�
    public int objectID;

    // ���¸� ������ ����
     public State state = State.PATROL;

    public PlayerController playerController;

    // �÷��̾��� ��ġ�� ������ ����
    private Transform playerTransform;

    // ���� ��ġ�� ���� �� ����
    private Transform enemyTransform;

    // ���� �����Ÿ�
    public float attackDist = 2.0f;

    // ���� �����Ÿ�
    public float traceDist = 10.0f;

    // ���� ���Ḧ ���� ����
    public float chargeDist = 1.0f;

    // ��� ���θ� �Ǵ��� ����
    public bool isDie = false;


    // �ڷ�ƾ���� ����� �����ð� ����
    private WaitForSeconds ws;

    // �̵� ���� Ŭ���� ȣ��
    private MoveAgent moveAgent;

    // �þ߰��� ���� Ŭ���� ȣ��
    private EnemyFOV enemyFov;

    private EnemyCtrl enemyCtrl;

    // �ִϸ����� ������Ʈ�� ������ ����

    private Animator animator;

    private readonly int hashMove = Animator.StringToHash("IsMove");

    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("IsHit");
    private readonly int hashChage = Animator.StringToHash("IsChage");
    private readonly int hashDead = Animator.StringToHash("IsDead");

    //���� ����� ���õ� ������

    public AudioClip enemyMove;
    public AudioClip enemyRush;
    public AudioClip enemyDead;

    private AudioSource audioSouce;

    private bool getAttackTarget = false;

    private int enemyTraceTime = 0;

    private float hitCooldown = 1.0f;

    private float lastHitTime = 0.0f;

    void Awake()
    {
        // �÷��̾� ���� ������Ʈ ���� // ����Ƽ ������ ���� �����Ÿ� ���ʿ��ҵ�
      var player = GameObject.Find("Player");
      
        // �÷��̾��� Transform ������Ʈ ����
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
            playerController = player.GetComponent<PlayerController>();
        }

        // ���� Transform ������Ʈ ����
        enemyTransform = GetComponent<Transform>();

        
        // �ڷ�ƾ�� �����ð� ����
        ws = new WaitForSeconds(0.3f);

       moveAgent = GetComponent<MoveAgent>();

        enemyFov = GetComponent<EnemyFOV>();

        enemyCtrl = GameObject.Find("EnemyCtrl").GetComponent<EnemyCtrl>();
        //���ӿ������� ������Ʈ�� ã�Ƽ� �����

        animator = GetComponent<Animator>();

        audioSouce = GetComponent<AudioSource>();
    }

    void PlaySound(string action)
    {
        switch(action)
        {
            case "MOVE":
                audioSouce.clip = enemyMove;
                break;
            case "RUSH":
                audioSouce.clip = enemyRush;
                break;
            case "DEAD":
                audioSouce.clip = enemyDead;
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
        // ���¸� üũ�Ҷ� ����� �ð� ����
        

        while(!isDie)
        {
            // ���°� �����̸� �ڷ�ƾ �Լ��� ����
            if(state == State.DIE) yield break;

            // �÷��̾�� ������ �Ÿ��� ���

            float dist = Vector3.Distance(playerTransform.position , enemyTransform.position);

            if (dist <= attackDist)
            // ���� �����Ÿ� �̳��� ���
            {
                
              if (enemyFov.isViewPlayer())
              { 
                    state = State.CHARGE;
                    //    // ���⿡ ������� �ִϸ��̼�
                    yield return new WaitForSeconds(1.0f);
                    state = State.ATTACK;
                    // ���⿡ ����(����) �ִϸ��̼�
                    yield return new WaitForSeconds(2.0f);
                    state = State.TRACE;
                }
               else
               {
                  state = State.TRACE;                             
               }
            }
            else if (enemyFov.isTracePlayer())
            {
                state = State.TRACE;               
            }          
            else
            {
                state = State.PATROL;               
            }

            if (!(enemyFov.isTracePlayer()))
            {
                yield return new WaitForSeconds(3.0f);
                state = State.PATROL;
            }

            //0.3�� ���� ����ϴ��߿��� ������� �纸?
            yield return ws;  
        }

    }

    IEnumerator Action()
    {
        float dist = Vector3.Distance(playerTransform.position, enemyTransform.position);
       
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        while (!isDie)
        {
            yield return ws;

            // ���¿� ���� �б� ó��
             
            switch (state)
            {
                case State.PATROL:
                    //����
                    moveAgent.patrolling = true;
                    animator.SetBool(hashChage, false);
                    animator.SetBool(hashAttack, false);
                    animator.SetBool(hashMove, true);
                    animator.SetBool(hashHit, false);
                    animator.SetBool(hashDead, false);
                    PlaySound("MOVE");
                    break;
                case State.TRACE:
                    //���ΰ��� ��ġ�� �Ѱ� ���� ���� ����

                    moveAgent.traceTarget = playerTransform.position;
                    animator.SetBool(hashChage, false);
                    animator.SetBool(hashAttack, false);
                    animator.SetBool(hashMove, true);
                    animator.SetBool(hashHit, false);
                    animator.SetBool(hashDead, false);
                    PlaySound("MOVE");
                    break;
                case State.CHARGE:
                    // ���� ������ �ִϸ��̼� ����ҿ���
                    moveAgent.Stop(); //�ϴ� ���߰� �ð� ���
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack, false);
                    animator.SetBool(hashChage, true);
                    animator.SetBool(hashHit, false);
                    animator.SetBool(hashDead, false);
                    getAttackTarget = false;

                    //yield return new WaitForSeconds(1f);
                    break;

                case State.ATTACK:
                   //if(getAttackTarget == false)
                   //{
                        moveAgent.attackTarget = playerTransform.position;
                        getAttackTarget = true;
                    //}
                    animator.SetBool(hashChage, false);
                    animator.SetBool(hashAttack, true);
                     PlaySound("RUSH");
                    break;
                case State.HIT:
                    //������ �ȵǾ������� �ϴ� ����
                    moveAgent.Stop();
                    TriggerHit();
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashChage, false);
                    animator.SetBool(hashAttack, false);                  
                    animator.SetBool(hashDead, false);

                    AnimatorStateInfo stateInfoHit = animator.GetCurrentAnimatorStateInfo(0);
                    float currentTimeNormal = stateInfoHit.normalizedTime % 1.0f;
                    if (currentTimeNormal >= 0.9f)
                    {
                        Debug.Log("��Ʈ �ִϸ��̼��� ���������� �������ϴ�.");
                        state = State.TRACE;
                    }
                    //yield return new WaitForSeconds(0.15f); // ���� �ִϸ��̼� �ð��� üũ�ؼ� ����� �־���, ������ �������� �̻����ʴ�
                    break;
                case State.DIE:
                    //������ �ȵǾ������� �ϴ� ����
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashChage, false);
                    animator.SetBool(hashAttack, false);
                    animator.SetBool(hashHit, false);
                    animator.SetBool(hashDead, true);
                    PlaySound("DEAD");
                    if(CompareTag("Enemy"))
                    {
                        int num = UnityEngine.Random.Range(1, 3);
                        if(num == 1)
                        {
                            playerController.oresBamount++;
                        }
                    }
                    else if(CompareTag("Enemy2"))
                    {
                        int num = UnityEngine.Random.Range(1, 3);
                        if (num == 1)
                        {
                            playerController.oresAamount++;
                        }
                    }
                    
                    yield return new WaitForSeconds(stateInfo.length - stateInfo.normalizedTime * stateInfo.length);
                    Destroy(gameObject);
                    break;
            }
        }
    }

    public void TriggerHit()
    {
        if (Time.time - lastHitTime > hitCooldown)
        {
            animator.SetTrigger("HitTrigger");
            lastHitTime = Time.time;
        }
    }

    void Update()
    {
    }
}
