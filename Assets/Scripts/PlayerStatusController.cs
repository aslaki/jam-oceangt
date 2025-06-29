using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class PlayerStatusController : MonoBehaviour
{

    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private float sanityDepletionRate = 5f;
    [SerializeField] private float oxygenDepletionRate = 5f;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] predatorHitClips = new AudioClip[3];

    [SerializeField] private Sprite leftMutationSprite;
    [SerializeField] private Sprite armMutationSprite;
    [SerializeField] private Sprite headMutationSprite;
    [SerializeField] private Sprite bodyMutationSprite;

    [SerializeField] private SpriteRenderer headSpriteRenderer;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    [SerializeField] private SpriteRenderer leftLegSpriteRenderer;
    [SerializeField] private SpriteRenderer rightLegSpriteRenderer;
    [SerializeField] private SpriteRenderer leftArmSpriteRenderer;
    [SerializeField] private SpriteRenderer rightArmSpriteRenderer;

    [SerializeField] private float maxEyeLightIntensity = 2f;

    [SerializeField] private Light2D eyeLight1;
    [SerializeField] private Light2D eyeLight2;



    private void Awake()
    {
        playerStatus.OnTriggerMutation += OnTriggerMutation;
    }
    private void Update()
    {

        if (GameManager.Instance == null ||
            GameManager.Instance.currentGameState != GameState.Game)
            return;
        playerStatus.DepleteOxygen(oxygenDepletionRate * Time.deltaTime);
        playerStatus.LoseSanity(sanityDepletionRate * Time.deltaTime);
    }

    public void OnPredatorHit()
    {
        playerStatus.OnPredatorHit();
        playerAudioSource.PlayOneShot(predatorHitClips[Random.Range(0, predatorHitClips.Length)]);
    }

    private void OnTriggerMutation(int mutationIndex)
    {
        // Handle mutation effects based on the mutation index
        switch (mutationIndex)
        {
            case 1:
                bodySpriteRenderer.sprite = bodyMutationSprite;
                break;
            case 2:
                leftLegSpriteRenderer.sprite = leftMutationSprite;
                break;
            case 3:
                leftArmSpriteRenderer.sprite = armMutationSprite;
                break;
            case 4:
                rightArmSpriteRenderer.sprite = armMutationSprite;
                break;
            case 5:
                rightLegSpriteRenderer.sprite = leftMutationSprite;
                break;
            case 6:
                headSpriteRenderer.sprite = headMutationSprite;
                break;
            default:
                Debug.LogWarning("Unknown mutation index: " + mutationIndex);
                break;
        }
        UpdateEyeLightIntensity(mutationIndex);
    }

    private void UpdateEyeLightIntensity(int mutationIndex)
    {
        int maxIndex = 6;
        eyeLight1.intensity = Mathf.Lerp(eyeLight1.intensity, maxEyeLightIntensity,
         (float)mutationIndex / maxIndex);
        eyeLight2.intensity = Mathf.Lerp(eyeLight2.intensity, maxEyeLightIntensity,
         (float)mutationIndex / maxIndex);
    }
   
}
