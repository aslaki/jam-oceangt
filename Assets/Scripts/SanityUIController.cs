using UnityEngine;
using UnityEngine.UI;

public class SanityUIController : MonoBehaviour
{

    [SerializeField] private PlayerStatus playerStatus;
    private Image sanityBar;

    private void Awake() {
        sanityBar = GetComponent<Image>();
    }

    private void OnEnable() {
        playerStatus.OnSanityChanged += UpdateSanityBar;
    }

    private void OnDisable() {
        playerStatus.OnSanityChanged -= UpdateSanityBar;
    }

    private void UpdateSanityBar(float sanityPercentage) {
        sanityBar.fillAmount = sanityPercentage;
    }
}
