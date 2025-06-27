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
            case GameState.Game:        
                StartCoroutine(TweenToTarget(cameraGamePosition, startGameTweenDuration));
                break;
        }
    }

    // TODO: didn't want to add any asset for this yet so just using a simple tweening function for now
    private IEnumerator TweenToTarget(Transform target, float duration)
    {
        float elapsedTime = 0;
        Vector3 startPosition = mainCamera.transform.position;
        // The target position, keeping the camera's z-axis at -10
        Vector3 endPosition = new Vector3(target.position.x, target.position.y, -10);

        while (elapsedTime < duration)  
        {
            // Calculate how far along the tween we are (0 to 1)
            float progress = elapsedTime / duration;
            // Apply an easing function for a smoother feel (optional but nice)
            float easedProgress = Mathf.SmoothStep(0, 1, progress);
            
            mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, easedProgress);
            
            // Wait for the next frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // After the loop, snap to the final position to guarantee it's correct
        mainCamera.transform.position = endPosition;
    }
}
