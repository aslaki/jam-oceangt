using UnityEngine;
using System;

// Game state enum
public enum GameState
{
    Menu,
    IntroSequence,
    Game
}

public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public GameState currentGameState;

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
                break;
            case GameState.IntroSequence:
                break;
            case GameState.Game:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }



    public void OnStartGame()
    {
        ChangeGameStateChanged(GameState.IntroSequence);
    }
    
    public void OnExitIntro()
    {
        ChangeGameStateChanged(GameState.Game);
    }
}
