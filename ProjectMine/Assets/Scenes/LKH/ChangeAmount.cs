using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GPT한테 속아서 삽질 매우했음
/// 시간아까워서 적당히 만들었으니 이해해줘
/// </summary>

public class ChangeAmount : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private UIController uc;

    private void Update()
    {
        ChangeText();
    }

    //텍스트를 지속적으로 업데이트
    private void ChangeText()
    {
        Transform grandParentTransform = transform.parent.parent;           //조부모를 받아옴  //광물등급에 대한 컨테이너
        Transform parentTransform = transform.parent;
        Transform siblingTransform = parentTransform.Find("InnerText");     //같은 자식관계의 Text 오브젝트를 받아옴
        Text siblingText = siblingTransform.GetComponent<Text>();

        if (grandParentTransform.CompareTag("SContainer"))
        {
            siblingText.text = $"{uc.Samount}";                             //화면에 표시할 텍스트
        }
        else if (grandParentTransform.CompareTag("AContainer"))
        {
            siblingText.text = $"{uc.Aamount}";
        }
        else if (grandParentTransform.CompareTag("BContainer"))
        {
            siblingText.text = $"{uc.Bamount}";
        }
        else
        {
            Debug.Log("태그 비교불가 확인필요");
        }
    }

    //좌우 버튼으로 광물 개수 조절
    public void changeAmountplus()
    {
        Transform grandParentTransform = transform.parent.parent;               //조부모를 받아옴  //광물등급에 대한 컨테이너

        if (grandParentTransform.CompareTag("SContainer"))
        {
            if (pc.oresSamount > 0 && uc.Samount < 3)                           //S등급 최대 갯수는 3개
            {
                pc.oresSamount--;                                               //플레이어의 광물을 빼고
                uc.Samount++;                                                   //여기에는 추가한다
            }
        }
        else if (grandParentTransform.CompareTag("AContainer"))
        {
            if (pc.oresAamount > 0 && uc.Aamount < 6)                           //A등급 최대 갯수는 6개
            {
                pc.oresAamount--;
                uc.Aamount++;
            }
        }
        else if (grandParentTransform.CompareTag("BContainer"))
        {
            if (pc.oresBamount > 0 && uc.Bamount < 10)
            {
                pc.oresBamount--;
                uc.Bamount++;
            }
        }
        else
        {
            Debug.Log("태그 비교불가 확인필요");
        }
    }

    //기능 자체는 플러스버튼과 동일
    public void changeAmountMinus()
    {
        Transform grandParentTransform = transform.parent.parent;            //조부모를 받아옴  //광물등급에 대한 컨테이너

        if (grandParentTransform.CompareTag("SContainer"))
        {
            if (pc.oresSamount > 0 && uc.Samount > 0)                         //최대 갯수는 10개
            {
                pc.oresSamount++;                                           //플레이어의 광물을 빼고
                uc.Samount--;                                                  //여기에는 추가한다
            }
        }
        else if (grandParentTransform.CompareTag("AContainer"))
        {
            if (pc.oresAamount > 0 && uc.Aamount > 0)
            {
                pc.oresAamount++;
                uc.Aamount--;
            }
        }
        else if (grandParentTransform.CompareTag("BContainer"))
        {
            if (pc.oresBamount > 0 && uc.Bamount > 0)
            {
                pc.oresBamount++;
                uc.Bamount--;
            }
        }
        else
        {
            Debug.Log("태그 비교불가 확인필요");
        }
    }
}
