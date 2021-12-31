using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public PlayerController player;
    public GameObject prepareUI;
    public GameObject mainGameUI;
    public GameObject finishGameUI;
    public GameObject gameOverUI;
    public GameObject distanceFinish;
    public GameObject weightUI;
    public Slider distanceSlider;
    public TextMeshProUGUI currentGoldText;
    public TextMeshProUGUI earnedGoldText;
    public TextMeshProUGUI totalGoldText;
    public TextMeshProUGUI sliderLevelText;
    [HideInInspector] public int sliderLevel = 1;
    [HideInInspector] public int gold;

    private void Awake()
    {
        //PlayerPrefs.SetInt("SliderLevel", 1);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SetStartingUI();
        SetGoldZeroOnStart();
        SetPlayerPrefs();
    }

    private void Update()
    {
        CalculateRoadDistance();
        EqualCurrentGold();
        UpdateGoldInfo();
    }

    private void CalculateRoadDistance()
    {
        if (distanceSlider)
        {
            distanceSlider.maxValue = distanceFinish.gameObject.transform.localPosition.z;
            distanceSlider.value = player.gameObject.transform.localPosition.z;
        }
    }

    private void SetStartingUI()
    {
        prepareUI.SetActive(true);
        mainGameUI.SetActive(false);
        finishGameUI.SetActive(false);
        gameOverUI.SetActive(false);
        weightUI.SetActive(false);
    }

    private void SetGoldZeroOnStart()
    {
        gold = 0;
    }

    public void EarnGoldByCollectables(int value)
    {
        gold = gold + value;
        UpdateGoldInfo();
    }


    private void EqualCurrentGold()
    {
        currentGoldText.text = gold.ToString();
    }

    public void UpdateGoldInfo()
    {
        earnedGoldText.text = currentGoldText.text;
        totalGoldText.text = PlayerPrefs.GetInt("TotalGold").ToString();
    }


    private void SetPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("TotalGold"))
        {
            PlayerPrefs.SetInt("TotalGold", gold);
        }

        if (!PlayerPrefs.HasKey("SliderLevel"))
        {
            PlayerPrefs.SetInt("SliderLevel", sliderLevel);
        }

        sliderLevelText.text = PlayerPrefs.GetInt("SliderLevel").ToString();
    }

    public IEnumerator DurationFinishUI()
    {
        yield return new WaitForSeconds(2f);
        finishGameUI.SetActive(true);
    }

    public IEnumerator DurationGameOverUI()
    {
        // Bu method player OnCollisionEnter'da player kaybettigi zaman coroutine olarak calistirilacak.
        yield return new WaitForSeconds(2f);
        gameOverUI.SetActive(true);
    }

    public void RetryButton()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void NextLevelButton()
    {
        // Bir sonraki level Instantiate edilecek. (Level Manager'dan method cagrilabilir) Simdilik Retry ekliyorum.
        RetryButton();
        PlayerPrefs.SetInt("SliderLevel", PlayerPrefs.GetInt("SliderLevel") + 1);
        sliderLevelText.text = PlayerPrefs.GetInt("SliderLevel").ToString();
        LevelManager.instance.NextLevel();
    }
}