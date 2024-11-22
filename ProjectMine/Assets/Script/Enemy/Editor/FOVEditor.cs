using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        //Enemy 클래스를 참조
        EnemyFOV fov = (EnemyFOV)target;

        // 원주 위의 시작점의 좌표를 계산
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        //원의 색상을 흰색으로 지정
        Handles.color = Color.yellow;

        //외관선만 표현하는 원반을 그림
        Handles.DrawWireDisc(fov.transform.position
            , Vector3.up
            , fov.viewRange);

        // 부채꼴 색상
        Handles.color = new Color(0, 255, 0, 0.2f);

        Handles.DrawSolidArc(fov.transform.position
            , Vector3.up
            , fromAnglePos
            , fov.viewAngle
            , fov.viewRange);

        // 시야각 임시 텍스트 표시
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f), fov.viewAngle.ToString());
    }
}
