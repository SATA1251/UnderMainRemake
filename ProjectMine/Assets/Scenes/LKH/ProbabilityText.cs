using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbabilityText : MonoBehaviour
{
    [SerializeField] private Text thisText;
    [SerializeField] private UIController uc;

    public int rateS = 10;    //S��� �⺻Ȯ��
    public int rateA = 20;    //A��� �⺻Ȯ��
    public int rateB = 70;    //B��� �⺻Ȯ��

    [SerializeField] private bool acceptedS;
    [SerializeField] private bool acceptedA;
    [SerializeField] private bool acceptedB;

    private void Start()
    {
        thisText.text = "S���  " + $"{10}" + "%\n" + "A���  " + $"{20}" + "%\n" + "B���  " + $"{70}" + "%";    //�⺻ Ȯ��
    }

    // Update is called once per frame
    private void Update()
    {
        CalculateProbability();
        thisText.text = "S���  " + $"{rateS}" + "%\n" + "A���  " + $"{rateA}" + "%\n" + "B���  " + $"{rateB}" + "%";
    }

    private void CalculateProbability()
    {
        if(uc != null) 
        { 
            if(uc.Samount == 3 && !acceptedS)     //S������ 3�� �־��� ��
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

            if(uc.Aamount == 6 && !acceptedA)     //A������ 6�� �־��� ��
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

            if(uc.Bamount == 10 && !acceptedB)      //B������ 10�� �־��� ��
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
