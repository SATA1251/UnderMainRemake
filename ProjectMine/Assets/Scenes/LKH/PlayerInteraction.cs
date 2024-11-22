using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.F;        //상호작용 키

    //public Animator machineAnimator;

    [SerializeField] UIController uc;
    [SerializeField] private bool isInteractable;                       //상호작용 가능여부 체크

    //트리거가 발동되면(범위 안에 들어오면) 상호작용 가능여부를 true로
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInteractable = true;
            //machineAnimator.SetBool("isActivated", true);   ///애니메이션 실행은 업그레이드 완료 후 실행하는걸로 바꿔야함

        }
    }

    //범위를 벗어나면 다시 false로
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInteractable = false;
            //machineAnimator.SetBool("isActivated", false);
        }
    }
    void Update()
    {
        if (isInteractable && Input.GetKeyUp(interactionKey))       //상호작용한 거리에 있고 상호작용 키를 눌렀을 때
        { 
            if (uc != null) 
            {
                //여기가 실질적으로 상호작용할 내용
                if (uc != null && !uc.isOpened) 
                { 
                    uc.OpenUI();              
                } 
                //else if (uc != null && uc.isOpened)
                //{
                //    uc.CloseUI();
                //}
            }
        }
    }
}
