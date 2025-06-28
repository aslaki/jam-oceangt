using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{

    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private float sanityDepletionRate = 5f;
    [SerializeField] private float oxygenDepletionRate = 5f;

    private void Update() {
        playerStatus.DepleteOxygen(oxygenDepletionRate * Time.deltaTime);
        playerStatus.LoseSanity(sanityDepletionRate * Time.deltaTime);
    }
   
}
