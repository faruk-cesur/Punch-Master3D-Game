using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
   

    private void Awake()
    {
      
    }
   
    public void CallRunAnimation()
    {
        animator.SetBool("Run", true);
        animator.SetBool("Idle", false);
        animator.SetBool("PunchEnd", false);
        animator.SetBool("PunchStart", false);
        animator.SetBool("HeadHit", false);
        animator.SetBool("ObstacleHit", false);
    }


    public void CallPunchStartAnimation()
    {
        animator.SetBool("Run", false);
        animator.SetBool("PunchStart", true);
        animator.SetBool("ObstacleHit", false);
    }
    public void CallPunchEndAnimation()
    {
        animator.SetBool("PunchStart", false);
        animator.SetBool("PunchEnd", true);
    }
    public void CallHeadHitAnimation()
    {
        animator.SetBool("PunchStart", false);
        animator.SetBool("ObstacleHit", false);
        animator.SetBool("HeadHit", true);
    }
    public void CallHeadHitRightAnimation()
    {
        animator.SetBool("PunchStart", false);
        animator.SetBool("ObstacleHit", false);
        animator.SetBool("HeadHitRight", true);
    }
    public void CallHeadHitLeftAnimation()
    {
        animator.SetBool("PunchStart", false);
        animator.SetBool("ObstacleHit", false);
        animator.SetBool("HeadHitLeft", true);
    }
    public void CallObstacleHitAnimation()
    {
        animator.SetBool("PunchStart", false);
        animator.SetBool("PunchEnd", false);
        animator.SetBool("Run", false);
        animator.SetBool("ObstacleHit", true);
    }

    public void CallVictoryAnimation()
    {
        animator.SetBool("Victory", true);
    }
    public void DeathAnimation()
    {
        animator.SetBool("Death", true);

    }
}