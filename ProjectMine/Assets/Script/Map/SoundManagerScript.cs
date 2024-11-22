using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject boss;

    public AudioSource startBGM;
    public AudioSource dieBGM;
    public AudioSource clearBGM;

    public int nowBGM;
    public int beforeBGM;
    /// <summary>
    /// 0   기본
    /// 1   사망 배경음
    /// 2   클리어 배경음
    /// </summary>
    public static SoundManagerScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            nowBGM = 0;
            beforeBGM = 0;
            startBGM.PlayOneShot(startBGM.clip);
            dieBGM.Stop();
            clearBGM.Stop();
            //startBGM.PlayOneShot();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if (player != null && boss != null)
        {
            if (player.GetComponent<PlayerController>().HP <= 0)
            {
                bgmChange(1);
            }

            if (boss.GetComponent<EnemyDamage>().hp <= 0)
            {
                bgmChange(2);
            }
        }        
    }

    public void bgmChange(int i)
    {
        nowBGM = i;
        if (beforeBGM != nowBGM)
        {
            switch (nowBGM)
            {
                case 0:
                    startBGM.PlayOneShot(startBGM.clip);
                    dieBGM.Stop();
                    clearBGM.Stop();
                    beforeBGM = nowBGM;
                    break;
                case 1:
                    startBGM.Stop();
                    dieBGM.PlayOneShot(dieBGM.clip);
                    clearBGM.Stop();
                    beforeBGM = nowBGM;
                    break;
                case 2:
                    startBGM.Stop();
                    dieBGM.Stop();
                    clearBGM.PlayOneShot(clearBGM.clip);
                    beforeBGM = nowBGM;
                    break;
            }
        }
    }
}
