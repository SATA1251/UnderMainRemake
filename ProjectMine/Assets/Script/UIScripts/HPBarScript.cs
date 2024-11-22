using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarScript : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    GameObject player;
    float playerHP;
    public void Start()
    {
    }

    void Update()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, player.GetComponent<PlayerController>().HP / 500, Time.deltaTime * 10);
    }

}
