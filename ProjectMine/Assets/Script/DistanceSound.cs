using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSound : MonoBehaviour
{
    public Transform target;        // �Ÿ��� ���� �Ҹ��� ������ ��� ���� ������Ʈ
    public float minDistance = 1f;  // �ּ� �Ÿ�
    public float maxDistance = 10f; // �ִ� �Ÿ�
    public float maxVolume = 1f;    // �ִ� ����

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // ���� AudioSource ���� �Ÿ� ���
        float distance = Vector3.Distance(target.position, transform.position);

        // �Ÿ��� ���� ����ġ ���
        float weight = 1f - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));

        // ���� ���� �� ���
        float volume = weight * maxVolume;

        // AudioSource�� ���� �� ����
        audioSource.volume = volume;
    }
}
