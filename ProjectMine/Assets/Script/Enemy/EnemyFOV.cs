using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    // �� ĳ������ ���� ���� �Ÿ��� ����  (��ä�÷� ǥ�� �� ����

    public float viewRange = 15.0f;
    [Range(0,360)]

    // ���� �þ߰�
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

        //���̾� ����ũ �� ���
        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("ObstacleLayer");
        layerMask = 1 << playerLayer | 1 << obstacleLayer;
    }

    // �־��� ������ ���� ���� ���� ���� ��ǩ���� ����ϴ� �Լ�
    public Vector3 CirclePoint(float angle)
    {
        // ���� ��ǥ�踦 �������� ���� �ϱ� ���� �� ĳ������ yȸ������ ����
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad)
            , 0 
            , Mathf.Cos(angle * Mathf.Deg2Rad));  
    }

    // �÷��̾� �����Լ�
    public bool isTracePlayer()
    {
        bool isTrace = false;

        // �����ݰ� ���� �ȿ��� ���ΰ� ĳ���͸� ����

        Collider[] colliders = Physics.OverlapSphere(enemyTransform.position
                                                            , viewRange
                                                            , 1 << playerLayer);

        //Physics.OverlapSphere - ������ ���������� ������ ���� ����� �����Ϸ��� �ݰ� �̳��� ���� �ִ� �ݶ��̴����� ��ȯ�ϴ� �Լ�

        // �迭�� ������ 1�� �� ���ΰ��� ���� �ȿ� �ִٰ� �Ǵ��Ұ���

        if(colliders.Length == 1)
        {
            // ���� �÷��̾��� �Ÿ� ���
            Vector3 dir = (playerTransform.position - enemyTransform.position).normalized;

            // �÷��̾ ��ä���� �þ߰��� ���Դ°�?

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
        RaycastHit hit; // ����ĳ��Ʈ

        //�� ĳ���Ϳ� ���ΰ� ������ ���⺤�͸� ���
        Vector3 dir = (playerTransform.position - enemyTransform.position).normalized;

        if(Physics.Raycast(enemyTransform.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag("Player"));
        }

        return isView;
    }
}
