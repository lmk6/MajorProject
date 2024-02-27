using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameras : MonoBehaviour
{

    [SerializeField] private GameObject camera_1;

    [SerializeField] private GameObject camera_2;

    private bool _manager;
    // Start is called before the first frame update

    public void ManageCamera()
    {
        camera_1.SetActive(!_manager);
        camera_2.SetActive(_manager);
        _manager = !_manager;
    }
    void Start()
    {
        _manager = true;
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
