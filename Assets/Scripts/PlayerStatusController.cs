using System;
using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{

    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private float sanityDepletionRate = 5f;
    [SerializeField] private float oxygenDepletionRate = 5f;
    
    public bool IsPlayerDead => playerStatus.IsDead;

    private void Update()
    {
        if(GameManager.Instance.currentGameState != GameState.Game)
            return;
        playerStatus.DepleteOxygen(oxygenDepletionRate * Time.deltaTime);
        playerStatus.LoseSanity(sanityDepletionRate * Time.deltaTime);
    }
   
}
