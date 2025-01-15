using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbabilityText : MonoBehaviour
{
    [SerializeField] private Text thisText;
    [SerializeField] private UIController uc;

    public int rateS = 3;    //S등급 기본확률
    public int rateA = 10;    //A등급 기본확률
    public int rateB = 87;    //B등급 기본확률

    [SerializeField] private bool acceptedS;
    [SerializeField] private bool acceptedA;
    [SerializeField] private bool acceptedB;

    private void Start()
    {
        thisText.text = "S등급  " + $"{3}" + "%\n" + "A등급  " + $"{10}" + "%\n" + "B등급  " + $"{87}" + "%";    //기본 확률
    }

    // Update is called once per frame
    private void Update()
    {
        CalculateProbability();
        thisText.text = "S등급  " + $"{rateS}" + "%\n" + "A등급  " + $"{rateA}" + "%\n" + "B등급  " + $"{rateB}" + "%";
    }

    private void CalculateProbability()
    {
        if (uc != null)
        {
            // S광물 추가에 따른 확률 계산
            if (uc.Samount > 0)
            {
                rateB = 50 - (uc.Samount * 5);
                rateA = 30 + (uc.Samount * 1);
                rateS = 20 + (uc.Samount * 4);
            }

            // A광물 추가에 따른 확률 계산
            if (uc.Aamount > 0)
            {
                rateB -= (uc.Aamount * 4);
                rateA += (uc.Aamount * 2);
                rateS += (uc.Aamount * 2);
            }

            // B광물 추가에 따른 확률 계산
            if (uc.Bamount > 0)
            {
                rateB += (uc.Bamount * 4);
                rateA -= (uc.Bamount * 2);
                rateS -= (uc.Bamount * 2);
            }
        }
    }

}
