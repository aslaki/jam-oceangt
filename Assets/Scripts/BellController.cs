using UnityEngine;
using UnityEngine.Audio;

public class BellController : MonoBehaviour
{

    [SerializeField] private AudioClip submergeSound;
    [SerializeField] private AudioClip hittingRocksSound;
    [SerializeField] private AudioClip goingToBellSound;
    [SerializeField] private AudioClip leavingBellSound;

    private AudioSource bellAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bellAudioSource = GetComponent<AudioSource>();
    }

    public void OnBellHitRocks() {
        bellAudioSource.PlayOneShot(hittingRocksSound);
    }

    public void OnBellSubmerge() {
        bellAudioSource.PlayOneShot(submergeSound);
    }

    public void OnBellGoingToBell() {
        bellAudioSource.PlayOneShot(goingToBellSound);
    }
    
    public void OnBellLeavingBell() {
        bellAudioSource.PlayOneShot(leavingBellSound);
    }
}
