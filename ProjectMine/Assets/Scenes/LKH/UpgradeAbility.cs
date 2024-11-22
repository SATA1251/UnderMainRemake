using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

[System.Serializable]                       //구조체도 Inspector에 표시되게 해준다
public struct Ability
{
    public string abilityName;              //능력 이름
    public string abilityDescription;       //능력 설명 텍스트
    public string abilityRate;              //능력 등급 (S/A/B)
    public System.Action upgradeFunction;   //플레이어에게 적용될 실질적 함수
    public string abilityID;                //능력의 고유ID      //코드상 구분하려고 만들었음

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
    //작명은 '능력 시스템' 문서 기준

    //S등급
    public Ability abilityBerserk;          //광전사
    public Ability abilityOnepunch;         //묵직한 한방

    //A등급
    public Ability abilityDrain;            //드레인
    public Ability abilityAlch;             //연금술
    public Ability abilityFish;             //어류
    public Ability abilityIronbody;         //금강불괴
    public Ability abilityOxygenPipe;       //산소파이프 강화

    //B등급
    public Ability abilityHidden;           //히든
    public Ability abilityMiningMaster;     //광물 마스터
    public Ability abilityDodgeMaster;      //회피 마스터
    public Ability abilityWindy;            //바람돌이
    public Ability abilityApnea;            //무호흡

    //등급별 능력들을 담을 리스트             //벡터가 없어서 List로 사용함
    public List<Ability> SabilityContainer = new List<Ability>();          //S등급
    public List<Ability> AabilityContainer = new List<Ability>();          //A등급
    public List<Ability> BabilityContainer = new List<Ability>();          //B등급
                                                                           
    public List<Ability> onScreenContainer = new List<Ability>();          //확률에 따라 3개 추려서 화면에 뿌려줄 리스트

    [SerializeField] private PlayerController playerController;     //스탯들이 다른 스크립트에 나눠져있어서 객체 3개 받아옴  
    [SerializeField] private HandController handController;
    [SerializeField] private Hand hand;
    [SerializeField] private StatController statController;

    //능력을 만들어주는 함수      //능력을 편하게 만들고 싶어서 만들었다
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

    void Berserk()              //광전사
    {
        Debug.Log("광전사");

        statController.currentAbility = abilityBerserk;
        statController.UpgradeStatus(abilityBerserk);
    }

    void OnePunch()             //묵직한 한 방
    {
        Debug.Log("묵직한 한 방");

        statController.currentAbility = abilityOnepunch;
        statController.UpgradeStatus(abilityOnepunch);
    }

    void Drain()                //드레인
    {
        Debug.Log("드레인");

        statController.currentAbility = abilityDrain;
        statController.UpgradeStatus(abilityDrain);
    }

    void Alchemy()              //연금술
    {
        Debug.Log("연금술");

        statController.currentAbility = abilityAlch;
        statController.UpgradeStatus(abilityAlch);
    }

    void Fish()                 //어류
    {
        Debug.Log("어류");

        statController.currentAbility = abilityFish;
        statController.UpgradeStatus(abilityFish);
    }

    void IronBody()             //금강불괴
    {
        Debug.Log("금강불괴");

        statController.currentAbility = abilityIronbody;
        statController.UpgradeStatus(abilityIronbody);
    }

    void UpOxygenPipe()         //산소파이프 강화      //이거는 S급 능력인거 같다는 생각이 
    {
        Debug.Log("산소파이프 강화");

        statController.currentAbility = abilityOxygenPipe;
        statController.UpgradeStatus(abilityOxygenPipe);
    }

    void HiddenAbil()           //히든
    {
        Debug.Log("히든");

        //무조건 이 능력을 고르게 해야함

        //보스방 이동
        statController.currentAbility = abilityHidden;
        statController.UpgradeStatus(abilityHidden);
    }

    void MiningMaster()         //광물 마스터
    {
        Debug.Log("광물 마스터");

        statController.currentAbility = abilityMiningMaster;
        statController.UpgradeStatus(abilityMiningMaster);
    }

    void DodgeMaster()          //회피 마스터
    {
        Debug.Log("회피 마스터");

        statController.currentAbility = abilityDodgeMaster;
        statController.UpgradeStatus(abilityDodgeMaster);
    }

    void Windy()                //바람돌이
    {
        Debug.Log("바람돌이");

        statController.currentAbility = abilityWindy;
        statController.UpgradeStatus(abilityWindy);
    }

    void Apnea()                //무호흡
    {
        Debug.Log("무호흡");

        statController.currentAbility = abilityApnea;
        statController.UpgradeStatus(abilityApnea);
    }

    private void Start()
    {
        //미리 만들어둔 함수로 능력들을 만든다
        //이게 맞나 싶다
        abilityBerserk = generateAbilities("광전사", "최대 체력이 25%로 설정되지만, 몬스터로부터 받는 피해가 70% 감소합니다. 공격력이 2배로 강해집니다.", "S", Berserk, "ID0001");
        abilityOnepunch = generateAbilities("묵직한 한 방", "공격속도가 60% 감소합니다. 공격력이 5배로 강해지고, 치명타 데미지가 2배가 됩니다.", "S", OnePunch, "ID0002");

        abilityDrain = generateAbilities("드레인", "산소가 0으로 유지되고, 모든 행동은 산소를 소모하지 않게 됩니다(산소량에 따른 지속피해는 유지됩니다). 몬스터 공격 시 체력을 10% 회복하며, 광물채취나 땅굴을 팔 때 체력을 5% 회복합니다.", "A", Drain, "ID0003");
        abilityAlch = generateAbilities("연금술", "체력이 60% 이하로 내려갈 때, 가장 낮은 등급의 광물을 하나 소모하여 체력을 20% 회복합니다. 광물이 없으면 발동하지 않습니다.", "A", Alchemy, "ID0004");
        abilityFish = generateAbilities("어류", "모든 행동의 산소 소모량이 70% 감소합니다. 산소량 최대치가 50% 증가하며, 산소를 최대치로 회복합니다.", "A", Fish, "ID0005");
        abilityIronbody = generateAbilities("금강불괴", "일반 몬스터, 네임드 몬스터로부터 받는 피해가 40% 감소하며, 보스 몬스터로부터 받는 피해가 20% 감소합니다. 체력을 10% 회복합니다.", "A", IronBody, "ID0006");
        abilityOxygenPipe = generateAbilities("산소파이프 강화", "산소 최대량이 100 증가합니다. 산소 파이프를 사용 시 버프 지속 시간이 5분으로 늘어나며, 지속 시간 동안 공격력이 20, 치명타 피해가 20% 증가합니다.", "A", UpOxygenPipe, "ID0007");

        abilityHidden = generateAbilities("히든", "이 능력을 반드시 선택해야합니다. 선택 시 바로 보스방으로 순간이동 됩니다.", "B", HiddenAbil, "ID0008");
        abilityMiningMaster = generateAbilities("광물 마스터", "광물 채취 시 산소를 소모하지 않습니다. 20% 확률로 광물을 2배로 얻습니다.", "B", MiningMaster, "ID0009");
        abilityDodgeMaster = generateAbilities("회피 마스터", "회피 시 산소를 소모하지 않습니다. 회피 속도가 1.5배 증가합니다.", "B", DodgeMaster, "ID0010");
        abilityWindy = generateAbilities("바람돌이", "공격, 이동속도가 30% 증가합니다. 적에게 받는 피해가 20% 증가합니다.", "B", Windy, "ID0011");
        abilityApnea = generateAbilities("무호흡", "최대 산소량이 60 감소하고 최대량만큼 회복합니다. 공격력과 공격속도가 1.5배가 됩니다.", "B", Apnea, "ID0012");

        //섹시하지 않은 방법으로 노가다로 추가해버리기
        SabilityContainer.Add(abilityBerserk);
        SabilityContainer.Add(abilityOnepunch);

        AabilityContainer.Add(abilityDrain);
        AabilityContainer.Add(abilityAlch);
        AabilityContainer.Add(abilityFish);
        AabilityContainer.Add(abilityIronbody);
        AabilityContainer.Add(abilityOxygenPipe);

        //BabilityContainer.Add(abilityHidden);         //혼자 확률 달라서 따로 관리할거임
        BabilityContainer.Add(abilityMiningMaster);
        BabilityContainer.Add(abilityDodgeMaster);
        BabilityContainer.Add(abilityWindy);
        BabilityContainer.Add(abilityApnea);
    }

}
