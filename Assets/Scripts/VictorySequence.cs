using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VictorySequence : MonoBehaviour
{

    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private AudioSource victoryAudioSource;

    [SerializeField] private Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatus.OnPlayerWon += OnPlayerWon;
        victoryAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
#if DEBUG
        if (Keyboard.current.vKey.isPressed)
        {
            OnPlayerWon();
        }
#endif
    }

    private void OnPlayerWon()
    {
        Debug.Log("Player has won the game!");
        playerStatus.isImmortal = true; // Set player to immortal state
        StartCoroutine(PlayVictorySequence());


    }

    private IEnumerator PlayVictorySequence()
    {
        victoryAudioSource.Play();
        victoryAudioSource.transform.position = playerTransform.position;
        yield return new WaitForSeconds(victoryAudioSource.clip.length + 3f);
        SceneManager.LoadScene("GameInit");
        
        
    }
}
