using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScreenController : MonoBehaviour
{

    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false; // Hide the dead screen initially
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        canvas.enabled = state == GameState.Dead;
    }

    public void OnRestartButtonClicked()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene("GameInit");
    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
