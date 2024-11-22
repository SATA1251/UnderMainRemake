using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class returnNameAbility : MonoBehaviour
{
    [SerializeField] UIChooseAbility uiChoose;
    Transform parentsTransform;
    private Text thisText;

    private void Start()
    {
        thisText = gameObject.GetComponent<Text>();
        parentsTransform = transform.parent;
    }

    //UIChooseAbility에 선택된 이름을 넘겨줌
    public void returnName()
    {
        if(gameObject.name == "Text1stHeader" || gameObject.name == "Text1stDescription")
        {
            Transform siblingHeader = parentsTransform.Find("Text1stHeader");
            Text siblingText = siblingHeader.GetComponent<Text>();

            uiChoose.selectedName = siblingText.text;
            siblingText.color = Color.white;
            siblingText.fontSize = 60;

            Debug.Log(siblingText.text);
        }
        else
        {
            Transform siblingHeader = parentsTransform.Find("Text1stHeader");
            Text siblingText = siblingHeader.GetComponent<Text>();

            siblingText.color = Color.black;
            siblingText.fontSize = 50;
        }
        
        if (gameObject.name == "Text2ndHeader" || gameObject.name == "Text2ndDescription")
        {
            Transform siblingHeader = parentsTransform.Find("Text2ndHeader");
            Text siblingText = siblingHeader.GetComponent<Text>();

            uiChoose.selectedName = siblingText.text;
            siblingText.color = Color.white;
            siblingText.fontSize = 60;

            Debug.Log(siblingText.text);
        }
        else
        {
            Transform siblingHeader = parentsTransform.Find("Text2ndHeader");
            Text siblingText = siblingHeader.GetComponent<Text>();

            siblingText.color = Color.black;
            siblingText.fontSize = 50;
        }
        
        if (gameObject.name == "Text3rdHeader" || gameObject.name == "Text3rdDescription")
        {
            Transform siblingHeader = parentsTransform.Find("Text3rdHeader");
            Text siblingText = siblingHeader.GetComponent<Text>();

            uiChoose.selectedName = siblingText.text;
            siblingText.color = Color.white;
            siblingText.fontSize = 60;

            Debug.Log(siblingText.text);
        }
        else
        {
            Transform siblingHeader = parentsTransform.Find("Text3rdHeader");
            Text siblingText = siblingHeader.GetComponent<Text>();

            siblingText.color = Color.black;
            siblingText.fontSize = 50;
        }

        uiChoose.UpdateSelectedAbility();
    }
}
