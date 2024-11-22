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


    public float OxygenBuff;        // ������(�������� ���ϴ� ����� �ƴ϶� ������) �ϴ� �ʱ�ȭ�� ����Ƽ �ȿ�����
    public float BuffTimer;         // ���� ���ӽð�(�ϴ� ���ӽð� 10�ʷ� �ص�, ����Ƽ �ȿ��� �ʱ�ȭ)
    private bool BuffOn = false;    // ���� üũ

    [SerializeField] private bool isTouch;  // ����������� �浹üũ ����

    public float GetTimer;          // Ű �� ������ 2�� üũ�� Ÿ�̸�

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

        // ���� ó��
        // �������� ������ �ִµ� fŰ�� ������
        if (isTouch == true && Input.GetKeyUp(KeyCode.F))
        {
            GetTimer = 0.0f;
            audioSrc.Stop();
            OxygenUI.SetActive(false);
            MinBar = 0.0f;

            OxygenBar.fillAmount = 0;
        }

        // ����������
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

        // �������� �������
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
