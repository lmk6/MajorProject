using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject _activeCamera;
    private Camera[] _cameras;

    private int _cameraIndex;
    // Start is called before the first frame update

    private void ManageCamera()
    {
        if (_cameras.Length == 0) return;
        
        _cameras[_cameraIndex].gameObject.SetActive(false);

        _cameraIndex = (_cameraIndex + 1) % _cameras.Length;
            
        _cameras[_cameraIndex].gameObject.SetActive(true);
    }

    void Start()
    {
        _cameras = FindObjectsOfType<Camera>();
        if (_cameras.Length == 0) return;
        foreach (var camera1 in _cameras)
        {
            camera1.gameObject.SetActive(false);
        }
        _cameras[_cameraIndex].gameObject.SetActive(true);
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