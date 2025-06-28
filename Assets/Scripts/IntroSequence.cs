using System.Collections;
using PrimeTween;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IntroSequence : MonoBehaviour
{
    [SerializeField]
    private Transform cameraEndPosition;

    [SerializeField]
    private Transform bellEndPosition;

    [SerializeField]
    private Transform bell;

    [SerializeField]
    private Light2D globalLight;

    [SerializeField]
    private float endIntensity = 0.1f;

    [SerializeField]
    private float finalCameraSize = 15f;

    [SerializeField]
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        player.SetActive(false); // Hide player during intro
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.IntroSequence)
        {
            StartCoroutine(PlayIntro());
        }
    }

    private IEnumerator PlayIntro()
    {
        // Move bell to position
        Tween.Position(bell, bellEndPosition.position, 11f);

        Tween.Custom(globalLight.intensity, endIntensity, duration: 11f,
             onValueChange: newVal => globalLight.intensity = newVal);

        // Create swinging motion - pendulum-like movement
        var swingDuration = 1.5f; // Time for one complete swing cycle

        // Start swinging immediately and loop indefinitely
        //Tween.LocalPositionX(bell, bell.localPosition.x + swingDistance, swingDuration, Ease.InOutSine, -1, CycleMode.Yoyo);

        // Add rotation swinging (bell tilting back and forth)
        var swingAngle = 15f; // Degrees of rotation swing
        Tween.LocalEulerAngles(bell, Vector3.zero, new Vector3(0, 0, swingAngle), swingDuration, Ease.InOutSine, -1, CycleMode.Yoyo);

        // Move camera to end position
        yield return Tween.PositionY(Camera.main.transform, cameraEndPosition.position.y, 10f).ToYieldInstruction();

        yield return new WaitForSeconds(1f);

        yield return Tween.ShakeCamera(Camera.main, strengthFactor: 0.5f).ToYieldInstruction();

        Camera.main.backgroundColor = Color.black;

        Tween.PositionX(Camera.main.transform, bellEndPosition.position.x, 2f, Ease.InOutSine);
        Tween.PositionY(Camera.main.transform, bellEndPosition.position.y, 2f, Ease.InOutSine);

        // Zoom in on the bell by changing camera size
        yield return Tween.Custom(Camera.main.orthographicSize, finalCameraSize, duration: 2f,
            onValueChange: newVal => Camera.main.orthographicSize = newVal).ToYieldInstruction();

        yield return new WaitForSeconds(1f);

        GameManager.Instance.OnStartGame();
        player.SetActive(true); // Show player after intro
    }
}
