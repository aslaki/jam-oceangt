using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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

    // [SerializeField] private Camera mainMenuCameraPrefab;

    [SerializeField] private Camera gameCamera;

    private Camera mainMenuCamera;
    private Canvas mainMenuCanvas;

    [SerializeField] private Canvas mainMenuCanvasPrefab;

    private void OnEnable() {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        playerStatus.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable() {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        playerStatus.OnPlayerDied -= OnPlayerDied;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            // mainMenuCamera = Instantiate(mainMenuCameraPrefab);
            SceneManager.SetActiveScene(scene);
            mainMenuCanvas = Instantiate(mainMenuCanvasPrefab);
            mainMenuCamera = mainMenuCanvas.GetComponent<Canvas>().worldCamera;
        }
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
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("GameManager Awake");
        }
    }

    public void SetMainMenuCamera(Camera mainMenuCamera)
    {
        this.mainMenuCamera = mainMenuCamera;
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
