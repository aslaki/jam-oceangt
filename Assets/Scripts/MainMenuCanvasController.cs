using UnityEngine;

public class MainMenuCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenuCameraPrefab;

    private Camera _mainMenuCamera;

    void Awake()
    {
        GameObject cameraObject = Instantiate(_mainMenuCameraPrefab);
        _mainMenuCamera = cameraObject.GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().worldCamera = _mainMenuCamera;
    }    
}
