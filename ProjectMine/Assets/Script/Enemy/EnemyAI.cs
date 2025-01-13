using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State  //몬스터의 상태를 나타냄
    {
        PATROL,
        TRACE,
        CHARGE,
        ATTACK,
        HIT,
        DIE
    }

    //변수 인덱스 정하기
    public int objectID;

    // 상태를 저장할 변수
     public State state = State.PATROL;

    public PlayerController playerController;

    // 플레이어의 위치를 저장할 변수
    private Transform playerTransform;

    // 적의 위치를 저장 할 변수
    private Transform enemyTransform;

    // 공격 사정거리
    public float attackDist = 2.0f;

    // 추적 사정거리
    public float traceDist = 10.0f;

    // 돌진 종료를 위한 변수
    public float chargeDist = 1.0f;

    // 사망 여부를 판단할 변수
    public bool isDie = false;


    // 코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;

    // 이동 관련 클래스 호출
    private MoveAgent moveAgent;

    // 시야각을 위한 클래스 호출
    private EnemyFOV enemyFov;

    private EnemyCtrl enemyCtrl;

    // 애니메이터 컴포넌트를 저장할 변수

    private Animator animator;

    private readonly int hashMove = Animator.StringToHash("IsMove");

    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("IsHit");
    private readonly int hashChage = Animator.StringToHash("IsChage");
    private readonly int hashDead = Animator.StringToHash("IsDead");

    //적의 사운드와 관련된 변수들

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
        // 플레이어 게임 오브젝트 추출 // 유니티 내에서 따로 넣을거면 불필요할듯
      var player = GameObject.Find("Player");
      
        // 플레이어의 Transform 컴포넌트 추출
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
            playerController = player.GetComponent<PlayerController>();
        }

        // 적의 Transform 컴포넌트 추출
        enemyTransform = GetComponent<Transform>();

        
        // 코루틴의 지연시간 생성
        ws = new WaitForSeconds(0.3f);

       moveAgent = GetComponent<MoveAgent>();

        enemyFov = GetComponent<EnemyFOV>();

        enemyCtrl = GameObject.Find("EnemyCtrl").GetComponent<EnemyCtrl>();
        //게임오브젝의 컴포넌트를 찾아서 갖고옴

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

    // 적 캐릭터의 상태를 검사하는 코루틴 함수
    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);
        // 상태를 체크할때 사용할 시간 변수
        

        while(!isDie)
        {
            // 상태가 죽음이면 코루틴 함수를 종료
            if(state == State.DIE) yield break;

            // 플레이어와 적과의 거리를 계산

            float dist = Vector3.Distance(playerTransform.position , enemyTransform.position);

            if (dist <= attackDist)
            // 공격 사정거리 이내인 경우
            {
                
              if (enemyFov.isViewPlayer())
              { 
                    state = State.CHARGE;
                    //    // 여기에 기모으는 애니메이션
                    yield return new WaitForSeconds(1.0f);
                    state = State.ATTACK;
                    // 여기에 공격(돌진) 애니메이션
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

            //0.3초 동안 대기하는중에는 제어권을 양보?
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

            // 상태에 따른 분기 처리
             
            switch (state)
            {
                case State.PATROL:
                    //순찰
                    moveAgent.patrolling = true;
                    animator.SetBool(hashChage, false);
                    animator.SetBool(hashAttack, false);
                    animator.SetBool(hashMove, true);
                    animator.SetBool(hashHit, false);
                    animator.SetBool(hashDead, false);
                    PlaySound("MOVE");
                    break;
                case State.TRACE:
                    //주인공의 위치를 넘겨 추적 모드로 변경

                    moveAgent.traceTarget = playerTransform.position;
                    animator.SetBool(hashChage, false);
                    animator.SetBool(hashAttack, false);
                    animator.SetBool(hashMove, true);
                    animator.SetBool(hashHit, false);
                    animator.SetBool(hashDead, false);
                    PlaySound("MOVE");
                    break;
                case State.CHARGE:
                    // 힘을 모으는 애니메이션 출력할예정
                    moveAgent.Stop(); //일단 멈추고 시간 재기
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
                    //구현이 안되어있으니 일단 정지
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
                        Debug.Log("히트 애니메이션이 성공적으로 끝났습니다.");
                        state = State.TRACE;
                    }
                    //yield return new WaitForSeconds(0.15f); // 실제 애니메이션 시간을 체크해서 상수로 넣었음, 당장은 괜찮은데 이쁘지않다
                    break;
                case State.DIE:
                    //구현이 안되어있으니 일단 정지
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
