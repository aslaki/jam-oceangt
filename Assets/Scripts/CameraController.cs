using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Vector2 cameraMenuPosition;

    private Camera mainCamera;

    [SerializeField]
    private Transform player;

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
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameState == GameState.Game)
        {
            // Follow the player
            if (player != null)
            {
                Vector3 targetPosition = player.position;
                targetPosition.z = mainCamera.transform.position.z; // Keep the camera's z position constant
                mainCamera.transform.position = targetPosition;
            }
        }   
    }
}
