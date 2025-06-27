using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    public void OnStartButtonClicked()
    {
        startMenu.SetActive(false);
        GameManager.Instance.OnStartGame();
    }
}
