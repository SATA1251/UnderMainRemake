using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;

public class UIHP : MonoBehaviour
{
    //[SerializeField]
    //private Entity entity;

    [SerializeField]
    private Slider sliderHP;
    [SerializeField]
    private TextMeshProUGUI textHP;

    // Update is called once per frame
    private void Update()
    {
        if (textHP != null) textHP.text = $"{sliderHP.value*100:F2}";
    }
}
