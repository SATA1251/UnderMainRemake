using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private UIController uiController;

    public void ExitUI()
    {
        if(uiController != null) 
        {
            uiController.CloseUI();
        }
    }

    
}
