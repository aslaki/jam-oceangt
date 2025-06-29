using UnityEngine;
using UnityEngine.Audio;

public class PlayerStatusController : MonoBehaviour
{

    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private float sanityDepletionRate = 5f;
    [SerializeField] private float oxygenDepletionRate = 5f;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] predatorHitClips = new AudioClip[3];
    
    private void Update()
    {
        
        if(GameManager.Instance == null ||
            GameManager.Instance.currentGameState != GameState.Game)
            return;
        playerStatus.DepleteOxygen(oxygenDepletionRate * Time.deltaTime);
        playerStatus.LoseSanity(sanityDepletionRate * Time.deltaTime);
    }

    public void OnPredatorHit() {
        playerStatus.OnPredatorHit();
        playerAudioSource.PlayOneShot(predatorHitClips[Random.Range(0, predatorHitClips.Length)]);
    }
   
}
