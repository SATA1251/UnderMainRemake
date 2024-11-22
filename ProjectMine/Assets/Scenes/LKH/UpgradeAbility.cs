using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

[System.Serializable]                       //����ü�� Inspector�� ǥ�õǰ� ���ش�
public struct Ability
{
    public string abilityName;              //�ɷ� �̸�
    public string abilityDescription;       //�ɷ� ���� �ؽ�Ʈ
    public string abilityRate;              //�ɷ� ��� (S/A/B)
    public System.Action upgradeFunction;   //�÷��̾�� ����� ������ �Լ�
    public string abilityID;                //�ɷ��� ����ID      //�ڵ�� �����Ϸ��� �������

    public void Clear()
    {
        this.abilityName = null;
        this.abilityDescription = null;
        this.abilityRate = null;
        this.abilityRate = null;
        this.upgradeFunction = null;
        this.abilityID = null;
    }
}

public class UpgradeAbility : MonoBehaviour
{
    public Ability noneAbility;
    //�۸��� '�ɷ� �ý���' ���� ����

    //S���
    public Ability abilityBerserk;          //������
    public Ability abilityOnepunch;         //������ �ѹ�

    //A���
    public Ability abilityDrain;            //�巹��
    public Ability abilityAlch;             //���ݼ�
    public Ability abilityFish;             //���
    public Ability abilityIronbody;         //�ݰ��ұ�
    public Ability abilityOxygenPipe;       //��������� ��ȭ

    //B���
    public Ability abilityHidden;           //����
    public Ability abilityMiningMaster;     //���� ������
    public Ability abilityDodgeMaster;      //ȸ�� ������
    public Ability abilityWindy;            //�ٶ�����
    public Ability abilityApnea;            //��ȣ��

    //��޺� �ɷµ��� ���� ����Ʈ             //���Ͱ� ��� List�� �����
    public List<Ability> SabilityContainer = new List<Ability>();          //S���
    public List<Ability> AabilityContainer = new List<Ability>();          //A���
    public List<Ability> BabilityContainer = new List<Ability>();          //B���
                                                                           
    public List<Ability> onScreenContainer = new List<Ability>();          //Ȯ���� ���� 3�� �߷��� ȭ�鿡 �ѷ��� ����Ʈ

    [SerializeField] private PlayerController playerController;     //���ȵ��� �ٸ� ��ũ��Ʈ�� �������־ ��ü 3�� �޾ƿ�  
    [SerializeField] private HandController handController;
    [SerializeField] private Hand hand;
    [SerializeField] private StatController statController;

    //�ɷ��� ������ִ� �Լ�      //�ɷ��� ���ϰ� ����� �; �������
    public Ability generateAbilities(string name, string description, string rate, Action func, string ID)
    {
        Ability ability = new Ability();

        ability.abilityName = name;
        ability.abilityDescription = description;
        ability.abilityRate = rate;
        ability.upgradeFunction = func;
        ability.abilityID = ID;

        return ability;
    }

    void Berserk()              //������
    {
        Debug.Log("������");

        statController.currentAbility = abilityBerserk;
        statController.UpgradeStatus(abilityBerserk);
    }

    void OnePunch()             //������ �� ��
    {
        Debug.Log("������ �� ��");

        statController.currentAbility = abilityOnepunch;
        statController.UpgradeStatus(abilityOnepunch);
    }

    void Drain()                //�巹��
    {
        Debug.Log("�巹��");

        statController.currentAbility = abilityDrain;
        statController.UpgradeStatus(abilityDrain);
    }

    void Alchemy()              //���ݼ�
    {
        Debug.Log("���ݼ�");

        statController.currentAbility = abilityAlch;
        statController.UpgradeStatus(abilityAlch);
    }

    void Fish()                 //���
    {
        Debug.Log("���");

        statController.currentAbility = abilityFish;
        statController.UpgradeStatus(abilityFish);
    }

    void IronBody()             //�ݰ��ұ�
    {
        Debug.Log("�ݰ��ұ�");

        statController.currentAbility = abilityIronbody;
        statController.UpgradeStatus(abilityIronbody);
    }

    void UpOxygenPipe()         //��������� ��ȭ      //�̰Ŵ� S�� �ɷ��ΰ� ���ٴ� ������ 
    {
        Debug.Log("��������� ��ȭ");

        statController.currentAbility = abilityOxygenPipe;
        statController.UpgradeStatus(abilityOxygenPipe);
    }

    void HiddenAbil()           //����
    {
        Debug.Log("����");

        //������ �� �ɷ��� ���� �ؾ���

        //������ �̵�
        statController.currentAbility = abilityHidden;
        statController.UpgradeStatus(abilityHidden);
    }

    void MiningMaster()         //���� ������
    {
        Debug.Log("���� ������");

        statController.currentAbility = abilityMiningMaster;
        statController.UpgradeStatus(abilityMiningMaster);
    }

    void DodgeMaster()          //ȸ�� ������
    {
        Debug.Log("ȸ�� ������");

        statController.currentAbility = abilityDodgeMaster;
        statController.UpgradeStatus(abilityDodgeMaster);
    }

    void Windy()                //�ٶ�����
    {
        Debug.Log("�ٶ�����");

        statController.currentAbility = abilityWindy;
        statController.UpgradeStatus(abilityWindy);
    }

    void Apnea()                //��ȣ��
    {
        Debug.Log("��ȣ��");

        statController.currentAbility = abilityApnea;
        statController.UpgradeStatus(abilityApnea);
    }

    private void Start()
    {
        //�̸� ������ �Լ��� �ɷµ��� �����
        //�̰� �³� �ʹ�
        abilityBerserk = generateAbilities("������", "�ִ� ü���� 25%�� ����������, ���ͷκ��� �޴� ���ذ� 70% �����մϴ�. ���ݷ��� 2��� �������ϴ�.", "S", Berserk, "ID0001");
        abilityOnepunch = generateAbilities("������ �� ��", "���ݼӵ��� 60% �����մϴ�. ���ݷ��� 5��� ��������, ġ��Ÿ �������� 2�谡 �˴ϴ�.", "S", OnePunch, "ID0002");

        abilityDrain = generateAbilities("�巹��", "��Ұ� 0���� �����ǰ�, ��� �ൿ�� ��Ҹ� �Ҹ����� �ʰ� �˴ϴ�(��ҷ��� ���� �������ش� �����˴ϴ�). ���� ���� �� ü���� 10% ȸ���ϸ�, ����ä�볪 ������ �� �� ü���� 5% ȸ���մϴ�.", "A", Drain, "ID0003");
        abilityAlch = generateAbilities("���ݼ�", "ü���� 60% ���Ϸ� ������ ��, ���� ���� ����� ������ �ϳ� �Ҹ��Ͽ� ü���� 20% ȸ���մϴ�. ������ ������ �ߵ����� �ʽ��ϴ�.", "A", Alchemy, "ID0004");
        abilityFish = generateAbilities("���", "��� �ൿ�� ��� �Ҹ��� 70% �����մϴ�. ��ҷ� �ִ�ġ�� 50% �����ϸ�, ��Ҹ� �ִ�ġ�� ȸ���մϴ�.", "A", Fish, "ID0005");
        abilityIronbody = generateAbilities("�ݰ��ұ�", "�Ϲ� ����, ���ӵ� ���ͷκ��� �޴� ���ذ� 40% �����ϸ�, ���� ���ͷκ��� �޴� ���ذ� 20% �����մϴ�. ü���� 10% ȸ���մϴ�.", "A", IronBody, "ID0006");
        abilityOxygenPipe = generateAbilities("��������� ��ȭ", "��� �ִ뷮�� 100 �����մϴ�. ��� �������� ��� �� ���� ���� �ð��� 5������ �þ��, ���� �ð� ���� ���ݷ��� 20, ġ��Ÿ ���ذ� 20% �����մϴ�.", "A", UpOxygenPipe, "ID0007");

        abilityHidden = generateAbilities("����", "�� �ɷ��� �ݵ�� �����ؾ��մϴ�. ���� �� �ٷ� ���������� �����̵� �˴ϴ�.", "B", HiddenAbil, "ID0008");
        abilityMiningMaster = generateAbilities("���� ������", "���� ä�� �� ��Ҹ� �Ҹ����� �ʽ��ϴ�. 20% Ȯ���� ������ 2��� ����ϴ�.", "B", MiningMaster, "ID0009");
        abilityDodgeMaster = generateAbilities("ȸ�� ������", "ȸ�� �� ��Ҹ� �Ҹ����� �ʽ��ϴ�. ȸ�� �ӵ��� 1.5�� �����մϴ�.", "B", DodgeMaster, "ID0010");
        abilityWindy = generateAbilities("�ٶ�����", "����, �̵��ӵ��� 30% �����մϴ�. ������ �޴� ���ذ� 20% �����մϴ�.", "B", Windy, "ID0011");
        abilityApnea = generateAbilities("��ȣ��", "�ִ� ��ҷ��� 60 �����ϰ� �ִ뷮��ŭ ȸ���մϴ�. ���ݷ°� ���ݼӵ��� 1.5�谡 �˴ϴ�.", "B", Apnea, "ID0012");

        //�������� ���� ������� �밡�ٷ� �߰��ع�����
        SabilityContainer.Add(abilityBerserk);
        SabilityContainer.Add(abilityOnepunch);

        AabilityContainer.Add(abilityDrain);
        AabilityContainer.Add(abilityAlch);
        AabilityContainer.Add(abilityFish);
        AabilityContainer.Add(abilityIronbody);
        AabilityContainer.Add(abilityOxygenPipe);

        //BabilityContainer.Add(abilityHidden);         //ȥ�� Ȯ�� �޶� ���� �����Ұ���
        BabilityContainer.Add(abilityMiningMaster);
        BabilityContainer.Add(abilityDodgeMaster);
        BabilityContainer.Add(abilityWindy);
        BabilityContainer.Add(abilityApnea);
    }

}
