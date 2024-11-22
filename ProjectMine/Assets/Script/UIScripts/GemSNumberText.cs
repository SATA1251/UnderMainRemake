using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GemSNumberText : MonoBehaviour
{
    [SerializeField]
    private Text gemNumberText;
    [SerializeField]
    private GameObject player;

    int playerGem;

    private void Start()
    {
    }
    void Update()
    {
        if (gemNumberText != null) gemNumberText.text = $"x{player.GetComponent<PlayerController>().oresSamount}";
    }
}
