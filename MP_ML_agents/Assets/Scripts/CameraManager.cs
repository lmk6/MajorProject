using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject _activeCamera;
    private Camera[] _cameras;

    private int _cameraIndex;

    /**
     * Purpose of this function is basically to switch to the next camera on the list
     */
    private void ManageCamera()
    {
        if (_cameras.Length == 0) return;
        
        _cameras[_cameraIndex].gameObject.SetActive(false);

        _cameraIndex = (_cameraIndex + 1) % _cameras.Length;
            
        _cameras[_cameraIndex].gameObject.SetActive(true);
    }

    /**
     * Start is called before the first frame and sets only the first found camera active.
     */
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
    
    /**
     * Inside update, an if statement detects a left mouse button press to switch the camera
     */
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ManageCamera();
        }
    }
}