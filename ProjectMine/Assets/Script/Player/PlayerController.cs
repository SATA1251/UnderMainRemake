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

    // hand의 변수를 가져옴
    public HandController hController;

    //UI 오픈 여부 가져옴
    [SerializeField] private UIController uiController;

    [SerializeField] private GameObject ESCmenu;
    [SerializeField] private GameObject dieMenu;
    [SerializeField] private GameObject clearMenu;
    [SerializeField] private GameObject abilityMenu;



    public int testStatus;

    public float HP;                        //현재 체력       
    public float MaxHP;                     //최대 체력
    public float walkSpeed;                 //이동속도
    public float damageDecreaseAmount;      //데미지 감소 정도
    public float damageDecreaseAmountBoss;  //보스에게 받는 데미지 감소 정도

    // 광물 소지 수
    public int oresSamount;
    public int oresAamount;
    public int oresBamount;
    public int oresTotalAmount;

    [SerializeField]
    private float lookSensitivity;  // 마우스 감도

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private Camera theCamera;

    public Rigidbody myRigid;  //플레이어의 움직임을 위한 컴포넌트


    public bool isUnBeat = false;  //이 값이 true 상태일때 데미지가 들어온다. false이면 무테키


    [SerializeField]
    private GameObject BuffUI;
    private bool isBuffOn = false;



    /// 사운드
    private AudioSource audioSrc;
    public AudioClip walkSound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public bool isMoving;



    /// 회피용 변수명들
    public float dodgeSpeed;        // 회피할때 순간 가속하는 속도량     // 이거 바꾸면 됨
    public float applySpeed;        // 실제 적용되는 속도(이동할때는 일단 100, 닷지를 누르면 0.3초간 250)
    private bool isDodge = false;   // 회피 체크
    public float dodgeAnimSpeed;    // 회피 애니메이션 속도 변수명       // 이거 바꾸면 됨
    public float dodgeTime;         // 0초부터 시작해서 점점 증가한다
    public float dodgeTimeCheck;    // 이게 회피 지속시간(지금은 0.3초)  // 지속시간 바꿀려면 이거만 바꾸면됨

    // 미니맵 구현 변수
    public Texture2D fogTexture;
    public int textureSize = 512;
    public int viewRadius = 10;

    private Color[] fogColors;

    // Rigidbody 컴포넌트 할당
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        ED = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyDamage>();

        // 이거 해줘야함 안그럼 0으로 시작해서 안움직임 제자리에서
        applySpeed = walkSpeed;
        // 회피 애니메이션 속도(배율)
        dodgeAnimSpeed = 1.0f;

        audioSrc = GetComponent<AudioSource>();
        ///광석 갯수 임시로 넣어줌 나중에 꼭 지워야해
        oresSamount = 0;
        oresAamount = 0;
        oresBamount = 0;

        fogTexture = new Texture2D(textureSize, textureSize);
        fogColors = new Color[textureSize * textureSize];

        for (int i = 0; i < fogColors.Length; i++)
        {
            fogColors[i] = Color.black;
        }

        fogTexture.SetPixels(fogColors);
        fogTexture.Apply();

        Renderer fogRenderer = FindAnyObjectByType<Renderer>();
        if(fogRenderer != null)
        {
            Material fogMaterial = fogRenderer.material;
            fogMaterial.SetTexture("_FogTexture", fogTexture);
        }
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
        //Dodge();                 // 회피
        Vector3 playerWorldPos = transform.position;

        Vector2Int playerTexPos = WorldToTextureCoord(playerWorldPos);

        UpdateFog(playerTexPos, viewRadius);

        MouseControl();
        CalculateOres();

        if (!uiController.isOpened && !ESCmenu.activeSelf && !dieMenu.activeSelf && !clearMenu.activeSelf && !abilityMenu.activeSelf)
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
                // 닷지 애니메이션 속도 조절 함수
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
        else
        {
            myRigid.velocity = Vector3.zero;
            Panim.SetBool("Walk", false);
        }
        BuffOn();

    }

    Vector2Int WorldToTextureCoord(Vector3 worldPos)
    {
        // 월드 좌표를 텍스처 좌표로 변환
        float x = Mathf.InverseLerp(-50f, 50f, worldPos.x) * textureSize; // 월드 범위 (-50, 50) 예시
        float y = Mathf.InverseLerp(-50f, 50f, worldPos.z) * textureSize;
        return new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
    }

    void UpdateFog(Vector2Int playerPos, int radius)
    {
        for(int y = - radius; y <= radius; y++)
        {
            for(int x = -radius; x <= radius; x++)
            {
                // 안개 걷히는것을 실시간으로 계산
                // 추가 예정
            }
        }
    }

    //총 광석 갯수 계산
    private void CalculateOres()
    {
        oresTotalAmount = oresAamount + oresBamount + oresSamount;
    }

    /// 피격 당했을때 HP가 줄어든는 함수
    public void BeAttacked()
    {

        // 현재HP에서 보스의 공격력만큼 HP가 줄어든다.
        HP -= ED.enemyDamage * ((100 - damageDecreaseAmount) / 100);
        Debug.Log(HP);
        PlaySound("HITSOUND");
        isUnBeat = false;

        if (HP <= 0)
        {
            // 사망화면으로 넘어가기
            PlaySound("DIESOUND");

        }
    }

    public void BeAttackedBossNormal()
    {



        // 현재HP에서 몬스터의 공격력만큼 HP가 줄어든다.
        HP -= ED.bossNormalDamage * ((100 -damageDecreaseAmountBoss) / 100);
        PlaySound("HITSOUND");


        if (HP <= 0)
        {
            PlaySound("DIESOUND");
            // 사망화면으로 넘어가기
        }
    }

    public void BeAttackedBossStone()
    {

        // 현재HP에서 몬스터의 공격력만큼 HP가 줄어든다.
        HP -= ED.bossThrowDamage * ((100 - damageDecreaseAmountBoss) / 100);
        PlaySound("HITSOUND");


        if (HP <= 0)
        {
            PlaySound("DIESOUND");
            // 사망화면으로 넘어가기
        }
    }
    private void Move()
    {

        // 좌우
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        // 앞뒤
        float _moveDirZ = Input.GetAxisRaw("Vertical");


            Debug.Log("켜져있는데 왜 들어와");
            Vector3 _moveHorizontal = transform.right * _moveDirX;
            Vector3 _moveVertical = transform.forward * _moveDirZ;

            Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

            myRigid.velocity = _velocity;

        //myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);        원본

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
        //Panim.SetTrigger("Dodge");


    }


    // 상하 카메라 회전
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

    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    //마우스 커서 고정/표시 여부
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
