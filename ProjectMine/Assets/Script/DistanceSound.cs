using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSound : MonoBehaviour
{
    public Transform target;        // 거리에 따라 소리를 조절할 대상 게임 오브젝트
    public float minDistance = 1f;  // 최소 거리
    public float maxDistance = 10f; // 최대 거리
    public float maxVolume = 1f;    // 최대 볼륨

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 대상과 AudioSource 간의 거리 계산
        float distance = Vector3.Distance(target.position, transform.position);

        // 거리에 따른 가중치 계산
        float weight = 1f - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));

        // 최종 볼륨 값 계산
        float volume = weight * maxVolume;

        // AudioSource의 볼륨 값 설정
        audioSource.volume = volume;
    }
}
