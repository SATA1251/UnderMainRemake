using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;
//using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public Animator Panim;
    public EnemyCtrl enemyCtrl;

    public EnemyDamage ED;

    // hand�� ������ ������
    public HandController hController;

    //UI ���� ���� ������
    [SerializeField] private UIController uiController;

    [SerializeField] private GameObject ESCmenu;
    [SerializeField] private GameObject dieMenu;
    [SerializeField] private GameObject clearMenu;



    public int testStatus;

    public float HP;                        //���� ü��       
    public float MaxHP;                     //�ִ� ü��
    public float walkSpeed;                 //�̵��ӵ�
    public float damageDecreaseAmount;      //������ ���� ����
    public float damageDecreaseAmountBoss;  //�������� �޴� ������ ���� ����

    // ���� ���� ��
    public int oresSamount;
    public int oresAamount;
    public int oresBamount;
    public int oresTotalAmount;

    [SerializeField]
    private float lookSensitivity;  // ���콺 ����

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;  //�÷��̾��� �������� ���� ������Ʈ


    public bool isUnBeat = false;  //�� ���� true �����϶� �������� ���´�. false�̸� ����Ű


    [SerializeField]
    private GameObject BuffUI;
    private bool isBuffOn = false;



    /// ����
    private AudioSource audioSrc;
    public AudioClip walkSound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public bool isMoving;



    /// ȸ�ǿ� �������
    public float dodgeSpeed;        // ȸ���Ҷ� ���� �����ϴ� �ӵ���     // �̰� �ٲٸ� ��
    public float applySpeed;        // ���� ����Ǵ� �ӵ�(�̵��Ҷ��� �ϴ� 100, ������ ������ 0.3�ʰ� 250)
    private bool isDodge = false;   // ȸ�� üũ
    public float dodgeAnimSpeed;    // ȸ�� �ִϸ��̼� �ӵ� ������       // �̰� �ٲٸ� ��
    public float dodgeTime;         // 0�ʺ��� �����ؼ� ���� �����Ѵ�
    public float dodgeTimeCheck;    // �̰� ȸ�� ���ӽð�(������ 0.3��)  // ���ӽð� �ٲܷ��� �̰Ÿ� �ٲٸ��


    // Rigidbody ������Ʈ �Ҵ�
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        ED = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyDamage>();

        // �̰� ������� �ȱ׷� 0���� �����ؼ� �ȿ����� ���ڸ�����
        applySpeed = walkSpeed;
        // ȸ�� �ִϸ��̼� �ӵ�(����)
        dodgeAnimSpeed = 1.0f;

        audioSrc = GetComponent<AudioSource>();
        ///���� ���� �ӽ÷� �־��� ���߿� �� ��������
        oresSamount = 0;
        oresAamount = 0;
        oresBamount = 0;
    }
    private void PlaySound(string action)
    {
        switch (action)
        {
            case "WALKSOUND":
                audioSrc.clip = walkSound;
                break;
            case "HITSOUND":
                audioSrc.clip = hitSound;
                break;
            case "DIESOUND":
                audioSrc.clip = dieSound;
                break;
        }
        audioSrc.Play();
    }

    void Update()
    {
        //Dodge();                 // ȸ��

        MouseControl();
        CalculateOres();

        if (!uiController.isOpened && !ESCmenu.activeSelf && !dieMenu.activeSelf && !clearMenu.activeSelf)
        {
            Move();

            CameraRotation();

            CharacterRotation();

            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                Panim.SetBool("Walk", false);
            }
            else
            {
                Panim.SetBool("Walk", true);

            }

            //BeAttacked();
            if (hController.Oxygen >= 60.0f)
            {
                TryDodge();
                // ���� �ִϸ��̼� �ӵ� ���� �Լ�
                Panim.SetFloat("DodgeSpeed", dodgeAnimSpeed);
            }

            if (isDodge == true)
            {
                dodgeTime += Time.deltaTime;

                if (dodgeTime > dodgeTimeCheck)
                {
                    isUnBeat = false;
                    isDodge = false;
                    applySpeed = walkSpeed;
                    dodgeTime = 0;
                }
            }

            if (Input.GetMouseButtonDown(0)) // 0 is for left mouse button
            {
                Vector3 mousePosition = Input.mousePosition;
                Debug.Log("Screen coordinates where mouse was clicked: " + mousePosition);
            }
        }
        BuffOn();

    }

    //�� ���� ���� ���
    private void CalculateOres()
    {
        oresTotalAmount = oresAamount + oresBamount + oresSamount;
    }

    /// �ǰ� �������� HP�� �پ��� �Լ�
    public void BeAttacked()
    {

        // ����HP���� ������ ���ݷ¸�ŭ HP�� �پ���.
        HP -= ED.enemyDamage * ((100 - damageDecreaseAmount) / 100);
        Debug.Log(HP);
        PlaySound("HITSOUND");
        isUnBeat = false;

        if (HP <= 0)
        {
            // ���ȭ������ �Ѿ��
            PlaySound("DIESOUND");

        }
    }

    public void BeAttackedBossNormal()
    {



        // ����HP���� ������ ���ݷ¸�ŭ HP�� �پ���.
        HP -= ED.bossNormalDamage * ((100 -damageDecreaseAmountBoss) / 100);
        PlaySound("HITSOUND");


        if (HP <= 0)
        {
            PlaySound("DIESOUND");
            // ���ȭ������ �Ѿ��
        }
    }

    public void BeAttackedBossStone()
    {

        // ����HP���� ������ ���ݷ¸�ŭ HP�� �پ���.
        HP -= ED.bossThrowDamage * ((100 - damageDecreaseAmountBoss) / 100);
        PlaySound("HITSOUND");


        if (HP <= 0)
        {
            PlaySound("DIESOUND");
            // ���ȭ������ �Ѿ��
        }
    }
    private void Move()
    {

        // �¿�
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        // �յ�
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.velocity = _velocity;

        //myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);        ����

        if (_moveDirX != 0 || _moveDirZ != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            if (!audioSrc.isPlaying)

                PlaySound("WALKSOUND");
        }
        else
        {
            //audioSrc.Stop();
        }
        // PlaySound("WALKSOUND");

        //audioSrc.Play();
    }

    private void TryDodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dodge();
        }
    }

    private void BuffOn()
    {
        if (hController.oxygenIncreasePerSecond != 40.0f)
        {
            BuffUI.SetActive(true);
        }
        else
        {
            BuffUI.SetActive(false);
        }
    }


    private void Dodge()
    {
        isDodge = true;

        hController.Oxygen -= hController.DodgeOxygen;

        applySpeed = dodgeSpeed;
        Panim.SetTrigger("Dodge");


    }


    // ���� ī�޶� ȸ��
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;

        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // �¿� ĳ���� ȸ��
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    //���콺 Ŀ�� ����/ǥ�� ����
    private void MouseControl()
    {
        if (!uiController.isOpened && !ESCmenu.activeSelf && !dieMenu.activeSelf && !clearMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
