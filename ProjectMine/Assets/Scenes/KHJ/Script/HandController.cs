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

    // ���� ������  Hand�� ����
    [SerializeField]
    private Hand currentHand;

    //UI ���� ���θ� ������ �ֱ� ����
    [SerializeField] UIController uiController;

    //�ɷ� ���ο� ���� �����ϴ� �Լ����� PlayerController���� ������ �ǵ������
    [SerializeField] PlayerController playerController;
    public bool isDrain = false;
    public bool isMineralMaster = false;

    // raycast�� ���ĺ������� �߰��Ѱ�
    private Camera playerCam;

    // ���� �ִϸ��̼� �ӵ� ������
    public float attackAnimSpeed;

    // �� �μ��� �Լ� ���������
    [SerializeField]
    WallScript wallScript;

    //public BoxCollider meleeArea;


    public EnemyDamage enemyDamage;

    // ������?
    private bool isAttack = false;
    private bool isSwing = false;

    // private RaycastHit hitInfo;

    public float MaxOxygen;  // �ִ� ��ҷ�

    public float Oxygen;     // ���� ���

    public float AttackOxygen;   // ���ݽ� �Ҹ� ���

    public float DodgeOxygen;    // ȸ�ǽ� �Ҹ� ���

    public float oxygenIncreaseRate;    // �ʴ� ��� ȸ����

    public float oxygenIncreasePerSecond;

    // ����
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
        oxygenIncreasePerSecond = oxygenIncreaseRate / 1.0f; // �ʴ� oxygen ������ ���

        playerCam = Camera.main;
        currentHand.applyDamage = currentHand.damage;

        /// ȿ���� 
        audioSrc = GetComponent<AudioSource>();

        wallScript = GameObject.FindGameObjectWithTag("Wall").GetComponent<WallScript>();
        // ���� �ִϸ��̼� �ӵ�(����)
        attackAnimSpeed = 1.0f;         /// �̰� �ٲٸ� ��
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

            // ���� �����ϴ� �Լ�(�ϴ� �ּ� ó���س�)
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
        if (Input.GetButton("Fire1"))     // ���콺 ��Ŭ��
        {
            if (!isAttack)
            {
                CriticalDamage();
                // �ڷ�ƾ���� ���� 
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
        audioSrc.loop = false; // �ݺ� ��� ��Ȱ��ȭ
        audioSrc.clip = clip;
        audioSrc.Play();
    }
    void PerformAreaAttack()
    {
        Vector3 boxSize = new Vector3(2f, 4f, 2f); // �ڽ� ũ�� ����
        Vector3 boxCenter = transform.position + transform.forward * 2f; // �ڽ� �߽� ��ġ ����
        Quaternion boxOrientation = transform.rotation; // �ڽ� ���� ����

        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, boxOrientation); // BoxCast ����



        foreach (Collider collider in hitColliders)
        {
            // ������ ������Ʈ�� ���� ó��
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

            //���� ���� �� �巹�� �ɷ��� ������ ������ ü���� ȸ��
            //���� �����̶� ü�� ��û ȸ���ǰ���
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
            Vector3 boxSize = new Vector3(2f, 2f, 2f); // �ڽ� ũ�� ����
            Vector3 boxCenter = transform.position; // �ڽ� �߽� ��ġ ����
            Quaternion boxOrientation = transform.rotation; // �ڽ� ���� ����

            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, boxOrientation); // BoxCast ����

            foreach (Collider collider in hitColliders)
            {
                // ������ ������Ʈ�� ���� ó��
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

                // �߰����� ó��
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
            Debug.Log("ũ��Ƽ��");
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
         // �浹 ������
         if (CheckObject())
         {
             if (hitInfo.transform.tag == "Enemy")
             {
                 // ���� hp ����(�����̲� �����;���)
                 //enemyDamage.EnemyHit();
                 hitInfo.collider.GetComponent<EnemyDamage>().EnemyHit();

                 audioSrc.PlayOneShot(moleHit);
             }
             if (hitInfo.transform.tag == "Enemy2")
             {
                 // ���� hp ����(�����̲� �����;���)
                 //enemyDamage.EnemyHit();
                 hitInfo.collider.GetComponent<EnemyDamage>().EnemyHit2();
                 //PlaySound("MOLEHIT");
                 audioSrc.PlayOneShot(moleHit);


             }
             if (hitInfo.transform.tag == "Enemy3")
             {
                 // ���� hp ����(�����̲� �����;���)
                 //enemyDamage.EnemyHit();
                 hitInfo.collider.GetComponent<EnemyDamage>().EnemyHit3();
                 //PlaySound("MOLEHIT");
                 audioSrc.PlayOneShot(moleHit);

             }

             if (hitInfo.transform.tag == "Wall")
             {
                 // �� �μ���
                 hitInfo.collider.GetComponent<WallScript>().DestroyWall();
                 audioSrc.PlayOneShot(wallHit);

             }

             if (hitInfo.transform.tag == "Mineral")
             {
                 // ������ �μ�����.
                 // �׸��� ���� ������ �μ����ٸ�
                 // �� ������ ȹ���Ѵ�.(���� ������ �����´�)

                 audioSrc.PlayOneShot(mineralHit);

             }

             isSwing = false;

             Debug.Log(hitInfo.transform.name);
         }
         // �浹 x
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

