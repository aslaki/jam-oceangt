using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    [Tooltip("The Light2D component that acts as the player's flashlight.")]
    [SerializeField] private Light2D spotLight;
    [Tooltip("Layer mask to specify which objects are considered enemies.")]
    [SerializeField] private LayerMask enemyLayer;
    [Tooltip("The number of rays to cast for enemy detection within the light cone. Higher numbers are more accurate but less performant.")]
    [SerializeField] private int numberOfRays = 20;

    private const float coneAngleOffset = 70f;
    private const float coneDistanceOffsetFactor = 0.4f;

    [SerializeField] private PlayerStatus playerStatus;

    private void OnEnable() {
        playerStatus.OnLightPowerChanged += UpdateLightPower;
    }

    private void OnDisable() {
        playerStatus.OnLightPowerChanged -= UpdateLightPower;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If the light is not assigned in the inspector, try to get it from the same GameObject.
        if (spotLight == null)
        {
            spotLight = GetComponent<Light2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemiesInCone();
    }

    private void UpdateLightPower(float lightPower) {
        spotLight.pointLightOuterRadius = lightPower;
    }

    /// <summary>
    /// Casts rays in a cone to detect enemies. The cone's angle and distance
    /// match the Light2D's outer angle and radius.
    /// </summary>
    private void DetectEnemiesInCone()
    {
        if (spotLight == null) return;

        float coneAngle = spotLight.pointLightOuterAngle - coneAngleOffset;
        float coneDistance = spotLight.pointLightOuterRadius - spotLight.pointLightOuterRadius * coneDistanceOffsetFactor;
        int rays = Mathf.Max(2, numberOfRays); // Ensure at least 2 rays
        float angleStep = coneAngle / (rays - 1);
        float startAngleOffset = -coneAngle / 2;

        for (int i = 0; i < rays; i++)
        {
            float angle = startAngleOffset + i * angleStep;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector2 direction = rotation * transform.up; // Assumes 'up' is the forward direction

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, coneDistance, enemyLayer);

            if (hit.collider != null)
            {
                // Enemy detected
                Debug.Log("Enemy detected: " + hit.collider.name);
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
            else
            {
                // Nothing hit, draw line to max distance
                Debug.DrawLine(transform.position, (Vector2)transform.position + direction * coneDistance, Color.green);
            }
        }
    }
}
