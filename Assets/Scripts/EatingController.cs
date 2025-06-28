using UnityEngine;
using PrimeTween;

public class EatingController : MonoBehaviour
{
    private Collider2D eatingCollider;

    [SerializeField] private PlayerStatus playerStatus;

    [SerializeField] private AudioClip[] eatingClips = new AudioClip[3];

    [SerializeField] private AudioSource playerAudioSource;


    void Awake() {
        eatingCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Fish") {
            Debug.Log("Enemy detected: " + other.gameObject.name);
            FishEffectType fishEffectType = other.gameObject.GetComponent<FishController>().GetEffectType();
            if (fishEffectType != FishEffectType.Predator) {
                playerStatus.ApplyEffect(fishEffectType);
                playerAudioSource.PlayOneShot(eatingClips[Random.Range(0, eatingClips.Length)]);
                Destroy(other.gameObject);
            }
        }
    }
}
