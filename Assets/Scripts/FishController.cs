using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;

    [SerializeField] private FishEffectType fishEffectType;

    [SerializeField] private float speed = 2f;

    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerStatus.ApplyEffect(fishEffectType);
            if (fishEffectType != FishEffectType.Predator) {
                Destroy(gameObject);
            }
        }
    }
}
