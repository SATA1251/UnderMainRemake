using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject uiScreen;                         //UI오브젝트
    public bool isOpened = false;                       //열려 있는지 체크

    [SerializeField] private GameObject UIMain;
    [SerializeField] private GameObject UISelect;
    [SerializeField] private GameObject UIChooseAbility;
    [SerializeField] private PlayerController playerController;

    //미안하다 시간이 없어서 하드코딩 갈긴다
    [SerializeField] private Text txt1;
    [SerializeField] private Text txt2;
    [SerializeField] private Text txt3;

    public bool upgradeCompleted = false; //업그레이드를 완료했는지 여부   //광물넣고 업그레이드 안했을 때 이걸로 판별해서 페이백

    public int Samount = 0;               //업그레이드 기계에서 보일 S광물수량
    public int Aamount = 0;               //A수량
    public int Bamount = 0;               //B수량

    public void OpenUI()
    {
        if (!isOpened && uiScreen != null) 
        {
            uiScreen.SetActive(true);

            //처음에는 무조건 회복창이 뜨도록
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
        if(!UIChooseAbility.activeSelf)         //업그레이드 선택창에서는 꺼지지 않게
        {
            paybackOres();

            if (isOpened && uiScreen != null)
            {
                uiScreen.SetActive(false);

                isOpened = false;
            }
        }      
    }

    //화면 전환     //체력회복 -> 능력고르기     //능력고르기 -> 체력회복
    public void ChangeScreen()
    {
        if(UIMain != null && UISelect != null) 
        { 
            //체력회복 UI가 active 상태이면
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

    //업그레이드나 회복이 완료되지 않은 상태에서 창닫기 버튼을 누르면 넣었던 광물들 돌려줌
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
