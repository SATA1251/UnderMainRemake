using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StatController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;     //스탯들이 다른 스크립트에 나눠져있어서 객체 3개 받아옴  
    [SerializeField] private HandController handController;
    [SerializeField] private Hand hand;

    [SerializeField] private UpgradeAbility ua;
    public Ability currentAbility;                  //현재 설정되어 있는 능력
    private float drainTimer;                       //초당 데미지를 위해

    //플레이어의 스탯을 초기 상태로
    [HideInInspector] public void ResetStatus()                      
    {
        //상수를 사용해야할까? Start()를 만들어서 거기서 초기 스탯을 기억하게 하면..
        //시간 남으면 고쳐보자
        playerController.MaxHP = 500;

        hand.damage = 50;
        hand.attackDelay = 1.5f;
        hand.attackDelayA = 1.2f;
        hand.attackDelayB = 0;
        handController.attackAnimSpeed = 1.0f;

        handController.MaxOxygen = 200;
        handController.AttackOxygen = 70;
        handController.DodgeOxygen = 70;

        playerController.walkSpeed = 5.0f;
        hand.criticalChance = 10.0f;
        hand.criticalDamage = 20;
        playerController.damageDecreaseAmount = 0;
        playerController.damageDecreaseAmountBoss = 0;

        playerController.dodgeSpeed = 25;
        playerController.dodgeAnimSpeed = 1.0f;
        playerController.dodgeTimeCheck = 0.3f;
    }

    [HideInInspector]
    public void ResetStatusToTitle()
    {
        playerController.MaxHP = 500;
        playerController.HP = playerController.MaxHP;

        hand.damage = 50;
        hand.attackDelay = 1.5f;
        hand.attackDelayA = 1.2f;
        hand.attackDelayB = 0;
        handController.attackAnimSpeed = 1.0f;

        handController.MaxOxygen = 200;
        handController.AttackOxygen = 70;
        handController.DodgeOxygen = 70;
        handController.Oxygen = handController.MaxOxygen;

        playerController.walkSpeed = 5.0f;
        hand.criticalChance = 10.0f;
        hand.criticalDamage = 20;
        playerController.damageDecreaseAmount = 0;
        playerController.damageDecreaseAmountBoss = 0;

        playerController.dodgeSpeed = 25;
        playerController.dodgeAnimSpeed = 1.0f;
        playerController.dodgeTimeCheck = 0.3f;
    }

    [HideInInspector] public void UpgradeStatus(Ability abil)
    {
        //능력을 새로 얻을 때 스탯을 초기화함
        ResetStatus();

        //왜 고유ID 같은걸 사용하는지 알겠다
        switch (abil.abilityID)
        {
            case "ID0001":
                Debug.Log("StatController 광전사");

                playerController.MaxHP = playerController.MaxHP * 0.25f;
                playerController.damageDecreaseAmount = 70.0f;                    //능력은 중복이 안된다고 한다.. 이렇게 해도 문제는 없을듯
                playerController.damageDecreaseAmountBoss = 40.0f;
                hand.damage *= 2.0f;
                playerController.HP = playerController.MaxHP;

                break;

            case "ID0002":
                Debug.Log("StatController 묵직한 한 방");

                hand.attackDelay *= 3f;
                hand.attackDelayA *= 2.8f;
                handController.attackAnimSpeed *= 0.4f;
                hand.damage *= 5.0f;
                hand.criticalDamage = hand.damage;

                break;

            case "ID0003":
                Debug.Log("StatController 드레인");

                handController.Oxygen = 0;
                handController.MaxOxygen = 0;
                handController.AttackOxygen = 0;
                handController.DodgeOxygen = 0;

                handController.isDrain = true;          //true를 넘겨주면 공격할떄마다 체력을 회복한다
                ///초당 데미지는 아래 HP 관리 함수에서 처리함

                break;

            case "ID0004":
                Debug.Log("StatController 연금술");

                ///이건 아래 HP 관리하는 함수에서 처리함

                break;

            case "ID0005":
                Debug.Log("StatController 어류");

                handController.AttackOxygen -= handController.AttackOxygen * 0.7f;
                handController.DodgeOxygen -= handController.DodgeOxygen * 0.7f;
                handController.MaxOxygen += handController.MaxOxygen * 0.5f;
                handController.Oxygen = handController.MaxOxygen;       //현재 산소량을 최대치로

                break;

            case "ID0006":
                Debug.Log("StatController 금강불괴");

                playerController.damageDecreaseAmount = 40.0f;              //데미지 감소 효과
                playerController.damageDecreaseAmountBoss = 20.0f;
                playerController.HP += playerController.MaxHP * 0.1f;    //체력 회복 효과

                break;

            case "ID0007":
                Debug.Log("StatController 산소파이프 강화");

                ///OxygenPipe에서 조절해야한대 플레이어에 있는 버프 지속시간은 지워 그냥
                handController.MaxOxygen += 100.0f;
                hand.damage += 20;
                //치명타 피해는 기본 공격력의 20이 증가하는 함수가 이미 있으니 치명타 피해증가효과를 따로 구현할 필요는 없을듯

                break;

            case "ID0008":
                Debug.Log("StatController 히든");

                ///이건 안쓸거 같다

                break;

            case "ID0009":
                Debug.Log("StatController 광물 마스터");

                ///공격함수에서 처리하는거 같으니 거기서 대상의 Tag를 비교해서 처리하자

                break;

            case "ID0010":
                Debug.Log("StatController 회피 마스터");

                handController.DodgeOxygen = 0;
                playerController.dodgeSpeed *= 1.5f;            //가속을 1.5배로 하고
                playerController.dodgeAnimSpeed *= 1.5f;        //애니메이션 속도도 1.5배
                playerController.dodgeTimeCheck *= 2/3f;        //그리고 가속시간을 줄인다

                break;

            case "ID0011":
                Debug.Log("StatController 바람돌이");

                hand.attackDelay *= 0.7f;
                hand.attackDelayA *= 0.7f;
                handController.attackAnimSpeed *= 1.3f;

                playerController.damageDecreaseAmount -= 20.0f;
                playerController.damageDecreaseAmountBoss -= 20.0f;

                break;

            case "ID0012":
                Debug.Log("StatController 무호흡");

                handController.MaxOxygen -= 60.0f;
                hand.damage *= 1.5f;

                hand.attackDelay *= 0.5f;
                hand.attackDelayA *= 0.5f;
                handController.attackAnimSpeed *= 1.5f;

                handController.Oxygen = handController.MaxOxygen;

                break;

            default:
                Debug.Log("능력 없음 확인 필요");
                break;
        }

    }

    private void CheckMaxHP()
    {
        //현재 능력이 '연금술'이고 체력이 60% 미만으로 떨어지면
        if(currentAbility.abilityID == "ID0004" && playerController.HP < playerController.MaxHP * 0.6f)
        {
            //B등급 광물을 먼저 체크하고
            if(playerController.oresBamount > 0)
            {
                playerController.oresBamount--;                         //광물을 소모하고
                playerController.HP += playerController.MaxHP * 0.2f;   //체력을 20% 회복한다
            }
            //B등급 광물이 없으면 A등급 광물을 체크하고
            else if(playerController.oresAamount > 0)
            {
                playerController.oresAamount--;
                playerController.HP += playerController.MaxHP * 0.2f;
            }
            //둘다 없으면 S등급 광물을 체크하고
            else if(playerController.oresSamount > 0)
            {
                playerController.oresSamount--;
                playerController.HP += playerController.MaxHP * 0.2f;
            }
        }

        //현재 능력이 '드레인'이면
        if(currentAbility.abilityID == "ID0003")
        {
            drainTimer += Time.deltaTime;

            if(drainTimer > 1f) 
            {
                playerController.HP -= 5f;
                drainTimer = 0f;
            }
        }

        //현재 체력이 최대 체력을 넘어가면 최대 체력에 맞춘다
        if (playerController.HP > playerController.MaxHP)
        {
            playerController.HP = playerController.MaxHP;
        }
    }

    private void Update()
    {
        CheckMaxHP();
    }
}
