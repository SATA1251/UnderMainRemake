using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using JetBrains.Annotations;

public class MenuButtonScript : MonoBehaviour
{
    [SerializeField] GameObject stopWindow;

    [SerializeField]
    GameObject ESCUI;
    [SerializeField]
    GameObject dieUI;
    [SerializeField]
    GameObject clearUI;



    public void ButtonOn()
    {        
        switch (this.gameObject.name)
        {
            case "StartButton":
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("Main");
                break;
            case "HowExitButton":
                stopWindow.SetActive(false);
                break;
            case "HowToPlayButton":
                stopWindow.SetActive(true);
                break;
            case "ExitButton":
                Application.Quit();
                break;
            case "MainReturnButton":
                Time.timeScale = 1.0f;                
                SceneManager.LoadScene("JHY");
                break;
            case "CloseButton":
                Time.timeScale = 1.0f;
                ESCUI.SetActive(false);
                break;
            case "RestartButton":
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("Main");
                break;

        }

    }
}
