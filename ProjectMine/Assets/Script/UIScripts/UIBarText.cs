using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;

public class UIBarText : MonoBehaviour
{
    //[SerializeField]
    //private Entity entity;

    [SerializeField]
    private Slider sliderBar;
    [SerializeField]
    private TextMeshProUGUI barText;

    // Update is called once per frame
    private void Update()
    {
        if (barText != null) barText.text = $"{sliderBar.value*100:F2}";
    }
}
