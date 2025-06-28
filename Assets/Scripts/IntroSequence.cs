using System.Collections;
using PrimeTween;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    [SerializeField]
    private Transform cameraEndPosition;

    [SerializeField]
    private Transform bellEndPosition;

    [SerializeField]
    private Transform bell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
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
        // Move camera to end position
        Tween.Position(Camera.main.transform, cameraEndPosition.position, 10f);
        
        // Move bell to end position
        Tween.Position(bell, bellEndPosition.position, 10f);
        
        // Create swinging motion - pendulum-like movement
        // Swing horizontally (left and right)
        var swingDistance = 0.5f; // Adjust this value to control swing intensity
        var swingDuration = 1.5f; // Time for one complete swing cycle
        
        // Start swinging immediately and loop indefinitely
        Tween.LocalPositionX(bell, bell.localPosition.x + swingDistance, swingDuration, Ease.InOutSine, -1, CycleMode.Yoyo);
        
        // Add rotation swinging (bell tilting back and forth)
        var swingAngle = 15f; // Degrees of rotation swing
        Tween.LocalEulerAngles(bell, Vector3.zero, new Vector3(0, 0, swingAngle), swingDuration, Ease.InOutSine, -1, CycleMode.Yoyo);
        
        // Wait for the initial position tween to complete
        yield return new WaitForSeconds(10f);
    }
}
