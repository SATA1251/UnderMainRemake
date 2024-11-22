using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIChooseAbility : MonoBehaviour
{
    [SerializeField] private UpgradeAbility ua;
    [SerializeField] private UIController uc;
    [SerializeField] private GameObject uca;
    Transform parentsTransform;
    Text firstHeader;
    Text firstDesc;
    Text secondHeader;
    Text secondDesc;
    Text thirdHeader;
    Text thirdDesc;

    public string selectedName;             //Ŭ���� ������Ʈ �̸��� ���� ����
    Ability selectedAbility;                //ȭ�� ������ ���õ� �ɷ�

    //public Animator machineAnimator;        //�ִϸ��̼� ����
    //private AudioSource audioSrc;           //�Ҹ� �����

    private void Start()
    {
        parentsTransform = transform.parent;

        Transform siblingFirstHeader = parentsTransform.Find("Text1stHeader");
        Transform siblingSecondHeader = parentsTransform.Find("Text2ndHeader");
        Transform siblingThirdHeader = parentsTransform.Find("Text3rdHeader");
        Transform siblingFirstDesc = parentsTransform.Find("Text1stDescription");
        Transform siblingSecondDesc = parentsTransform.Find("Text2ndDescription");
        Transform siblingThirdDesc = parentsTransform.Find("Text3rdDescription");

        firstHeader = siblingFirstHeader.GetComponent<Text>();
        firstDesc = siblingFirstDesc.GetComponent<Text>();
        secondHeader = siblingSecondHeader.GetComponent<Text>();
        secondDesc = siblingSecondDesc.GetComponent<Text>();
        thirdHeader = siblingThirdHeader.GetComponent<Text>();
        thirdDesc = siblingThirdDesc.GetComponent<Text>();

        //audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateText();
    }

    //����â������ �������� ������Ʈ ���ش�
    private void UpdateText()
    {
        if(ua.onScreenContainer.Count == 3)         //3���� �ƴٴ°� �ɷ��� �������� �й谡 �� �Ǿ��ٴ� �̾߱�
        {
            firstHeader.text = ua.onScreenContainer[0].abilityName;
            firstDesc.text = ua.onScreenContainer[0].abilityDescription;

            secondHeader.text = ua.onScreenContainer[1].abilityName;
            secondDesc.text = ua.onScreenContainer[1].abilityDescription;

            thirdHeader.text = ua.onScreenContainer[2].abilityName;
            thirdDesc.text = ua.onScreenContainer[2].abilityDescription;
        }       
    }  

    //Ŭ������ �� ������ �ɷ��� ��Ƶδ� �Լ�        //���ø��ϰ� �� �ɷ��� ������������ ���� ���� �����ϱ�
    public void UpdateSelectedAbility()
    {
        //���𰡰� ���õǾ�����
        if(selectedName != null)
        {
            for (int i = 0; i < ua.onScreenContainer.Count; i++)
            {
                if (ua.onScreenContainer[i].abilityName == selectedName)
                {
                    selectedAbility = ua.onScreenContainer[i];
                    Debug.Log("���õ� �ɷ� ID : " + selectedAbility.abilityID);
                    return;
                }
            }
        }
    }

    //"����" ��ư�� ������ 3���� �� �ɷ��� ������ �� ���� �÷��̾ ����
    public void ApplyUpgradeToPlayer()
    {
        if(selectedAbility.abilityName != null)     //������� ������
        {
            selectedAbility.upgradeFunction();      //�����Ƽ �ȿ� ����ִ� �Լ��� �����Ѵ�     //StatControlller�� �Ѱ���           
        }

        selectedAbility.Clear();                    //�� ��������� �������

        uc.upgradeCompleted = true;                 //���׷��̵� �Ϸ������� true�� ���ְ�
        uc.upgradeCompleted = true;                 //���׷��̵� �Ϸ������� true�� ���ְ�
        uca.SetActive(false);                       //�̰ź��� ����� �� �����ϱ�
        uc.paybackOres();                           //���׷��̵� �Ϸ� ���ο� ���� ������ ������ �����ִ°� ó���ϰ�
        uc.CloseUI();                               //UI�� ������
        uc.upgradeCompleted = false;                //�ٽ� ���׷��̵� �ڽ��� �� ���� ���� ���׷��̵尡 �� �Ȱ����� ����ؾ� �ϴϱ�

        //�ٸ� ����� ������ ������ ���� �����
        //������ �Ϸ�Ǿ����� ���̶���Ʈ �Ǿ��� �۾�ü�� �ǵ�����
        firstHeader.color = Color.black;
        firstHeader.fontSize = 50;
        secondHeader.color = Color.black;
        secondHeader.fontSize = 50;
        thirdHeader.color = Color.black;
        thirdHeader.fontSize = 50;
    }
}
