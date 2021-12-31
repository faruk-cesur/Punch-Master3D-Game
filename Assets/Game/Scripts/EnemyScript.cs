using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private int enemyKG = 10;
    [SerializeField] private TextMeshProUGUI enemyKGText;
    [SerializeField] private float startingScale = 2f;
    [SerializeField] private float scaleMultiplier = 0.05f;


    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform enemyChibi;
    [SerializeField] private Transform enemyModel;
    [SerializeField] private Rigidbody rb;

    public AnimationController animationController;

    public int EnemyKG
    {
        get => enemyKG;
    }

    void Start()
    {
        if (enemyKG >= 350)
        {
            enemyKG = 350;
        }
        PrepareEnemyScale();
        UpdateEnemyKgText(enemyKG);
       
    }
    
    void Update()
    {
        UpdateEnemyRotation();
        if (!IsTouchGround())
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

    }
    public bool IsTouchGround()
    {
        if (Physics.Raycast(new Vector3(transform.position.x,transform.position.y+1,transform.position.z), -Vector3.up, 100f))
        {
            return true;
        }
        return false;
    }
    private void PrepareEnemyScale()
    {
        float targetScale = ((enemyKG / 10) * scaleMultiplier) + startingScale;
        enemyChibi.localScale = Vector3.one * targetScale;
    }
  
    private void UpdateEnemyKgText(int value)
    {
        enemyKGText.text = value.ToString() + " KG";
    }

    #region RotateEnemy
    private float CalculateDisctanceFromPlayer()
    {
        return Vector3.Distance(transform.position, playerPos.position);
    }

    private void UpdateEnemyRotation()
    {
        if (CalculateDisctanceFromPlayer() <= 23f)
        {
            enemyModel.transform.LookAt(playerModel);
       
        }
    }
    #endregion

    public void SetEnemyKG(int newKG)
    {

        enemyKG = newKG;
        UpdateEnemyKgText(enemyKG);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.CurrentGameState != GameState.MainGame)
        {
            return;
        }
    }

    public void CallPunchStart()
    {
        animationController.CallPunchStartAnimation();
    }

    public void CallPunchEnd()
    {
        animationController.CallPunchEndAnimation();
    }
}
