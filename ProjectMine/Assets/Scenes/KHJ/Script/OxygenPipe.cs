using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenPipe : MonoBehaviour
{
    [SerializeField]
    private HandController hController;

    [SerializeField]
    private PlayerController pController;

    [SerializeField]
    private GameObject OxygenUI;

    [SerializeField]
    private Image OxygenBar;
    private float MinBar = 0.0f;
    private float MaxBar = 2.0f;


    public float OxygenBuff;        // 버프값(기존값에 더하는 방식이 아니라 덮어씌운다) 일단 초기화는 유니티 안에서함
    public float BuffTimer;         // 버프 지속시간(일단 지속시간 10초로 해둠, 유니티 안에서 초기화)
    private bool BuffOn = false;    // 버프 체크

    [SerializeField] private bool isTouch;  // 산소파이프와 충돌체크 변수

    public float GetTimer;          // 키 꾹 누르는 2초 체크용 타이머

    private AudioSource audioSrc;
    public AudioClip oxygenPipe;


    private bool isUIActive = false;


    void Start()
    {
        BuffTimer = 60.0f;
        GetTimer = 0.0f;

        OxygenBar.fillAmount = 0.0f;
        isTouch = false;

        hController = GameObject.Find("Holder").GetComponent<HandController>();
        pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        audioSrc = GetComponent<AudioSource>();
        OxygenUI.SetActive(false);

    }
    private void PlaySound(string action)
    {
        switch (action)
        {
            case "OXYGENPIPE":
                audioSrc.clip = oxygenPipe;
                break;
  
        }
        audioSrc.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = true;
            OxygenUI.SetActive(true);

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = false;
            //audioSrc.Stop();
            if (isTouch == false)
            {
                OxygenUI.SetActive(false);
                OxygenBar.fillAmount = 0.0f;
                MinBar = 0.0f;

            }
        }
    }
    void Update()
    {
        if (isTouch == true && Input.GetKey(KeyCode.F))
        {
            GetTimer += Time.deltaTime;

            MinBar += Time.deltaTime;
            OxygenBar.fillAmount = 0.0f + (Mathf.Lerp(0, 100, MinBar/MaxBar)/100);

            

            if (Input.GetKeyDown(KeyCode.F))
            {
                PlaySound("OXYGENPIPE");
            }
            if (GetTimer > 2.0f)
            {
                BuffOn = true;
                hController.Oxygen = hController.MaxOxygen;
                GetTimer = 0.0f;
            }
            if (GetTimer < 2.0f && pController.isUnBeat == true)
            {
                isTouch = false;
                audioSrc.Stop();
                GetTimer = 0.0f;
                MinBar = 0.0f; 
                OxygenBar.fillAmount = 0;

            }
        }

        // 예외 처리
        // 파이프와 접축해 있는데 f키를 땠을때
        if (isTouch == true && Input.GetKeyUp(KeyCode.F))
        {
            GetTimer = 0.0f;
            audioSrc.Stop();
            OxygenUI.SetActive(false);
            MinBar = 0.0f;

            OxygenBar.fillAmount = 0;
        }

        // 버프받을때
        if (BuffOn == true)
        {
            OxygenUI.SetActive(false);
            MinBar = 0.0f;

            OxygenBar.fillAmount = 0;

            BuffTimer -= Time.deltaTime;
            GetTimer = 0.0f;
            hController.oxygenIncreasePerSecond = OxygenBuff / 1.0f;
        }
        if (BuffTimer <= 0.0f)
        {
            BuffOn = false;
            hController.oxygenIncreasePerSecond = hController.oxygenIncreaseRate / 1.0f;
            BuffTimer = 10.0f;
        }

        // 파이프를 벗어났을때
        if (isTouch == false)
        {
            GetTimer = 0.0f;
            MinBar = 0.0f;

            OxygenBar.fillAmount = 0;
           // OxygenUI.SetActive(false);

            //audioSrc.Stop();
        }

        if (OxygenBar.fillAmount == 1)
        {
            MinBar = 0.0f;

            OxygenBar.fillAmount = 0;

        }

        if(GetTimer == 0.0f)
        {
            MinBar = 0.0f;

            OxygenBar.fillAmount = 0;

        }
    }


}
