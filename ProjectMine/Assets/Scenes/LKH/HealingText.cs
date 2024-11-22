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
        thisText.text = "체력 " + $"{0}" + " %회복";
    }

    private void Update()
    {
        thisText.text = "체력 " + $"{CalculateHealAmount()}" + " %회복";
    }

    //회복량 계산        //UIController에서 넣어준 광물 갯수로 회복량이 정해짐
    public int CalculateHealAmount()
    {
        int result = uc.Samount * 30 + uc.Aamount * 20 + uc.Bamount * 10;

        if (result > 100)
        {
            result = 100;       //100이 넘으면 100으로 처리     //초과해도 광물은 돌려주지 않는다
        }

        return result;
    }
}
