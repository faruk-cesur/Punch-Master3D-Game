using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool finishCam;
    public Transform playerModelRoot;
    public float speed;
    public float swerveSpeed;
    public float maxDistanceX;
    private float _directionX;
    private float _mousePosX;

    public AnimationController animationController;

    [SerializeField] private int playerKG = 30;

    [SerializeField] private float punchPower = 15f;
    [SerializeField] private float startingScale = 2f;
    [SerializeField] private float scaleMultiplier = 0.05f;

    [SerializeField] private Transform playerBoxerModel;
    [SerializeField] private TextMeshProUGUI playerKGText;
    [SerializeField] private Transform playerKGTxt;

    private float currentPlayerScale;

    private void Start()
    {
        currentPlayerScale = startingScale;
        CheckPlayerScaleChange();
        UpdatePlayerKgText(playerKG);
    }
    
    void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.Prepare:
                TapToStart();
                break;
            case GameState.MainGame:
                
                PlayerMovement();
                
                break;
            case GameState.GameOver:
                break;
            case GameState.FinishGame:
                break;
            default:
                break;
        }
    }

    private float PlayerTargetScale()
    {
        return (((playerKG / 10) * scaleMultiplier) + startingScale);
    }

    private void CheckPlayerScaleChange()
    {
        if (PlayerTargetScale() != currentPlayerScale)
        {
            currentPlayerScale = PlayerTargetScale();
            Vector3 targetScale = Vector3.one * currentPlayerScale;
            playerBoxerModel.DOScale(targetScale, 0.3f);
        }
    }

    #region PlayerKGArrangements

    private int GetPlayerCurrentKG()
    {
        return playerKG;
    }

    private void SetPlayerKG(int newKG)
    {
        playerKG = newKG;
        if (playerKG >= 350)
        {
            playerKG = 350;
        }
        UpdatePlayerKgText(playerKG);
        CheckPlayerScaleChange();
    }

    private void IncreasePlayerKg(int kgAmount)
    {
        if (kgAmount > 0)
        {
            int increasedPlayerKG = playerKG + kgAmount;
            SetPlayerKG(increasedPlayerKG);
        }
    }
    private void DecreasePlayerKgByObstacle(int kgAmount)
    {
        int currentPlayerKG = GetPlayerCurrentKG();
        int decreasedPlayerKg = currentPlayerKG - kgAmount;

        if (decreasedPlayerKg <= 0)
        {
            animationController.DeathAnimation();
            SetPlayerKG(0);
            GameManager.Instance.CurrentGameState = GameState.GameOver;
        }

        else
        {
            playerBoxerModel.localRotation = Quaternion.Euler(Vector3.zero);
            SetPlayerKG(decreasedPlayerKg);
        }
    }
    private IEnumerator DecreasePlayerKg(int kgAmount, EnemyScript enemy)
    {
        yield return new WaitForSeconds(0.1f);
        playerBoxerModel.position = new Vector3(playerBoxerModel.position.x, 0f, playerBoxerModel.position.z);
        int currentPlayerKG = GetPlayerCurrentKG();
        int decreasedPlayerKg = currentPlayerKG - kgAmount;

        if (decreasedPlayerKg <= 0)
        {
            animationController.DeathAnimation();
            SetPlayerKG(0);
            GameManager.Instance.CurrentGameState = GameState.GameOver;
        }

        else
        {
            if (enemy.transform.position.x > transform.position.x)
            {
                animationController.CallHeadHitRightAnimation();
            }
            else
            {
                animationController.CallHeadHitLeftAnimation();
            }
           // animationController.CallHeadHitAnimation();
            yield return new WaitForSeconds(1f);
            playerBoxerModel.localRotation = Quaternion.Euler(Vector3.zero);
            animationController.CallRunAnimation();
            SetPlayerKG(decreasedPlayerKg);
        }
    }

    private void UpdatePlayerKgText(int value)
    {
        playerKGText.text = value.ToString() + " KG";
    }

    private void GrowingUpScale()
    {

    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.CurrentGameState != GameState.MainGame)
        {
            return;
        }

        if (other.CompareTag("enemy"))
        {
            EnemyScript enemy = other.GetComponentInParent<EnemyScript>();
            if (enemy)
            {
                int enemyKG = enemy.EnemyKG;
                //CheckEnemyInteraction(enemy, enemyKG);
                StartCoroutine(CheckEnemyInteraction(enemy, enemyKG));
            }
        }

        if (other.CompareTag("collectable"))
        {
            CollectableObjects collectableObject = other.GetComponentInParent<CollectableObjects>();
            if (collectableObject)
            {
                int collectableEnergy = collectableObject.GetCollectableEnergy();
                GetEnergyFromCollectables(collectableObject, collectableEnergy);
                UIManager.Instance.EarnGoldByCollectables(2);
                collectableObject.gameObject.SetActive(false);
            }
        }

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("obstaclee");
            DecreasePlayerKgByObstacle(10);
            StartCoroutine(CallObstacleHitAnim());
        }
    }
    private IEnumerator CallObstacleHitAnim()
    {
        animationController.CallObstacleHitAnimation();
        yield return new WaitForSeconds(1.5f);
        animationController.CallRunAnimation();
    }
    private IEnumerator CheckEnemyInteraction(EnemyScript enemy, int enemyKG)
    {
        if (enemy)
        {
            if (enemyKG > playerKG)
            {
                animationController.CallPunchStartAnimation();
                enemy.CallPunchEnd();
                //DecreasePlayerKg(30);
                StartCoroutine(DecreasePlayerKg(30, enemy));
            }
            else if (enemyKG <= playerKG)
            {
                enemy.CallPunchStart();
                animationController.CallPunchEndAnimation();
                yield return new WaitForSeconds(0.1f);
                Rigidbody rigidbody = enemy.GetComponent<Rigidbody>();
                Vector3 direction = (enemy.transform.position - playerModelRoot.position).normalized;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(direction * punchPower, ForceMode.Impulse);
                enemy.GetComponentInChildren<Collider>().isTrigger = false;
                UIManager.Instance.EarnGoldByCollectables(5);
                enemy.SetEnemyKG(0);
                animationController.CallRunAnimation();
                enemy.animationController.DeathAnimation();
               
                Destroy(enemy.gameObject, 3f);
            }
        }
    }

    private void GetEnergyFromCollectables(CollectableObjects collectableObj, int collectableEnergy)
    {
        if (collectableObj)
        {
            IncreasePlayerKg(collectableEnergy);
        }
    }


    #region Straight Movement
    private void PlayerMovement()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 playerPos = playerModelRoot.localPosition;
            _directionX = playerPos.x;
            _mousePosX = GameManager.Cam.ScreenToViewportPoint(Input.mousePosition).x;
        }

        if (Input.GetMouseButton(0) && _mousePosX != 0)
        {
            float newMousePosX = GameManager.Cam.ScreenToViewportPoint(Input.mousePosition).x;
            float distance = newMousePosX - _mousePosX;
            float posX = _directionX + (distance * swerveSpeed);
            Vector3 newPlayerPos = playerModelRoot.localPosition;
            newPlayerPos.x = posX;
            newPlayerPos.x = Mathf.Clamp(newPlayerPos.x, -maxDistanceX, maxDistanceX);
            playerModelRoot.localPosition = newPlayerPos;
        }
    }

    #endregion

    public void FinishGameRotateTXT()
    {
        playerKGTxt.rotation = Quaternion.Euler(0, 180f, 0);
    }

    public void PlayerSpeedDown()
    {
        StartCoroutine(FinishGame());
    }

    IEnumerator FinishGame()
    {
        float timer = 0;
        float fixSpeed = speed;
        while (true)
        {
            timer += Time.deltaTime;
            speed = Mathf.Lerp(fixSpeed, 0, timer);

            if (timer >= 1f)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void TapToStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.CurrentGameState = GameState.MainGame;
            animationController.CallRunAnimation();
            UIManager.Instance.prepareUI.SetActive(false);
            UIManager.Instance.mainGameUI.SetActive(true);
            UIManager.Instance.weightUI.SetActive(true);
           // AnimationController.Instance.RunAnimationTrue();
        }
    }
}