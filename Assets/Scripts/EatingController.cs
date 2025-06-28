using UnityEngine;
using PrimeTween;

public class EatingController : MonoBehaviour
{
    private Collider2D eatingCollider;

    [SerializeField] private PlayerStatus playerStatus;


    void Awake() {
        eatingCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Fish") {
            Debug.Log("Enemy detected: " + other.gameObject.name);
            FishEffectType fishEffectType = other.gameObject.GetComponent<FishController>().GetEffectType();
            playerStatus.ApplyEffect(fishEffectType);
            if (fishEffectType != FishEffectType.Predator) {
                Destroy(other.gameObject);
            }
        }
    }
}
