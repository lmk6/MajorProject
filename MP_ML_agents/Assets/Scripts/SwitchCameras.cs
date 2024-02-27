using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameras : MonoBehaviour
{
    [SerializeField] private GameObject camera_1;
    [SerializeField] private GameObject camera_2;
    [SerializeField] private GameObject camera_3;

    private bool _manager;
    private GameObject _activeCamera;

    private int _cameraState;
    // Start is called before the first frame update

    private void ManageCamera()
    {
        if(_activeCamera != null) _activeCamera.SetActive(false);
        _activeCamera = _cameraState switch
        {
            0 => camera_1,
            1 => camera_2,
            2 => camera_3,
            _ => camera_1
        };
        if (_cameraState >= 2)
        {
            _cameraState = 0;
        }
        else
        {
            _cameraState += 1;
        }
        _activeCamera.SetActive(true);
    }

    void Start()
    {
        camera_1.SetActive(false);
        camera_2.SetActive(false);
        camera_3.SetActive(false);
        ManageCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ManageCamera();
        }
    }
}