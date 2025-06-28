using UnityEngine;

public class BellController : MonoBehaviour
{

    public Transform bellPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameState != GameState.IntroSequence)
        {
            if (bellPosition.position == this.transform.position)
            {
                GameManager.Instance.OnExitIntro();
            }
        }
        
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.IntroSequence)
        {
            StartCoroutine(Tween.TweenToTarget(bellPosition, this.transform, 5f));
        }
    }
    
}
