using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBarScript : MonoBehaviour
{
    [SerializeField]        
    private Slider hpBar;
    [SerializeField]
    private Slider hpBar2;

    private GameObject bossHpObject1;
    private GameObject bossHpObject2;

    [SerializeField]
    GameObject Boss;
    public float BossHP;
    public float BossMaxHP;

    bool isNextHPStart = false;

    public void Start()
    {
        BossMaxHP = Boss.GetComponent<EnemyDamage>().hp;

        bossHpObject1 = GameObject.Find("HPSlider_Boss");
        bossHpObject2 = GameObject.Find("HPSlider_Boss2");

        bossHpObject1.SetActive(false);
        bossHpObject2.SetActive(false);
    }

    void Update()
    {
        BossHP = Boss.GetComponent<EnemyDamage>().hp;

        if(BossHP < BossMaxHP)
        {
            bossHpObject1.SetActive(true);
            bossHpObject2.SetActive(true);
        }

        if(isNextHPStart == false)
        {
            hpBar2.value = Mathf.Lerp(hpBar2.value, (BossHP - 250) / 250, Time.deltaTime * 10);

            if (BossHP < BossMaxHP / 2)
            {
                isNextHPStart = true;
            }
        }
        else
        {
            hpBar.value = Mathf.Lerp(hpBar.value, BossHP / 250, Time.deltaTime * 10);
        }

        //hpBar.value = Mathf.Lerp(hpBar.value, Boss.GetComponent<EnemyDamage>().HP / 500, Time.deltaTime * 10);
    }

}
