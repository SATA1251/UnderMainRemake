using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    private GameObject _minimapBackgraound;
    private GameObject _minimapShape;
    private GameObject _minimapBackgraoundOpen;
    private GameObject _minimapShapeOpen;

    private Vector3 _minimapCameraCenter;
    private Vector3 _minimapCameraBasicPosition;
    private bool _isMinimapOpen;
    // Start is called before the first frame update
    void Start()
    {
        _minimapBackgraound = GameObject.Find("MinimapBackgraound");
        _minimapShape = GameObject.Find("MinimapShape");
        _minimapBackgraoundOpen = GameObject.Find("MinimapOpenBackgraound");
        _minimapShapeOpen = GameObject.Find("MinimapOpenShape");

        _minimapBackgraoundOpen.SetActive(false);
        _minimapShapeOpen.SetActive(false);

        _isMinimapOpen = false;
    }

    // 미니맵 카메라가 맵 중앙으로 이동 할 때의 position (0.2, 0.015, 0.2)
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) // 테스트용 getket
        {
            if(_isMinimapOpen == false)
            {
                _isMinimapOpen = true;
            }
            else
            {
                _isMinimapOpen = false;
            }
        }

        if(_isMinimapOpen == true)
        {
            MinimapOpen();
        }
        else if(_isMinimapOpen == false)
        {
            MinimapClose();
        }
    }

    void MinimapOpen()
    {
        _minimapBackgraound.SetActive(false);
        _minimapShape.SetActive(false);
        _minimapBackgraoundOpen.SetActive(true);
        _minimapShapeOpen.SetActive(true);
    }

    void MinimapClose()
    {
        _minimapBackgraound.SetActive(true);
        _minimapShape.SetActive(true);
        _minimapBackgraoundOpen.SetActive(false);
        _minimapShapeOpen.SetActive(false);
    }
}
