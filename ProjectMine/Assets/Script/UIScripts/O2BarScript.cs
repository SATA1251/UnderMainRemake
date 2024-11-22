using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O2BarScript : MonoBehaviour
{
    [SerializeField]
    private Slider o2Bar;

    [SerializeField]
    GameObject player;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        o2Bar.value = Mathf.Lerp(o2Bar.value, player.GetComponent<HandController>().Oxygen / 200, Time.deltaTime * 10);
    }
}