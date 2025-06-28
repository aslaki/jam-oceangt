using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private float introMusicFadeDuration = 2.0f;

    private AudioSource musicAudioSource;
    private float originalVolume;
    private Coroutine fadeCoroutine;

    private GameState currentGameStateMusic;

    private bool isWaitingForIntroMusicToEnd = false;

    private void Awake() {
        musicAudioSource = GetComponent<AudioSource>();
        originalVolume = musicAudioSource.volume;
    }
    
    private void OnEnable() {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable() {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        Debug.Log("MusicManager: OnGameStateChanged: " + newState);
        if (newState == GameState.Menu) {
            PlayMusic(menuMusic);
            currentGameStateMusic = GameState.Menu;
        }
        else if (newState == GameState.IntroSequence) {
            PlayMusic(introMusic);
            currentGameStateMusic = GameState.IntroSequence;
        }
        else if (newState == GameState.Game) {
            isWaitingForIntroMusicToEnd = true;
        }
    }

    private void Update() {
        if (isWaitingForIntroMusicToEnd) {
            float progress =  Mathf.Clamp01(musicAudioSource.time / introMusic.length);
            if (progress >= 0.98f) {
                isWaitingForIntroMusicToEnd = false;
                PlayMusic(mainMusic, true, true, introMusicFadeDuration);
                currentGameStateMusic = GameState.Game;
            }
        }
    }

    private void PlayMusic(AudioClip music, bool loop = false, bool crossFade = false, float fadeDuration = 2.0f) {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        if (crossFade)
        {
            fadeCoroutine = StartCoroutine(PerformCrossFade(music, loop, fadeDuration));
        }
        else
        {
            musicAudioSource.clip = music;
            musicAudioSource.loop = loop;
            musicAudioSource.volume = originalVolume;
            musicAudioSource.Play();
        }
    }

    private IEnumerator PerformCrossFade(AudioClip newClip, bool loop, float duration)
    {
        float fadeOutDuration = duration / 2;
        float fadeInDuration = duration / 2;

        if (musicAudioSource.isPlaying)
        {
            float timer = 0f;
            float startVolume = musicAudioSource.volume;
            while (timer < fadeOutDuration)
            {
                musicAudioSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeOutDuration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            fadeInDuration = duration;
        }

        musicAudioSource.clip = newClip;
        musicAudioSource.loop = loop;
        musicAudioSource.Play();
        musicAudioSource.volume = 0;

        float fadeInTimer = 0f;
        while (fadeInTimer < fadeInDuration)
        {
            musicAudioSource.volume = Mathf.Lerp(0, originalVolume, fadeInTimer / fadeInDuration);
            fadeInTimer += Time.deltaTime;
            yield return null;
        }

        musicAudioSource.volume = originalVolume;
        fadeCoroutine = null;
    }
}
