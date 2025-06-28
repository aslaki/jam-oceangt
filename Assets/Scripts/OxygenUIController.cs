using UnityEngine;
using UnityEngine.UI;

public class OxygenUIController : MonoBehaviour
{

    [SerializeField] private PlayerStatus playerStatus;
    private Image oxygenBar;

    private void Awake() {
        oxygenBar = GetComponent<Image>();
    }

    private void OnEnable() {
        playerStatus.OnOxygenChanged += UpdateOxygenBar;
    }

    private void OnDisable() {
        playerStatus.OnOxygenChanged -= UpdateOxygenBar;
    }

    private void UpdateOxygenBar(float oxygenPercentage) {
        oxygenBar.fillAmount = oxygenPercentage;
    }
}
