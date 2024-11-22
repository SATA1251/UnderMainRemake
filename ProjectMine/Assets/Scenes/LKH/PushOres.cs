using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PushOres : MonoBehaviour
{
    [SerializeField] private UIController uc;
    [SerializeField] private PlayerController pc;
    [SerializeField] private GameObject UIMain;
    [SerializeField] private GameObject UISelect;
    [SerializeField] private GameObject UIChooseAbility;
    [SerializeField] private UpgradeAbility ua;
    [SerializeField] private ProbabilityText prob;
    [SerializeField] private HealingText healingText;

    //스크린에 띄울 3개 능력을 정하는 함수
    private void AddToOnScreenList()
    {
        int rateS = prob.rateS;             //광물에 따라 정해진 확률을 가져온다
        int rateA = prob.rateA;
        int rateB = prob.rateB;

        for (int i = 0; i < 3; i++)         //3개를 골라야하니까
        {
            int randomNumber = UnityEngine.Random.Range(1, 100);

            if (randomNumber <= rateS)              //S확률
            {
                int randomInList = UnityEngine.Random.Range(0, ua.SabilityContainer.Count);    //다시 S등급중에서 랜덤으로 고르고

                ua.onScreenContainer.Add(ua.SabilityContainer[randomInList]);                     //화면에 띄울 리스트에 넣는다
            }
            else if (randomNumber <= rateA)         //A확률
            {
                int randomInList = UnityEngine.Random.Range(0, ua.AabilityContainer.Count);

                ua.onScreenContainer.Add(ua.AabilityContainer[randomInList]);
            }
            else                                    //나머지니까 B확률
            {
                ////히든이 걸릴 확률은 5%
                //int randomForBList = UnityEngine.Random.Range(1, 100);
                //Debug.Log($"{randomForBList}");
                //if (randomForBList < 6)
                //{
                //    ua.onScreenContainer.Add(ua.abilityHidden);
                //}
                //else
                //{
                //    int randomInList = UnityEngine.Random.Range(0, ua.BabilityContainer.Count);

                //    ua.onScreenContainer.Add(ua.BabilityContainer[randomInList]);
                //}

                int randomInList = UnityEngine.Random.Range(0, ua.BabilityContainer.Count);

                ua.onScreenContainer.Add(ua.BabilityContainer[randomInList]);
            }
        }

        //확인하려고 로그찍는중
        for (int i = 0; i < ua.onScreenContainer.Count; i++)
        {
            Debug.Log(ua.onScreenContainer[i].abilityName);
        }

        //중복이 확인되었을 때
        if(ua.onScreenContainer.Count != ua.onScreenContainer.Distinct().Count())
        {
            Debug.Log("능력 선택 중 중복발생");
            ua.onScreenContainer.Clear();       //리스트를 비워주고
            AddToOnScreenList();                //다시 실행하자
        }

        uc.Samount = 0;
        uc.Aamount = 0;
        uc.Bamount = 0;

        UISelect.SetActive(false);              //광물 넣는 창을 끄고
        UIChooseAbility.SetActive(true);        //능력을 고르는 창을 띄운다

        //ua.onScreenContainer.Clear();       //볼일 다봤으면 비워줘야겠지    //근데 이건 Select에서 해야지
    }

    //광물로 플레이어 체력 회복 시키는 코드
    private void OresToHeal()
    {
        float healAmount = healingText.CalculateHealAmount();

        pc.HP += pc.MaxHP * healAmount / 100;

        uc.Samount = 0;
        uc.Aamount = 0;
        uc.Bamount = 0;
    }

    //눌렀을 때 발동
    public void pushOres()
    {
        if(UIMain.activeSelf && !UISelect.activeSelf)
        {
            OresToHeal();
        }
        else if (!UIMain.activeSelf && UISelect.activeSelf)
        {
            if(uc.Samount + uc.Aamount + uc.Bamount > 0)
            {
                AddToOnScreenList();
            }
        }
    }
}
