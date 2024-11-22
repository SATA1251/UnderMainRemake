using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingText : MonoBehaviour
{
    [SerializeField] private Text thisText;
    [SerializeField] private UIController uc;

    private void Start()
    {
        thisText.text = "ü�� " + $"{0}" + " %ȸ��";
    }

    private void Update()
    {
        thisText.text = "ü�� " + $"{CalculateHealAmount()}" + " %ȸ��";
    }

    //ȸ���� ���        //UIController���� �־��� ���� ������ ȸ������ ������
    public int CalculateHealAmount()
    {
        int result = uc.Samount * 30 + uc.Aamount * 20 + uc.Bamount * 10;

        if (result > 100)
        {
            result = 100;       //100�� ������ 100���� ó��     //�ʰ��ص� ������ �������� �ʴ´�
        }

        return result;
    }
}
