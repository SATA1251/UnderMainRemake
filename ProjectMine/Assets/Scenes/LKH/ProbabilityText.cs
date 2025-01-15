using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbabilityText : MonoBehaviour
{
    [SerializeField] private Text thisText;
    [SerializeField] private UIController uc;

    public int rateS = 3;    //S��� �⺻Ȯ��
    public int rateA = 10;    //A��� �⺻Ȯ��
    public int rateB = 87;    //B��� �⺻Ȯ��

    [SerializeField] private bool acceptedS;
    [SerializeField] private bool acceptedA;
    [SerializeField] private bool acceptedB;

    private void Start()
    {
        thisText.text = "S���  " + $"{3}" + "%\n" + "A���  " + $"{10}" + "%\n" + "B���  " + $"{87}" + "%";    //�⺻ Ȯ��
    }

    // Update is called once per frame
    private void Update()
    {
        CalculateProbability();
        thisText.text = "S���  " + $"{rateS}" + "%\n" + "A���  " + $"{rateA}" + "%\n" + "B���  " + $"{rateB}" + "%";
    }

    private void CalculateProbability()
    {
        if (uc != null)
        {
            // S���� �߰��� ���� Ȯ�� ���
            if (uc.Samount > 0)
            {
                rateB = 50 - (uc.Samount * 5);
                rateA = 30 + (uc.Samount * 1);
                rateS = 20 + (uc.Samount * 4);
            }

            // A���� �߰��� ���� Ȯ�� ���
            if (uc.Aamount > 0)
            {
                rateB -= (uc.Aamount * 4);
                rateA += (uc.Aamount * 2);
                rateS += (uc.Aamount * 2);
            }

            // B���� �߰��� ���� Ȯ�� ���
            if (uc.Bamount > 0)
            {
                rateB += (uc.Bamount * 4);
                rateA -= (uc.Bamount * 2);
                rateS -= (uc.Bamount * 2);
            }
        }
    }

}
