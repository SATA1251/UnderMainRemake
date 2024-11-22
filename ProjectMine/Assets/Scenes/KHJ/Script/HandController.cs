using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;



public class HandController : MonoBehaviour
{
    [SerializeField] private BoxCollider attackCollider;

    // 현재 장착된  Hand형 무기
    [SerializeField]
    private Hand currentHand;

    //UI 오픈 여부를 가지고 있기 위해
    [SerializeField] UIController uiController;

    //능력 여부에 따라 공격하는 함수에서 PlayerController쪽의 스탯을 건드려야함
    [SerializeField] PlayerController playerController;
    public bool isDrain = false;
    public bool isMineralMaster = false;

    // raycast를 고쳐보기위해 추가한거
    private Camera playerCam;

    // 공격 애니메이션 속도 변수명
    public float attackAnimSpeed;

    // 벽 부수는 함수 가져오기용
    [SerializeField]
    WallScript wallScript;

    //public BoxCollider meleeArea;


    public EnemyDamage enemyDamage;

    // 공격중?
    private bool isAttack = false;
    private bool isSwing = false;

    // private RaycastHit hitInfo;

    public float MaxOxygen;  // 최대 산소량

    public float Oxygen;     // 현재 산소

    public float AttackOxygen;   // 공격시 소모 산소

    public float DodgeOxygen;    // 회피시 소모 산소

    public float oxygenIncreaseRate;    // 초당 산소 회복량

    public float oxygenIncreasePerSecond;

    // 사운드
    private AudioSource audioSrc;

    public AudioClip hitSound;
    public AudioClip dieSound;
    public AudioClip moleHit;
    public AudioClip wallHit;
    public AudioClip mineralHit;
    public AudioClip bossHit;
    private Collision testColl;

    private bool isPlayingSound;



    void Start()
    {
        //currentHand = GameObject.Find("Hand").GetComponent<Hand>();

        enemyDamage = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyDamage>();
        oxygenIncreaseRate = 40.0f;
        oxygenIncreasePerSecond = oxygenIncreaseRate / 1.0f; // 초당 oxygen 증가량 계산

        playerCam = Camera.main;
        currentHand.applyDamage = currentHand.damage;

        /// 효과음 
        audioSrc = GetComponent<AudioSource>();

        wallScript = GameObject.FindGameObjectWithTag("Wall").GetComponent<WallScript>();
        // 공격 애니메이션 속도(배율)
        attackAnimSpeed = 1.0f;         /// 이거 바꾸면 됨
    }



    // Update is called once per frame
    void Update()
    {
        if (!uiController.isOpened)
        {
            if (Oxygen >= AttackOxygen)
            {
                TryAttack();
            }
            IncreaseOxygen();

            // 공속 조절하는 함수(일단 주석 처리해놈)
            currentHand.anim.SetFloat("AttackSpeed", attackAnimSpeed);
        }



    }

    private void PlaySound(string action)
    {
        switch (action)
        {
            case "ATTACK":
                audioSrc.clip = hitSound;
                break;
            case "DIE":
                audioSrc.clip = dieSound;
                break;
            case "MOLEHIT":
                audioSrc.clip = moleHit;
                break;
            case "WALLHIT":
                audioSrc.clip = wallHit;
                break;
            case "MINERALHIT":
                audioSrc.clip = mineralHit;
                break;
            case "BOSSHIT":
                audioSrc.clip = bossHit;
                break;
        }
        audioSrc.Play();
    }

    private void IncreaseOxygen()
    {
        Oxygen += oxygenIncreasePerSecond * Time.deltaTime;
        Oxygen = Mathf.Clamp(Oxygen, 0, MaxOxygen);
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))     // 마우스 좌클릭
        {
            if (!isAttack)
            {
                CriticalDamage();
                // 코루틴으로 실행 
                StartCoroutine(AttackCoroutine());
            }
        }

    }


    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);

        attackCollider.enabled = true;

        isSwing = true;
        PlaySound("ATTACK");
        //StartCoroutine(HitCoroutine());
        Oxygen -= AttackOxygen;
        PerformAreaAttack();

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;
        attackCollider.enabled = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }
    private void PlaySoundOnce(AudioClip clip)
    {
        audioSrc.loop = false; // 반복 재생 비활성화
        audioSrc.clip = clip;
        audioSrc.Play();
    }
    void PerformAreaAttack()
    {
        Vector3 boxSize = new Vector3(2f, 4f, 2f); // 박스 크기 설정
        Vector3 boxCenter = transform.position + transform.forward * 2f; // 박스 중심 위치 설정
        Quaternion boxOrientation = transform.rotation; // 박스 방향 설정

        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, boxOrientation); // BoxCast 수행



        foreach (Collider collider in hitColliders)
        {
            // 감지된 오브젝트에 대한 처리
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<EnemyDamage>().EnemyHit();
                audioSrc.PlayOneShot(moleHit);

            }
            else if (collider.CompareTag("Enemy2"))
            {
                collider.GetComponent<EnemyDamage>().EnemyHit2();
                audioSrc.PlayOneShot(moleHit);

            }
            else if (collider.CompareTag("Wall"))
            {
                collider.GetComponent<WallScript>().DestroyWall();

                PlaySoundOnce(wallHit);
                // audioSrc.PlayOneShot(wallHit);

            }
            else if (collider.CompareTag("Boss"))
            {
                collider.GetComponent<EnemyDamage>().BossHit();

                audioSrc.PlayOneShot(wallHit);

            }
            else if (collider.CompareTag("GemS"))
            {
                collider.GetComponent<GemScript>().DestroyGem();

                audioSrc.PlayOneShot(mineralHit);

            }
            else if (collider.CompareTag("GemA"))
            {
                collider.GetComponent<GemScript>().DestroyGem();

                audioSrc.PlayOneShot(mineralHit);
            }
            else if (collider.CompareTag("GemB"))
            {
                collider.GetComponent<GemScript>().DestroyGem();

                audioSrc.PlayOneShot(mineralHit);
            }

            //공격 실행 후 드레인 능력을 가지고 있으면 체력을 회복
            //광역 공격이라 체력 엄청 회복되겠지
            if (isDrain)
            {
                if (collider.CompareTag("Enemy") || collider.CompareTag("Enemy2") || collider.CompareTag("Boss"))
                {                   
                    playerController.HP += playerController.MaxHP * 0.1f;

                }
                else if (collider.CompareTag("Wall"))
                {
                    playerController.HP += playerController.MaxHP * 0.05f;
                }
            }         
        }

    }





    /*    void DetectNearbyObjects()
        {
            Vector3 boxSize = new Vector3(2f, 2f, 2f); // 박스 크기 설정
            Vector3 boxCenter = transform.position; // 박스 중심 위치 설정
            Quaternion boxOrientation = transform.rotation; // 박스 방향 설정

            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, boxOrientation); // BoxCast 수행

            foreach (Collider collider in hitColliders)
            {
                // 감지된 오브젝트에 대한 처리
                if (collider.CompareTag("Enemy"))
                {
                    GetComponent<EnemyDamage>().EnemyHit();

                    audioSrc.PlayOneShot(moleHit);
                }
                if (collider.CompareTag("Enemy2"))
                {
                    collision.gameObject.GetComponent<EnemyDamage>().EnemyHit();

                    audioSrc.PlayOneShot(moleHit);
                }
                if (collider.CompareTag("Enemy3"))
                {
                    collision.gameObject.GetComponent<EnemyDamage>().EnemyHit();

                    audioSrc.PlayOneShot(moleHit);
                }
                else if (collider.CompareTag("Wall"))
                {

                }
                // ...

                // 추가적인 처리
            }
        }*/

    /*    public void OnCollisionEnter(Collision collision)
        {
            GameObject collidedObject = collision.gameObject;
            string objectTag = collidedObject.tag;

            if (objectTag == "Enemy")
            {
                collidedObject.GetComponent<EnemyDamage>().EnemyHit();

                audioSrc.PlayOneShot(moleHit);

                return;
            }
            if (objectTag == "Enemy2")
            {
                collidedObject.GetComponent<EnemyDamage>().EnemyHit2();

                audioSrc.PlayOneShot(moleHit);

                return ;
            }
            if (objectTag == "Enemy3")
            {
                collidedObject.GetComponent<EnemyDamage>().EnemyHit3();

                audioSrc.PlayOneShot(moleHit);

                return;
            }
            if (objectTag == "Wall")
            {
                Debug.Log(1);

                collidedObject.GetComponent<WallScript>().DestroyWall();
                audioSrc.PlayOneShot(wallHit);

                return;

            }
            if (objectTag == "Boss")
            {
                collidedObject.GetComponent<EnemyDamage>().BossHit();

                return;

            }
            Debug.Log(objectTag);
        }*/

    private void CriticalDamage()
    {
        if (Random.value <= currentHand.criticalChance / 100)
        {
            currentHand.applyDamage = currentHand.damage + currentHand.criticalDamage;
            Debug.Log("크리티컬");
        }
        else
        {
            currentHand.applyDamage = currentHand.damage;
        }
    }

}

/* IEnumerator HitCoroutine()
 {
     while (isSwing)
     {
         // 충돌 됬을때
         if (CheckObject())
         {
             if (hitInfo.transform.tag == "Enemy")
             {
                 // 적의 hp 감소(성근이꺼 가져와야함)
                 //enemyDamage.EnemyHit();
                 hitInfo.collider.GetComponent<EnemyDamage>().EnemyHit();

                 audioSrc.PlayOneShot(moleHit);
             }
             if (hitInfo.transform.tag == "Enemy2")
             {
                 // 적의 hp 감소(성근이꺼 가져와야함)
                 //enemyDamage.EnemyHit();
                 hitInfo.collider.GetComponent<EnemyDamage>().EnemyHit2();
                 //PlaySound("MOLEHIT");
                 audioSrc.PlayOneShot(moleHit);


             }
             if (hitInfo.transform.tag == "Enemy3")
             {
                 // 적의 hp 감소(성근이꺼 가져와야함)
                 //enemyDamage.EnemyHit();
                 hitInfo.collider.GetComponent<EnemyDamage>().EnemyHit3();
                 //PlaySound("MOLEHIT");
                 audioSrc.PlayOneShot(moleHit);

             }

             if (hitInfo.transform.tag == "Wall")
             {
                 // 벽 부수기
                 hitInfo.collider.GetComponent<WallScript>().DestroyWall();
                 audioSrc.PlayOneShot(wallHit);

             }

             if (hitInfo.transform.tag == "Mineral")
             {
                 // 광물이 부셔진다.
                 // 그리고 만약 광물이 부셔졌다면
                 // 그 광물을 획득한다.(광물 정보를 가져온다)

                 audioSrc.PlayOneShot(mineralHit);

             }

             isSwing = false;

             Debug.Log(hitInfo.transform.name);
         }
         // 충돌 x
         yield return null;
     }
 }

 private bool CheckObject()
 {
     Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));

     Vector3 rayDir = playerCam.transform.forward;

     if (Physics.BoxCast(rayOrigin, transform.lossyScale / 2, rayDir, out hitInfo, transform.rotation, currentHand.range, ~LayerMask.GetMask("Player")))
     // if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
     {

         return true;
     }
     return false;
 }*/

