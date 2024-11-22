using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMoveAgent : MonoBehaviour
{
    private NavMeshAgent agent;


    // 적 캐릭터의 Transform 컴포넌트를 저장할 변수
    private Transform bossTransform;

    //추척시의 이동속도
    private readonly float traceSpeed = 4.0f;

    // 회전시 속도 조절용 변수 
    private float damping = 7.0f;

    // 추적 대상의 위치를 판단하는 변수
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
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
            _attackTarget = value;
            agent.speed = 0.1f;
            damping = 7.0f;
            TraceTarget(_attackTarget);
        }
    }


    void Start()
    {
        bossTransform = GetComponent<Transform>();
        // NavMeshAgent 컴포넌트를 추출한 후 변수에 저장
        agent = GetComponent<NavMeshAgent>();
        //목적지에 가까워질수록 속도를 줄이는 옵션을 비활성화
        agent.autoBraking = false;
    }

    //플레이어를 추적할 때 이동시키는 함수
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    //추적을 정지시키는 함수
    public void Stop()
    {
        agent.isStopped = true;
        //바로 정지하기 위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        // 적 캐릭터가 이동중일때만 회전
        if (agent.isStopped == false)
        {
        // 가야할 방향 벡터를 쿼터니언 타입의 각도로 변환

            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            // 보간 함수를 사용해 점진적으로 회전시킴
            bossTransform.rotation = Quaternion.Slerp(bossTransform.rotation, rot, Time.deltaTime * damping);
        }
    }

}
