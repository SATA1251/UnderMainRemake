using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    // 적 캐릭터의 추적 사정 거리의 범위  (부채꼴로 표현 할 예정

    public float viewRange = 15.0f;
    [Range(0,360)]

    // 적의 시야각
    public float viewAngle = 120.0f;

    private Transform enemyTransform;
    private Transform playerTransform;

    private int playerLayer;
    private int obstacleLayer;
    private int layerMask;
    void Start()
    {
        enemyTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //레이어 마스크 값 계산
        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("ObstacleLayer");
        layerMask = 1 << playerLayer | 1 << obstacleLayer;
    }

    // 주어진 각도에 의해 원주 위의 점의 좌푯값을 계산하는 함수
    public Vector3 CirclePoint(float angle)
    {
        // 로컬 좌표계를 기준으로 설정 하기 위해 적 캐릭터의 y회전값을 더함
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad)
            , 0 
            , Mathf.Cos(angle * Mathf.Deg2Rad));  
    }

    // 플레이어 추적함수
    public bool isTracePlayer()
    {
        bool isTrace = false;

        // 추적반경 범위 안에서 주인공 캐릭터를 추출

        Collider[] colliders = Physics.OverlapSphere(enemyTransform.position
                                                            , viewRange
                                                            , 1 << playerLayer);

        //Physics.OverlapSphere - 중점과 반지름으로 가상의 원을 만들어 추출하려는 반경 이내에 들어와 있는 콜라이더들을 반환하는 함수

        // 배열의 개수가 1일 때 주인공이 범위 안에 있다고 판단할것임

        if(colliders.Length == 1)
        {
            // 적과 플레이어의 거리 계산
            Vector3 dir = (playerTransform.position - enemyTransform.position).normalized;

            // 플레이어가 부채꼴의 시야각에 들어왔는가?

            if(Vector3.Angle(enemyTransform.forward , dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }
        return isTrace;
    }

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit; // 레이캐스트

        //적 캐릭터와 주인공 사이의 방향벡터를 계산
        Vector3 dir = (playerTransform.position - enemyTransform.position).normalized;

        if(Physics.Raycast(enemyTransform.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag("Player"));
        }

        return isView;
    }
}
