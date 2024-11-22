using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using JetBrains.Annotations;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] GameObject stopWindow;
    public void ButtonOn()
    {        
        switch (this.gameObject.name)
        {
            case "StartButton":
                Thread.Sleep(1000);
                SceneManager.LoadScene("JHYMain");
                break;
            case "HowExitButton":
                stopWindow.SetActive(false);
                break;
            case "HowToPlayButton":
                stopWindow.SetActive(true);
                break;
            case "ExitButton":
                Application.Quit();
                Debug.Log("Application has quit.");
                break;
        }

    }
}
