using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string weaponName;

    // ���� ����
    public bool isPickAxe;      // ���

    public int range;           // ���ݹ���
    public float applyDamage;  // ���ݷ� �޾ƿ��� ������
    public float damage;       // �Ϲݰ��ݷ�
    public float criticalDamage; // ġ��Ÿ ���ݷ�
    public float criticalChance; // ġ��Ÿ Ȯ��
    public float attackDelay;   // ���� ������
    public float attackDelayA;  // ���� Ȱ��ȭ ����
    public float attackDelayB;  // ���� ��Ȱ��ȭ ����

    public Animator anim;       // �ִϸ��̼�
    

}
