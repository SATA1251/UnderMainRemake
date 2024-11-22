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

    public string selectedName;             //클릭한 오브젝트 이름을 담을 변수
    Ability selectedAbility;                //화면 내에서 선택된 능력

    //public Animator machineAnimator;        //애니메이션 실행
    //private AudioSource audioSrc;           //소리 실행용

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

    //선택창에서의 선택지를 업데이트 해준다
    private void UpdateText()
    {
        if(ua.onScreenContainer.Count == 3)         //3개가 됐다는건 능력이 랜덤으로 분배가 잘 되었다는 이야기
        {
            firstHeader.text = ua.onScreenContainer[0].abilityName;
            firstDesc.text = ua.onScreenContainer[0].abilityDescription;

            secondHeader.text = ua.onScreenContainer[1].abilityName;
            secondDesc.text = ua.onScreenContainer[1].abilityDescription;

            thirdHeader.text = ua.onScreenContainer[2].abilityName;
            thirdDesc.text = ua.onScreenContainer[2].abilityDescription;
        }       
    }  

    //클릭했을 때 선택한 능력을 담아두는 함수        //선택만하고 그 능력을 최종결정하지 않을 수도 있으니까
    public void UpdateSelectedAbility()
    {
        //무언가가 선택되었을때
        if(selectedName != null)
        {
            for (int i = 0; i < ua.onScreenContainer.Count; i++)
            {
                if (ua.onScreenContainer[i].abilityName == selectedName)
                {
                    selectedAbility = ua.onScreenContainer[i];
                    Debug.Log("선택된 능력 ID : " + selectedAbility.abilityID);
                    return;
                }
            }
        }
    }

    //"선택" 버튼을 누르면 3가지 중 능력을 선택한 후 이제 플레이어에 적용
    public void ApplyUpgradeToPlayer()
    {
        if(selectedAbility.abilityName != null)     //비어있지 않을때
        {
            selectedAbility.upgradeFunction();      //어빌리티 안에 담겨있는 함수를 실행한다     //StatControlller에 넘겨줌           
        }

        selectedAbility.Clear();                    //다 사용했으니 비워주자

        uc.upgradeCompleted = true;                 //업그레이드 완료했으니 true로 해주고
        uc.upgradeCompleted = true;                 //업그레이드 완료했으니 true로 해주고
        uca.SetActive(false);                       //이거부터 꺼줘야 다 꺼지니까
        uc.paybackOres();                           //업그레이드 완료 여부에 따라 투입한 광물을 돌려주는걸 처리하고
        uc.CloseUI();                               //UI를 꺼버려
        uc.upgradeCompleted = false;                //다시 업그레이드 박스를 열 때는 아직 업그레이드가 안 된것으로 취급해야 하니까

        //다른 방법이 있을것 같은데 이젠 힘들어
        //선택이 완료되었을때 하이라이트 되었던 글씨체를 되돌려줌
        firstHeader.color = Color.black;
        firstHeader.fontSize = 50;
        secondHeader.color = Color.black;
        secondHeader.fontSize = 50;
        thirdHeader.color = Color.black;
        thirdHeader.fontSize = 50;
    }
}
