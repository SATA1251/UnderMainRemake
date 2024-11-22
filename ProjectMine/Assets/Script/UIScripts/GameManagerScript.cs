using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public AudioSource bgmPlayer;
    void Start()
    {
        bgmPlayer.Play();
    }
}
