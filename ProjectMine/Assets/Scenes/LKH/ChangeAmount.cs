using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GPT���� �ӾƼ� ���� �ſ�����
/// �ð��Ʊ���� ������ ��������� ��������
/// </summary>

public class ChangeAmount : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private UIController uc;

    private void Update()
    {
        ChangeText();
    }

    //�ؽ�Ʈ�� ���������� ������Ʈ
    private void ChangeText()
    {
        Transform grandParentTransform = transform.parent.parent;           //���θ� �޾ƿ�  //������޿� ���� �����̳�
        Transform parentTransform = transform.parent;
        Transform siblingTransform = parentTransform.Find("InnerText");     //���� �ڽİ����� Text ������Ʈ�� �޾ƿ�
        Text siblingText = siblingTransform.GetComponent<Text>();

        if (grandParentTransform.CompareTag("SContainer"))
        {
            siblingText.text = $"{uc.Samount}";                             //ȭ�鿡 ǥ���� �ؽ�Ʈ
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
            Debug.Log("�±� �񱳺Ұ� Ȯ���ʿ�");
        }
    }

    //�¿� ��ư���� ���� ���� ����
    public void changeAmountplus()
    {
        Transform grandParentTransform = transform.parent.parent;               //���θ� �޾ƿ�  //������޿� ���� �����̳�

        if (grandParentTransform.CompareTag("SContainer"))
        {
            if (pc.oresSamount > 0 && uc.Samount < 3)                           //S��� �ִ� ������ 3��
            {
                pc.oresSamount--;                                               //�÷��̾��� ������ ����
                uc.Samount++;                                                   //���⿡�� �߰��Ѵ�
            }
        }
        else if (grandParentTransform.CompareTag("AContainer"))
        {
            if (pc.oresAamount > 0 && uc.Aamount < 6)                           //A��� �ִ� ������ 6��
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
            Debug.Log("�±� �񱳺Ұ� Ȯ���ʿ�");
        }
    }

    //��� ��ü�� �÷�����ư�� ����
    public void changeAmountMinus()
    {
        Transform grandParentTransform = transform.parent.parent;            //���θ� �޾ƿ�  //������޿� ���� �����̳�

        if (grandParentTransform.CompareTag("SContainer"))
        {
            if (pc.oresSamount > 0 && uc.Samount > 0)                         //�ִ� ������ 10��
            {
                pc.oresSamount++;                                           //�÷��̾��� ������ ����
                uc.Samount--;                                                  //���⿡�� �߰��Ѵ�
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
            Debug.Log("�±� �񱳺Ұ� Ȯ���ʿ�");
        }
    }
}
