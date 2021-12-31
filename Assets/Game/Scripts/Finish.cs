using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player)
        {
            player.finishCam = true;
            player.PlayerSpeedDown();
            PlayerPrefs.SetInt("TotalGold", UIManager.Instance.gold + PlayerPrefs.GetInt("TotalGold"));
            // AnimationController.Instance.FinishAnimationTrue();
            player.animationController.CallVictoryAnimation();
            player.FinishGameRotateTXT();
            UIManager.Instance.mainGameUI.SetActive(false);
            UIManager.Instance.UpdateGoldInfo();
            StartCoroutine(UIManager.Instance.DurationFinishUI());
            GameManager.Instance.CurrentGameState = GameState.FinishGame;
        }
    }
}