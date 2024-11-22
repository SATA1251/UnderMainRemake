using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.F;        //��ȣ�ۿ� Ű

    //public Animator machineAnimator;

    [SerializeField] UIController uc;
    [SerializeField] private bool isInteractable;                       //��ȣ�ۿ� ���ɿ��� üũ

    //Ʈ���Ű� �ߵ��Ǹ�(���� �ȿ� ������) ��ȣ�ۿ� ���ɿ��θ� true��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInteractable = true;
            //machineAnimator.SetBool("isActivated", true);   ///�ִϸ��̼� ������ ���׷��̵� �Ϸ� �� �����ϴ°ɷ� �ٲ����

        }
    }

    //������ ����� �ٽ� false��
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
        if (isInteractable && Input.GetKeyUp(interactionKey))       //��ȣ�ۿ��� �Ÿ��� �ְ� ��ȣ�ۿ� Ű�� ������ ��
        { 
            if (uc != null) 
            {
                //���Ⱑ ���������� ��ȣ�ۿ��� ����
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
