using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        //Enemy Ŭ������ ����
        EnemyFOV fov = (EnemyFOV)target;

        // ���� ���� �������� ��ǥ�� ���
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        //���� ������ ������� ����
        Handles.color = Color.yellow;

        //�ܰ����� ǥ���ϴ� ������ �׸�
        Handles.DrawWireDisc(fov.transform.position
            , Vector3.up
            , fov.viewRange);

        // ��ä�� ����
        Handles.color = new Color(0, 255, 0, 0.2f);

        Handles.DrawSolidArc(fov.transform.position
            , Vector3.up
            , fromAnglePos
            , fov.viewAngle
            , fov.viewRange);

        // �þ߰� �ӽ� �ؽ�Ʈ ǥ��
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f), fov.viewAngle.ToString());
    }
}
