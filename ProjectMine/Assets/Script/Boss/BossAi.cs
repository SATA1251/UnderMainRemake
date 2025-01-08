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

    // 상태를 저장할 변수

    public State state = State.MOVE;

    // 플레이어의 위치를 저장할 변수
    private Transform playerTransform;

    // 적의 위치를 저장 할 변수
    private Transform bossTransform;

    // 공격 사정거리
    public float attackDist = 3.0f;

    public float stoneAttackDist = 50.0f;

    // 추적 사정거리
    public float traceDist = 100.0f;

    //보스의 체력및 공격을 저장할 변수
    public EnemyDamage enemyDamage;
    // 사망 여부를 판단할 변수
    public bool isDie = false;

    // 코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;

    // 이동 관련 클래스 호출
    private BossMoveAgent moveAgent;

    private BossAttack bossAttack;

    public BossStoneAttack stoneAttack;

    // 애니메이터 컴포넌트를 저장할 변수

    private Animator animator;

    // 일반 공격 체크용 변수
    public bool normalAttackCheck = false;

    public bool isAttack = false;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashStoneAttack = Animator.StringToHash("IsStoneAttack");
    private readonly int hashDead = Animator.StringToHash("IsDead");

    //보스의 사운드와 관련된 변수들
    public AudioClip bossHowling;
    public AudioClip bossMove;
    public AudioClip bossNormalAttack;
    public AudioClip bossStoneAttack;
    public AudioClip bossDead;

    private AudioSource audioSouce;

    private float hitCooldown = 1.0f; // 쿨다운 시간
    private float lastHitTime = 0.0f;

    public bool normalAttackCooldown = false;
    private float normalAttackCoolTime= 2.0f;

    public bool normalAttackTrace = false;

    public bool stoneAttackCooldown = false;

    void Awake()
    {
        // 플레이어 게임 오브젝트 추출 // 유니티 내에서 따로 넣을거면 불필요할듯
        var player = GameObject.FindGameObjectWithTag("Player");


        // 플레이어의 Transform 컴포넌트 추출
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        }

        // 적의 Transform 컴포넌트 추출
        bossTransform = GetComponent<Transform>();

        moveAgent = GetComponent<BossMoveAgent>();

        bossAttack = GameObject.Find("NormalAttack").GetComponent<BossAttack>();

        stoneAttack = GameObject.FindGameObjectWithTag("Rock").GetComponent<BossStoneAttack>();

        bossAttack.gameObject.SetActive(false);

        stoneAttack.gameObject.SetActive(false);

        animator = GetComponent<Animator>();

        enemyDamage = GetComponent<EnemyDamage>();

        audioSouce = GetComponent<AudioSource>();
        // 코루틴 시작 시간 저장
    }

    // 현재 액션에 따른 클립 이름을 반환
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
            // 이미 재생 중인 클립과 동일한 경우, 재생하지 않음
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

    // 적 캐릭터의 상태를 검사하는 코루틴 함수
    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);

        normalAttackCheck = false;
        bool isStoneAttack = false;

        while (!isDie)
        {
            // 상태가 죽음이면 코루틴 함수를 종료
            if (state == State.DIE)
            {

                yield break;
            }


            // 플레이어와 적과의 거리를 계산           
            float dist = Vector3.Distance(playerTransform.position, bossTransform.position);


            // 공격 사정거리 이내인 경우
            if (enemyDamage.hp > 490)
            {
                state = State.IDLE;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {          
                
                    if (dist > attackDist) // 사거리 밖이면 이동 후 공격
                    {
                        state = State.MOVE;                                 
                    }
                    else if (dist <= attackDist)//&& !normalAttackCooldown) // 사거리 안이면 공격
                    {
                        state = State.NORMAL_ATTACK;
                        CoolsownRoutine();
                     }         
                    
                    if (dist <= stoneAttackDist && !stoneAttackCooldown) // 스톤 공격 사거리 안이면 공격
                    {
                        state = State.STONE_ATTACK;
                        yield return new WaitForSeconds(1.0f); // 스톤 공격 애니메이션 시간
                                                               //isStoneAttack = true;
                    } 
            }

            //0.3초 동안 대기하는중에는 제어권을 양보?
            yield return ws;
        }

    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            // 상태에 따른 분기 처리

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


                    // 공격 애니메이션 종료 후 상태 변경
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
                        Debug.Log("히트 애니메이션이 성공적으로 끝났습니다.");
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
                    float currentTimeStone = stateInfoStoneHit.normalizedTime % 1.0f; // 루프를 고려한 시간
                    Debug.Log("normalizedTime: " + currentTimeStone);
                    if(currentTimeStone <= 0.2f)
                    {
                        stoneAttack.GetPlayerPosition(playerTransform.position); // 오브젝트가 꺼져있을 때 가져오려 해서 문제가 생긴다
                    }

                    if (currentTimeStone >= 0.5f && currentTimeStone <= 0.7f)
                    {
                        PlaySound("STONE_ATTACK");
                        stoneAttack.gameObject.SetActive(true);
                        stoneAttack.StoneAttack();
                        Debug.Log("돌 던지기 성공");
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
                     yield return new WaitForSeconds(4.8f);  // 죽음 처리 대기
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
