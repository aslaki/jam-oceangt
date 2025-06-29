using UnityEngine;
using System;

// Game state enum
public enum GameState
{
    Menu,
    IntroSequence,
    Game,
    Dead
}

public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public GameState currentGameState;

    [SerializeField] private PlayerStatus playerStatus;

    [SerializeField] private Camera mainMenuCamera;

    private void OnEnable() {
        playerStatus.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable() {
        playerStatus.OnPlayerDied -= OnPlayerDied;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void ChangeGameStateChanged(GameState newState)
    {
        Debug.Log("Game state changed to: " + newState);
        switch (newState)
        {
            case GameState.Menu:
                mainMenuCamera.gameObject.SetActive(true);
                break;
            case GameState.IntroSequence:
                mainMenuCamera.gameObject.SetActive(false); 
                break;
            case GameState.Game:
                mainMenuCamera.gameObject.SetActive(false);
                break;
            case GameState.Dead:
                mainMenuCamera.gameObject.SetActive(false);
                break;
        }

        OnGameStateChanged?.Invoke(newState);
        currentGameState = newState;
    }


    public void OnStartIntro()
    {
        ChangeGameStateChanged(GameState.IntroSequence);
    }

    public void OnStartGame()
    {
        ChangeGameStateChanged(GameState.Game);
    }

    public void OnExitIntro()
    {
        ChangeGameStateChanged(GameState.Game);
    }
    
    public void OnPlayerDied()
    {
        ChangeGameStateChanged(GameState.Dead);
    }
}
