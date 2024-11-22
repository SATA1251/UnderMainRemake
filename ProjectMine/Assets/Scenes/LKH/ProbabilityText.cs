using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbabilityText : MonoBehaviour
{
    [SerializeField] private Text thisText;
    [SerializeField] private UIController uc;

    public int rateS = 10;    //S등급 기본확률
    public int rateA = 20;    //A등급 기본확률
    public int rateB = 70;    //B등급 기본확률

    [SerializeField] private bool acceptedS;
    [SerializeField] private bool acceptedA;
    [SerializeField] private bool acceptedB;

    private void Start()
    {
        thisText.text = "S등급  " + $"{10}" + "%\n" + "A등급  " + $"{20}" + "%\n" + "B등급  " + $"{70}" + "%";    //기본 확률
    }

    // Update is called once per frame
    private void Update()
    {
        CalculateProbability();
        thisText.text = "S등급  " + $"{rateS}" + "%\n" + "A등급  " + $"{rateA}" + "%\n" + "B등급  " + $"{rateB}" + "%";
    }

    private void CalculateProbability()
    {
        if(uc != null) 
        { 
            if(uc.Samount == 3 && !acceptedS)     //S광물을 3개 넣었을 때
            {
                rateB -= 5;
                rateA += 1;
                rateS += 4;

                acceptedS = true;
            }
            else if(uc.Samount != 3 && acceptedS)
            { 
                rateB += 5;
                rateA -= 1;
                rateS -= 4;

                acceptedS = false;
            }

            if(uc.Aamount == 6 && !acceptedA)     //A광물을 6개 넣었을 때
            {
                rateB -= 4;
                rateA += 2;
                rateS += 2;

                acceptedA = true;
            }
            else if( uc.Aamount != 6 && acceptedA)
            {
                rateB += 4;
                rateA -= 2;
                rateS -= 2;

                acceptedA = false;
            }

            if(uc.Bamount == 10 && !acceptedB)      //B광물을 10개 넣었을 때
            {
                rateB -= 4;
                rateA += 4;

                acceptedB = true;
            }
            else if(uc.Bamount != 10 && acceptedB)
            {
                rateB += 4;
                rateA -= 4;

                acceptedB = false;
            }
        }
    }
}
