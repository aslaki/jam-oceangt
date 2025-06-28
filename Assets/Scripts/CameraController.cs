using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Vector2 cameraMenuPosition;
    [SerializeField] private Transform cameraGamePosition;

    [SerializeField] private float startGameTweenDuration = 5f;

    private Camera mainCamera;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Awake()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        cameraMenuPosition = mainCamera.transform.position;
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Menu:
                mainCamera.transform.position = new Vector3(0, 10, -10);
                break;
            case GameState.IntroSequence:        
                StartCoroutine(Tween.TweenToTarget(cameraGamePosition, this.transform ,startGameTweenDuration));
                break;
        }
    }
}
