using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameInitiator : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManagerPrefab;

    private GameManager _gameManager;

    private async void Start()
    {
        BindObjects();
        await BeginGame();
    }

    private void BindObjects()
    {
        _gameManager = Instantiate(_gameManagerPrefab);
    }

    public async Awaitable BeginGame()
    {
        await SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
    }
}