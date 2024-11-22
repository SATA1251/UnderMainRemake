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

    //��ũ���� ��� 3�� �ɷ��� ���ϴ� �Լ�
    private void AddToOnScreenList()
    {
        int rateS = prob.rateS;             //������ ���� ������ Ȯ���� �����´�
        int rateA = prob.rateA;
        int rateB = prob.rateB;

        for (int i = 0; i < 3; i++)         //3���� �����ϴϱ�
        {
            int randomNumber = UnityEngine.Random.Range(1, 100);

            if (randomNumber <= rateS)              //SȮ��
            {
                int randomInList = UnityEngine.Random.Range(0, ua.SabilityContainer.Count);    //�ٽ� S����߿��� �������� ����

                ua.onScreenContainer.Add(ua.SabilityContainer[randomInList]);                     //ȭ�鿡 ��� ����Ʈ�� �ִ´�
            }
            else if (randomNumber <= rateA)         //AȮ��
            {
                int randomInList = UnityEngine.Random.Range(0, ua.AabilityContainer.Count);

                ua.onScreenContainer.Add(ua.AabilityContainer[randomInList]);
            }
            else                                    //�������ϱ� BȮ��
            {
                ////������ �ɸ� Ȯ���� 5%
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

        //Ȯ���Ϸ��� �α������
        for (int i = 0; i < ua.onScreenContainer.Count; i++)
        {
            Debug.Log(ua.onScreenContainer[i].abilityName);
        }

        //�ߺ��� Ȯ�εǾ��� ��
        if(ua.onScreenContainer.Count != ua.onScreenContainer.Distinct().Count())
        {
            Debug.Log("�ɷ� ���� �� �ߺ��߻�");
            ua.onScreenContainer.Clear();       //����Ʈ�� ����ְ�
            AddToOnScreenList();                //�ٽ� ��������
        }

        uc.Samount = 0;
        uc.Aamount = 0;
        uc.Bamount = 0;

        UISelect.SetActive(false);              //���� �ִ� â�� ����
        UIChooseAbility.SetActive(true);        //�ɷ��� ���� â�� ����

        //ua.onScreenContainer.Clear();       //���� �ٺ����� �����߰���    //�ٵ� �̰� Select���� �ؾ���
    }

    //������ �÷��̾� ü�� ȸ�� ��Ű�� �ڵ�
    private void OresToHeal()
    {
        float healAmount = healingText.CalculateHealAmount();

        pc.HP += pc.MaxHP * healAmount / 100;

        uc.Samount = 0;
        uc.Aamount = 0;
        uc.Bamount = 0;
    }

    //������ �� �ߵ�
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
