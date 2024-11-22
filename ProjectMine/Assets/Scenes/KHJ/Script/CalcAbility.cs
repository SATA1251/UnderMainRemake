using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcAbility : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;     //스탯들이 다른 스크립트에 나눠져있어서 객체 3개 받아옴  
    [SerializeField] private HandController handController;
    [SerializeField] private Hand hand;

    public Ability currentAbility;                //현재 설정되어 있는 능력

    //능력을 가지고 있는 체크
    [SerializeField] private bool checkBerserk;                     //광전사
    [SerializeField] private bool checkOnePunch;                    //묵직한 한 방
    [SerializeField] private bool checkDrain;                       //드레인
    [SerializeField] private bool checkAlch;                        //연금술
    [SerializeField] private bool checkFish;                        //어류
    [SerializeField] private bool checkIron;                        //금강불괴
    [SerializeField] private bool checkOxyPipe;                     //산소파이프 강화
    [SerializeField] private bool checkHidden;                      //히든
    [SerializeField] private bool checkMining;                      //광물마스터
    [SerializeField] private bool checkDodge;                       //회피마스터
    [SerializeField] private bool checkWindy;                       //바람돌이
    [SerializeField] private bool checkApnea;                       //무호흡


}
