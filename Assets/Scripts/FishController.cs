using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;

    [SerializeField] private FishEffectType fishEffectType;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerStatus.ApplyEffect(fishEffectType);
            Destroy(gameObject);
        }
    }
}
