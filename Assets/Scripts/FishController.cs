using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class FishController : MonoBehaviour
{
    [SerializeField] private FishEffectType fishEffectType;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float minMoveDistance = 2f;
    [SerializeField] private float maxMoveDistance = 8f;
    [SerializeField] private float verticalSpeed = 3f;
    [SerializeField] private float minVerticalAmplitude = 0.2f;
    [SerializeField] private float maxVerticalAmplitude = 0.8f;
    [SerializeField] private float minPauseTime = 0.5f;
    [SerializeField] private float maxPauseTime = 3.0f;
    [SerializeField] private float fleeSpeed = 3f;
    [SerializeField] private float fleeDuration = 1f;
    [SerializeField] private float fleePauseDuration = 0.3f;
    [SerializeField] private AudioClip[] predatorHitClips = new AudioClip[2];

    private AudioSource fishAudioSource;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection = Vector2.right;
    private Vector3 initialPosition;
    private float currentMoveDistance;
    private float currentVerticalAmplitude;
    private bool isPaused = false;
    private bool isFleeing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
        fishAudioSource = GetComponent<AudioSource>();
        if (Random.value > 0.5f)
        {
            moveDirection = Vector2.left;
        }
        UpdateSpriteDirection();
        
        currentMoveDistance = Random.Range(minMoveDistance, maxMoveDistance);
        currentVerticalAmplitude = Random.Range(minVerticalAmplitude, maxVerticalAmplitude);
    }

    private void FixedUpdate()
    {
        if (isPaused || isFleeing) return;

        float horizontalVelocity = moveDirection.x * speed;
        float verticalVelocity = Mathf.Cos(Time.time * verticalSpeed) * currentVerticalAmplitude;

        rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);

        bool shouldTurn = (moveDirection.x > 0 && transform.position.x >= initialPosition.x + currentMoveDistance) ||
                          (moveDirection.x < 0 && transform.position.x <= initialPosition.x - currentMoveDistance);

        if (shouldTurn)
        {
            StartCoroutine(PauseAndTurn());
        }
    }

    private IEnumerator PauseAndTurn()
    {
        isPaused = true;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));

        moveDirection *= -1; // Flip direction
        UpdateSpriteDirection();
        currentMoveDistance = Random.Range(minMoveDistance, maxMoveDistance);
        currentVerticalAmplitude = Random.Range(minVerticalAmplitude, maxVerticalAmplitude);

        isPaused = false;
    }

    private void UpdateSpriteDirection()
    {
        spriteRenderer.flipX = moveDirection.x < 0;
    }

    public FishEffectType GetEffectType()
    {
        return fishEffectType;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (fishEffectType == FishEffectType.Predator) {
            if (other.gameObject.tag == "PredatorFishCollider") {
                other.gameObject.GetComponent<PlayerStatusController>().OnPredatorHit();
                fishAudioSource.PlayOneShot(predatorHitClips[Random.Range(0, predatorHitClips.Length)]);
            }
            if (!isFleeing) {
                StopAllCoroutines();
                StartCoroutine(Flee(other.transform));
            }
        }
        else if (other.gameObject.tag == "Player") {
            if (!isFleeing) {
                StopAllCoroutines();
                StartCoroutine(Flee(other.transform));
            }
        }
    }

    private IEnumerator Flee(Transform playerTransform)
    {
        isFleeing = true;
        isPaused = false;

        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(fleePauseDuration);

        Vector2 fleeDirection = (transform.position - playerTransform.position).normalized;
        spriteRenderer.flipX = fleeDirection.x < 0;

        rb.linearVelocity = fleeDirection * fleeSpeed;

        yield return new WaitForSeconds(fleeDuration);

        rb.linearVelocity = Vector2.zero;
        
        initialPosition = transform.position;
        moveDirection = (Random.value > 0.5f) ? Vector2.right : Vector2.left;
        UpdateSpriteDirection();
        currentMoveDistance = Random.Range(minMoveDistance, maxMoveDistance);
        currentVerticalAmplitude = Random.Range(minVerticalAmplitude, maxVerticalAmplitude);
        
        isFleeing = false;
    }
}
