using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcAbility : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;     //���ȵ��� �ٸ� ��ũ��Ʈ�� �������־ ��ü 3�� �޾ƿ�  
    [SerializeField] private HandController handController;
    [SerializeField] private Hand hand;

    public Ability currentAbility;                //���� �����Ǿ� �ִ� �ɷ�

    //�ɷ��� ������ �ִ� üũ
    [SerializeField] private bool checkBerserk;                     //������
    [SerializeField] private bool checkOnePunch;                    //������ �� ��
    [SerializeField] private bool checkDrain;                       //�巹��
    [SerializeField] private bool checkAlch;                        //���ݼ�
    [SerializeField] private bool checkFish;                        //���
    [SerializeField] private bool checkIron;                        //�ݰ��ұ�
    [SerializeField] private bool checkOxyPipe;                     //��������� ��ȭ
    [SerializeField] private bool checkHidden;                      //����
    [SerializeField] private bool checkMining;                      //����������
    [SerializeField] private bool checkDodge;                       //ȸ�Ǹ�����
    [SerializeField] private bool checkWindy;                       //�ٶ�����
    [SerializeField] private bool checkApnea;                       //��ȣ��


}
