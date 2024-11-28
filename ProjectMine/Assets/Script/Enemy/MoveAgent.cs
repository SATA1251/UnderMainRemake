using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{

    // NavMestAgent 컴포넌트를 저장할 변수  // NavMestAgent는 Enemy 
    private NavMeshAgent agent;

    // 적 캐릭터의 Transform 컴포넌트를 저장할 변수
    private Transform enemyTransform;


    // 순찰 지점을 저장하는 변수
    public List<Transform> wayPoints;

    // 다음 순찰 지점의 배열의 index
    public int nextIdex = 0;

    // 순찰, 추척시의 이동속도
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private readonly float attackSpeed = 10.0f;
    private readonly float stoneAttack = 30.0f;

    //  순찰 여부를 판단하는 변수 
    private bool _patrolling;

    public GameObject group;

    // 랜덤성을 부여할 변수

    int randomIndex = 0;

    // 애니메이터용 변수


    // 회전시 속도 조절용 변수 
    private float damping = 1.0f;

    // 벽 태그 설정

    public string wallTag = "Wall";
    public string hardWallTag = "HardWall";

    public float speed
    {
        get { return agent.velocity.magnitude; }
    }


    public bool patrolling
    {
        get { return _patrolling; }
        set { 
            _patrolling = value; 
            if(_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }

    // 추적 대상의 위치를 판단하는 변수
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set 
        {   
            if(IsWallBetween(transform.position, value))
            {
                Debug.Log("벽 충돌");
                return;
            }
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    // 공격 대상의 위치를 판단하는 변수
    private Vector3 _attackTarget;
    public Vector3 attackTarget
    {
        get { return _attackTarget; }
        set
        {
            if (IsWallBetween(transform.position, value))
            {
                Debug.Log("벽 충돌");
                return;
            }
            _attackTarget = value;
            agent.speed = attackSpeed;
            damping = 7.0f;
            TraceTarget(_attackTarget);
        }
    }

    // 보스 던지기 공격의 대상의 위치를 판단하는 변수
    private Vector3 _stoneAttackTarget;
    public Vector3 stoneAttackTarget
    {
        get { return _stoneAttackTarget; }
        set
        {
            _stoneAttackTarget = value;
            agent.speed = stoneAttack;
            damping = 14.0f;
            TraceTarget(_stoneAttackTarget);
        }
    }

    private bool IsWallBetween(Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        if(Physics.Raycast(start, (end - start).normalized, out hit, Vector3.Distance(start, end)))
        {
            Debug.Log("진입 성공");
            if(hit.collider.CompareTag(wallTag) || hit.collider.CompareTag(hardWallTag))
            {
                Debug.Log("체크성공");
                return true; 
            }
        }
       return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyTransform = GetComponent<Transform>();
        // NavMeshAgent 컴포넌트를 추출한 후 변수에 저장
        agent = GetComponent<NavMeshAgent>();
        //목적지에 가까워질수록 속도를 줄이는 옵션을 비활성화
        agent.autoBraking = false;

        //자동으로 회전하는 기능을 비활성화
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

            if (group != null)
            {
                group.GetComponentsInChildren<Transform>(wayPoints);

                wayPoints.RemoveAt(0);

                randomIndex = Random.Range(0, wayPoints.Count);

            }      
        MoveWayPoint();
    }

    // 다음 목적지까지 이동 명령을 내림  // 순찰할때의 하나의 패턴
    void MoveWayPoint()
    {
       
        //최단 거리 경로 계산이 끝나지 않으면 수행 x
        if (agent.isPathStale) return;

        // 다음 목적지를 wayPoints 배열에서 추출한 위치로 다음 목적지를 지정
        agent.destination = wayPoints[randomIndex].position;
        // 내비게이션 기능을 활성화 해서 이동을 시작함
        agent.isStopped = false;
        
    }


    //플레이어를 추적할 때 이동시키는 함수
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;        
        agent.isStopped = false;
    }

    // 순찰 및 추적을 정지시키는 함수
    public void Stop()
    {
        agent.isStopped = true;
        //바로 정지하기 위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Update()
    {
        // 적 캐릭터가 이동중일때만 회전
        if(agent.isStopped == false)
        {
            // 가야할 방향 벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot =Quaternion.LookRotation(agent.desiredVelocity);
            // 보간 함수를 사용해 점진적으로 회전시킴
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, rot, Time.deltaTime * damping);
        }

        //순찰 모드가 아닐 경우 이후 로직을 수행하지 않음
        if (!_patrolling) return;


        // NavMeshAgent가 이동하고 있고 목적지에 도착 했는지 여부를 계산
        if (agent.velocity.sqrMagnitude >= (0.2f * 0.2f)
            && agent.remainingDistance <= 0.5f)
        {
            //다음 목적지의 배열 첨자를 계산
            nextIdex = ++nextIdex % wayPoints.Count;
            //다음 목적지로 이동 명령을 수행
            randomIndex = Random.Range(0, wayPoints.Count);
            MoveWayPoint();
        }
    }
}
