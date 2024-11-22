using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GameObject enemyGroup_1;
    public GameObject enemyGroup_2;

    public bool isHit1st = false;
    public bool isHit2nd = false;


    /// <summary>
    /// 5.12 배성근
    /// 컨트롤러에 먼저 누군가 맞았다는것을 올려보내고
    /// 맞았다는걸 알면 trace로 자식들에게 뿌려주어야 한다
    /// 
    /// 5.13 배성근
    /// 
    /// GameObject.Find를 통해 각 오브젝트에 들어있는 함수를 
    /// 따로 가져올 수 있다는 것을 알았다
    /// </summary>

}
