using System;
using UnityEngine;
using UnityEngine.SceneManagement;


    [CreateAssetMenu(fileName = "PlayerStatus", menuName = "jam-oceangt/Player Status")]
    public class PlayerStatus : ScriptableObject
    {
        [Header("Oxygen Settings")]
        [SerializeField]
        private float maxOxygen = 100f;

        [SerializeField]
        private float currentOxygen;

        [Header("Sanity Settings")]
        [SerializeField]
        private float maxSanity = 100f;

        [SerializeField]
        private float currentSanity;

        [Header("Light settings")]
        [SerializeField]
        private float maxLightPower = 100f;
        [SerializeField]
        private float currentLightPower;

        [Header("Status")]
        [SerializeField]
        private bool isDead = false;
        
        [SerializeField]
        private int fishEatCount = 0;

        private int requiredFishEatCount = 30; // Number of fish to eat to win
        
        private int[] mutationThresholds = new int[] { 5, 10, 15, 20, 25, 30 }; // Thresholds for mutations};

        [SerializeField]
        private int currentMutationIndex = 0;
        // Events
    public event Action<float> OnOxygenChanged;
        public event Action<float> OnSanityChanged;
        public event Action<float> OnLightPowerChanged;

        public event Action<int> OnTriggerMutation;

        public event Action OnPlayerDied;
        
        public event Action OnPlayerWon;

        // Properties
    public float MaxOxygen => maxOxygen;
        public float CurrentOxygen => currentOxygen;
        public float MaxSanity => maxSanity;
        public float CurrentSanity => currentSanity;
        public float CurrentLightPower => currentLightPower;
        public bool IsDead => isDead;
        public bool isImmortal = false;

        // Normalized health value (0-1)
    private float oxygenPercentage => Mathf.Clamp01(currentOxygen / maxOxygen);
        private float sanityPercentage => Mathf.Clamp01(currentSanity / maxSanity);

        // Reset status when scene is reloaded
        private void OnSceneLoaded(
            UnityEngine.SceneManagement.Scene scene,
            UnityEngine.SceneManagement.LoadSceneMode mode
        )
        {
            ResetStatus();
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            ResetStatus();
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnDestroy()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // Reset player status to default values
        public void ResetStatus()
        {
            Debug.Log("Resetting player status");
            currentOxygen = maxOxygen;
            currentSanity = maxSanity;
            currentLightPower = 15f;
            isDead = false;
            fishEatCount = 0;
            currentMutationIndex = 0;
            isImmortal = false;
            OnOxygenChanged?.Invoke(oxygenPercentage);
            OnSanityChanged?.Invoke(sanityPercentage);
            OnLightPowerChanged?.Invoke(currentLightPower);
        }

        public void ApplyEffect(FishEffectType fishEffectType) {
            switch (fishEffectType) {
                case FishEffectType.Light:
                    IncreaseLightPower(50);
                    break;
                case FishEffectType.Sanity:
                    GainSanity(10);
                    break;
                case FishEffectType.Oxygen:
                    GainOxygen(10);
                    break;
                case FishEffectType.Predator:
                    LoseSanity(20);
                    DepleteOxygen(20);
                    break;
            }
            fishEatCount++;
            Debug.Log($"Fish eaten: {fishEatCount}");
            for(int i = 0; i < mutationThresholds.Length; i++) {
            if (fishEatCount == mutationThresholds[i])
            {
                currentMutationIndex++;
                OnTriggerMutation?.Invoke(currentMutationIndex); // Trigger mutation event
                Debug.Log($"Triggered mutation {currentMutationIndex} at fish count {fishEatCount}");

            }
            }
            if (fishEatCount >= requiredFishEatCount) {
                Debug.Log("Player has eaten enough fish to win!");
                isImmortal = true; // Set player to immortal state
                OnPlayerWon?.Invoke();
            }
            
            
        }

        public void OnPredatorHit() {
            LoseSanity(20);
            DepleteOxygen(20);
        }

        public void IncreaseLightPower(float lightPowerAmount)
        {
            currentLightPower = Mathf.Min(maxLightPower, currentLightPower + lightPowerAmount);
            OnLightPowerChanged?.Invoke(currentLightPower);
        }

        public void DepleteOxygen(float oxygenAmount)
        {
            if (isDead || isImmortal)
                return;

            currentOxygen = Mathf.Max(0, currentOxygen - oxygenAmount);
            OnOxygenChanged?.Invoke(oxygenPercentage);

            // Check for death
            if (currentOxygen <= 0 && !isDead)
            {
                SetDead(true);
            }
        }

        public void GainOxygen(float oxygenAmount)
        {
            currentOxygen = Mathf.Min(maxOxygen, currentOxygen + oxygenAmount);
            OnOxygenChanged?.Invoke(oxygenPercentage);
        }

        public void LoseSanity(float sanityAmount)
        {
            Debug.Log("Losing sanity: " + sanityAmount);
            if (isDead || isImmortal)
                return;

            currentSanity = Mathf.Max(0, currentSanity - sanityAmount);
            OnSanityChanged?.Invoke(sanityPercentage);

            // Check for death
            if (currentSanity <= 0 && !isDead)
            {
                Debug.Log("Player is dead");
                SetDead(true);
            }
        }

        public void GainSanity(float sanityAmount)
        {
            currentSanity = Mathf.Min(maxSanity, currentSanity + sanityAmount);
            OnSanityChanged?.Invoke(sanityPercentage);
        }

        // Heal the player
        public void Heal(float healAmount)
        {
            if (isDead)
                return; // Can't heal if dead

            currentOxygen = Mathf.Min(maxOxygen, currentOxygen + healAmount);
            OnOxygenChanged?.Invoke(oxygenPercentage);
        }

        // Set player's dead state
        public void SetDead(bool dead)
        {
            Debug.Log("Setting player dead state to: " + dead);
            if (isDead == dead)
                return; // No change

            isDead = dead;

            if (isDead)
            {
                OnPlayerDied?.Invoke();
            }
        }
    }

