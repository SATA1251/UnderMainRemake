using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StatController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;     //���ȵ��� �ٸ� ��ũ��Ʈ�� �������־ ��ü 3�� �޾ƿ�  
    [SerializeField] private HandController handController;
    [SerializeField] private Hand hand;

    [SerializeField] private UpgradeAbility ua;
    public Ability currentAbility;                  //���� �����Ǿ� �ִ� �ɷ�
    private float drainTimer;                       //�ʴ� �������� ����

    //�÷��̾��� ������ �ʱ� ���·�
    [HideInInspector] public void ResetStatus()                      
    {
        //����� ����ؾ��ұ�? Start()�� ���� �ű⼭ �ʱ� ������ ����ϰ� �ϸ�..
        //�ð� ������ ���ĺ���
        playerController.MaxHP = 500;

        hand.damage = 50;
        hand.attackDelay = 1.5f;
        hand.attackDelayA = 1.2f;
        hand.attackDelayB = 0;
        handController.attackAnimSpeed = 1.0f;

        handController.MaxOxygen = 200;
        handController.AttackOxygen = 70;
        handController.DodgeOxygen = 70;

        playerController.walkSpeed = 5.0f;
        hand.criticalChance = 10.0f;
        hand.criticalDamage = 20;
        playerController.damageDecreaseAmount = 0;
        playerController.damageDecreaseAmountBoss = 0;

        playerController.dodgeSpeed = 25;
        playerController.dodgeAnimSpeed = 1.0f;
        playerController.dodgeTimeCheck = 0.3f;
    }

    [HideInInspector]
    public void ResetStatusToTitle()
    {
        playerController.MaxHP = 500;
        playerController.HP = playerController.MaxHP;

        hand.damage = 50;
        hand.attackDelay = 1.5f;
        hand.attackDelayA = 1.2f;
        hand.attackDelayB = 0;
        handController.attackAnimSpeed = 1.0f;

        handController.MaxOxygen = 200;
        handController.AttackOxygen = 70;
        handController.DodgeOxygen = 70;
        handController.Oxygen = handController.MaxOxygen;

        playerController.walkSpeed = 5.0f;
        hand.criticalChance = 10.0f;
        hand.criticalDamage = 20;
        playerController.damageDecreaseAmount = 0;
        playerController.damageDecreaseAmountBoss = 0;

        playerController.dodgeSpeed = 25;
        playerController.dodgeAnimSpeed = 1.0f;
        playerController.dodgeTimeCheck = 0.3f;
    }

    [HideInInspector] public void UpgradeStatus(Ability abil)
    {
        //�ɷ��� ���� ���� �� ������ �ʱ�ȭ��
        ResetStatus();

        //�� ����ID ������ ����ϴ��� �˰ڴ�
        switch (abil.abilityID)
        {
            case "ID0001":
                Debug.Log("StatController ������");

                playerController.MaxHP = playerController.MaxHP * 0.25f;
                playerController.damageDecreaseAmount = 70.0f;                    //�ɷ��� �ߺ��� �ȵȴٰ� �Ѵ�.. �̷��� �ص� ������ ������
                playerController.damageDecreaseAmountBoss = 40.0f;
                hand.damage *= 2.0f;
                playerController.HP = playerController.MaxHP;

                break;

            case "ID0002":
                Debug.Log("StatController ������ �� ��");

                hand.attackDelay *= 3f;
                hand.attackDelayA *= 2.8f;
                handController.attackAnimSpeed *= 0.4f;
                hand.damage *= 5.0f;
                hand.criticalDamage = hand.damage;

                break;

            case "ID0003":
                Debug.Log("StatController �巹��");

                handController.Oxygen = 0;
                handController.MaxOxygen = 0;
                handController.AttackOxygen = 0;
                handController.DodgeOxygen = 0;

                handController.isDrain = true;          //true�� �Ѱ��ָ� �����ҋ����� ü���� ȸ���Ѵ�
                ///�ʴ� �������� �Ʒ� HP ���� �Լ����� ó����

                break;

            case "ID0004":
                Debug.Log("StatController ���ݼ�");

                ///�̰� �Ʒ� HP �����ϴ� �Լ����� ó����

                break;

            case "ID0005":
                Debug.Log("StatController ���");

                handController.AttackOxygen -= handController.AttackOxygen * 0.7f;
                handController.DodgeOxygen -= handController.DodgeOxygen * 0.7f;
                handController.MaxOxygen += handController.MaxOxygen * 0.5f;
                handController.Oxygen = handController.MaxOxygen;       //���� ��ҷ��� �ִ�ġ��

                break;

            case "ID0006":
                Debug.Log("StatController �ݰ��ұ�");

                playerController.damageDecreaseAmount = 40.0f;              //������ ���� ȿ��
                playerController.damageDecreaseAmountBoss = 20.0f;
                playerController.HP += playerController.MaxHP * 0.1f;    //ü�� ȸ�� ȿ��

                break;

            case "ID0007":
                Debug.Log("StatController ��������� ��ȭ");

                ///OxygenPipe���� �����ؾ��Ѵ� �÷��̾ �ִ� ���� ���ӽð��� ���� �׳�
                handController.MaxOxygen += 100.0f;
                hand.damage += 20;
                //ġ��Ÿ ���ش� �⺻ ���ݷ��� 20�� �����ϴ� �Լ��� �̹� ������ ġ��Ÿ ��������ȿ���� ���� ������ �ʿ�� ������

                break;

            case "ID0008":
                Debug.Log("StatController ����");

                ///�̰� �Ⱦ��� ����

                break;

            case "ID0009":
                Debug.Log("StatController ���� ������");

                ///�����Լ����� ó���ϴ°� ������ �ű⼭ ����� Tag�� ���ؼ� ó������

                break;

            case "ID0010":
                Debug.Log("StatController ȸ�� ������");

                handController.DodgeOxygen = 0;
                playerController.dodgeSpeed *= 1.5f;            //������ 1.5��� �ϰ�
                playerController.dodgeAnimSpeed *= 1.5f;        //�ִϸ��̼� �ӵ��� 1.5��
                playerController.dodgeTimeCheck *= 2/3f;        //�׸��� ���ӽð��� ���δ�

                break;

            case "ID0011":
                Debug.Log("StatController �ٶ�����");

                hand.attackDelay *= 0.7f;
                hand.attackDelayA *= 0.7f;
                handController.attackAnimSpeed *= 1.3f;

                playerController.damageDecreaseAmount -= 20.0f;
                playerController.damageDecreaseAmountBoss -= 20.0f;

                break;

            case "ID0012":
                Debug.Log("StatController ��ȣ��");

                handController.MaxOxygen -= 60.0f;
                hand.damage *= 1.5f;

                hand.attackDelay *= 0.5f;
                hand.attackDelayA *= 0.5f;
                handController.attackAnimSpeed *= 1.5f;

                handController.Oxygen = handController.MaxOxygen;

                break;

            default:
                Debug.Log("�ɷ� ���� Ȯ�� �ʿ�");
                break;
        }

    }

    private void CheckMaxHP()
    {
        //���� �ɷ��� '���ݼ�'�̰� ü���� 60% �̸����� ��������
        if(currentAbility.abilityID == "ID0004" && playerController.HP < playerController.MaxHP * 0.6f)
        {
            //B��� ������ ���� üũ�ϰ�
            if(playerController.oresBamount > 0)
            {
                playerController.oresBamount--;                         //������ �Ҹ��ϰ�
                playerController.HP += playerController.MaxHP * 0.2f;   //ü���� 20% ȸ���Ѵ�
            }
            //B��� ������ ������ A��� ������ üũ�ϰ�
            else if(playerController.oresAamount > 0)
            {
                playerController.oresAamount--;
                playerController.HP += playerController.MaxHP * 0.2f;
            }
            //�Ѵ� ������ S��� ������ üũ�ϰ�
            else if(playerController.oresSamount > 0)
            {
                playerController.oresSamount--;
                playerController.HP += playerController.MaxHP * 0.2f;
            }
        }

        //���� �ɷ��� '�巹��'�̸�
        if(currentAbility.abilityID == "ID0003")
        {
            drainTimer += Time.deltaTime;

            if(drainTimer > 1f) 
            {
                playerController.HP -= 5f;
                drainTimer = 0f;
            }
        }

        //���� ü���� �ִ� ü���� �Ѿ�� �ִ� ü�¿� �����
        if (playerController.HP > playerController.MaxHP)
        {
            playerController.HP = playerController.MaxHP;
        }
    }

    private void Update()
    {
        CheckMaxHP();
    }
}
