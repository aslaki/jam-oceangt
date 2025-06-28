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

        [Header("Status")]
        [SerializeField]
        private bool isDead = false;

        // Events
        public event Action<float> OnOxygenChanged;
        public event Action<float> OnSanityChanged;
        public event Action OnPlayerDied;

        // Properties
        public float MaxOxygen => maxOxygen;
        public float CurrentOxygen => currentOxygen;
        public float MaxSanity => maxSanity;
        public float CurrentSanity => currentSanity;
        public bool IsDead => isDead;

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
            isDead = false;
            OnOxygenChanged?.Invoke(oxygenPercentage);
            OnSanityChanged?.Invoke(sanityPercentage);
        }

        public void DepleteOxygen(float oxygenAmount)
        {
            if (isDead)
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
            if (isDead)
                return;

            currentSanity = Mathf.Max(0, currentSanity - sanityAmount);
            OnSanityChanged?.Invoke(sanityPercentage);

            // Check for death
            if (currentSanity <= 0 && !isDead)
            {
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
            if (isDead == dead)
                return; // No change

            isDead = dead;

            if (isDead)
            {
                OnPlayerDied?.Invoke();
            }
        }
    }

