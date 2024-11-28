using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{

    // NavMestAgent ������Ʈ�� ������ ����  // NavMestAgent�� Enemy 
    private NavMeshAgent agent;

    // �� ĳ������ Transform ������Ʈ�� ������ ����
    private Transform enemyTransform;


    // ���� ������ �����ϴ� ����
    public List<Transform> wayPoints;

    // ���� ���� ������ �迭�� index
    public int nextIdex = 0;

    // ����, ��ô���� �̵��ӵ�
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private readonly float attackSpeed = 10.0f;
    private readonly float stoneAttack = 30.0f;

    //  ���� ���θ� �Ǵ��ϴ� ���� 
    private bool _patrolling;

    public GameObject group;

    // �������� �ο��� ����

    int randomIndex = 0;

    // �ִϸ����Ϳ� ����


    // ȸ���� �ӵ� ������ ���� 
    private float damping = 1.0f;

    // �� �±� ����

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

    // ���� ����� ��ġ�� �Ǵ��ϴ� ����
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set 
        {   
            if(IsWallBetween(transform.position, value))
            {
                Debug.Log("�� �浹");
                return;
            }
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
            if (IsWallBetween(transform.position, value))
            {
                Debug.Log("�� �浹");
                return;
            }
            _attackTarget = value;
            agent.speed = attackSpeed;
            damping = 7.0f;
            TraceTarget(_attackTarget);
        }
    }

    // ���� ������ ������ ����� ��ġ�� �Ǵ��ϴ� ����
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
            Debug.Log("���� ����");
            if(hit.collider.CompareTag(wallTag) || hit.collider.CompareTag(hardWallTag))
            {
                Debug.Log("üũ����");
                return true; 
            }
        }
       return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyTransform = GetComponent<Transform>();
        // NavMeshAgent ������Ʈ�� ������ �� ������ ����
        agent = GetComponent<NavMeshAgent>();
        //�������� ����������� �ӵ��� ���̴� �ɼ��� ��Ȱ��ȭ
        agent.autoBraking = false;

        //�ڵ����� ȸ���ϴ� ����� ��Ȱ��ȭ
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

    // ���� ���������� �̵� ����� ����  // �����Ҷ��� �ϳ��� ����
    void MoveWayPoint()
    {
       
        //�ִ� �Ÿ� ��� ����� ������ ������ ���� x
        if (agent.isPathStale) return;

        // ���� �������� wayPoints �迭���� ������ ��ġ�� ���� �������� ����
        agent.destination = wayPoints[randomIndex].position;
        // ������̼� ����� Ȱ��ȭ �ؼ� �̵��� ������
        agent.isStopped = false;
        
    }


    //�÷��̾ ������ �� �̵���Ű�� �Լ�
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;        
        agent.isStopped = false;
    }

    // ���� �� ������ ������Ű�� �Լ�
    public void Stop()
    {
        agent.isStopped = true;
        //�ٷ� �����ϱ� ���� �ӵ��� 0���� ����
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Update()
    {
        // �� ĳ���Ͱ� �̵����϶��� ȸ��
        if(agent.isStopped == false)
        {
            // ������ ���� ���͸� ���ʹϾ� Ÿ���� ������ ��ȯ
            Quaternion rot =Quaternion.LookRotation(agent.desiredVelocity);
            // ���� �Լ��� ����� ���������� ȸ����Ŵ
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, rot, Time.deltaTime * damping);
        }

        //���� ��尡 �ƴ� ��� ���� ������ �������� ����
        if (!_patrolling) return;


        // NavMeshAgent�� �̵��ϰ� �ְ� �������� ���� �ߴ��� ���θ� ���
        if (agent.velocity.sqrMagnitude >= (0.2f * 0.2f)
            && agent.remainingDistance <= 0.5f)
        {
            //���� �������� �迭 ÷�ڸ� ���
            nextIdex = ++nextIdex % wayPoints.Count;
            //���� �������� �̵� ����� ����
            randomIndex = Random.Range(0, wayPoints.Count);
            MoveWayPoint();
        }
    }
}
