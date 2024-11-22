using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MainUIColler : MonoBehaviour
{
    [SerializeField]
    GameObject ESCUI;

    [SerializeField]
    GameObject dieUI;

    [SerializeField]
    GameObject clearUI;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject Boss;

    private float m_timer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCUI.SetActive(true);
            Time.timeScale = 0;
        }

        if (player.GetComponent<PlayerController>().HP <=0)
        {
            dieUI.SetActive(true);
            Time.timeScale = 0;
        }

        if (Boss.GetComponent<EnemyDamage>().hp <= 0)
        {
            m_timer += Time.deltaTime;

            if(m_timer > 5) 
            {
                clearUI.SetActive(true);
                Time.timeScale = 0;
            }          
        }
    }
}
