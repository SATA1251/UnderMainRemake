using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject uiScreen;                         //UI������Ʈ
    public bool isOpened = false;                       //���� �ִ��� üũ

    [SerializeField] private GameObject UIMain;
    [SerializeField] private GameObject UISelect;
    [SerializeField] private GameObject UIChooseAbility;
    [SerializeField] private PlayerController playerController;

    //�̾��ϴ� �ð��� ��� �ϵ��ڵ� �����
    [SerializeField] private Text txt1;
    [SerializeField] private Text txt2;
    [SerializeField] private Text txt3;

    public bool upgradeCompleted = false; //���׷��̵带 �Ϸ��ߴ��� ����   //�����ְ� ���׷��̵� ������ �� �̰ɷ� �Ǻ��ؼ� ���̹�

    public int Samount = 0;               //���׷��̵� ��迡�� ���� S��������
    public int Aamount = 0;               //A����
    public int Bamount = 0;               //B����

    public void OpenUI()
    {
        if (!isOpened && uiScreen != null) 
        {
            uiScreen.SetActive(true);

            //ó������ ������ ȸ��â�� �ߵ���
            if(!UIMain.activeSelf)
            {
                UISelect.SetActive(false);
                UIChooseAbility.SetActive(false);
                UIMain.SetActive(true);
            }

            isOpened = true;
        }
    }

    public void CloseUI()
    {
        if(!UIChooseAbility.activeSelf)         //���׷��̵� ����â������ ������ �ʰ�
        {
            paybackOres();

            if (isOpened && uiScreen != null)
            {
                uiScreen.SetActive(false);

                isOpened = false;
            }
        }      
    }

    //ȭ�� ��ȯ     //ü��ȸ�� -> �ɷ°���     //�ɷ°��� -> ü��ȸ��
    public void ChangeScreen()
    {
        if(UIMain != null && UISelect != null) 
        { 
            //ü��ȸ�� UI�� active �����̸�
            if (UIMain.activeSelf)
            {
                UIMain.SetActive(false);
                UISelect.SetActive(true);
            }
            else if (UISelect.activeSelf)
            {
                UISelect.SetActive(false);
                UIMain.SetActive(true);
            }
        }
    }

    //���׷��̵峪 ȸ���� �Ϸ���� ���� ���¿��� â�ݱ� ��ư�� ������ �־��� ������ ������
    public void paybackOres()
    {
        if (!upgradeCompleted)
        {
            playerController.oresSamount += Samount;
            playerController.oresAamount += Aamount;
            playerController.oresBamount += Bamount;

            Samount = 0;
            Aamount = 0;
            Bamount = 0;

            txt1.text = "0";
            txt2.text = "0";
            txt3.text = "0";
        }
    }
}
