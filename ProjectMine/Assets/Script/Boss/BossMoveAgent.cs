using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMoveAgent : MonoBehaviour
{
    private NavMeshAgent agent;


    // �� ĳ������ Transform ������Ʈ�� ������ ����
    private Transform bossTransform;

    //��ô���� �̵��ӵ�
    private readonly float traceSpeed = 4.0f;

    // ȸ���� �ӵ� ������ ���� 
    private float damping = 7.0f;

    // ���� ����� ��ġ�� �Ǵ��ϴ� ����
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

    // ���� ����� ��ġ�� �Ǵ��ϴ� ����
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
        // NavMeshAgent ������Ʈ�� ������ �� ������ ����
        agent = GetComponent<NavMeshAgent>();
        //�������� ����������� �ӵ��� ���̴� �ɼ��� ��Ȱ��ȭ
        agent.autoBraking = false;
    }

    //�÷��̾ ������ �� �̵���Ű�� �Լ�
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    //������ ������Ű�� �Լ�
    public void Stop()
    {
        agent.isStopped = true;
        //�ٷ� �����ϱ� ���� �ӵ��� 0���� ����
        agent.velocity = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        // �� ĳ���Ͱ� �̵����϶��� ȸ��
        if (agent.isStopped == false)
        {
        // ������ ���� ���͸� ���ʹϾ� Ÿ���� ������ ��ȯ

            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            // ���� �Լ��� ����� ���������� ȸ����Ŵ
            bossTransform.rotation = Quaternion.Slerp(bossTransform.rotation, rot, Time.deltaTime * damping);
        }
    }

}
