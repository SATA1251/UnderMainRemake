using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string weaponName;

    // 웨폰 유형
    public bool isPickAxe;      // 곡괭이

    public int range;           // 공격범위
    public float applyDamage;  // 공격력 받아오는 변수명
    public float damage;       // 일반공격력
    public float criticalDamage; // 치명타 공격력
    public float criticalChance; // 치명타 확률
    public float attackDelay;   // 공격 딜레이
    public float attackDelayA;  // 공격 활성화 시점
    public float attackDelayB;  // 공격 비활성화 시점

    public Animator anim;       // 애니메이션
    

}
