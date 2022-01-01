using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance => _instance;

    public List<GameObject> levels;

    [HideInInspector] public int currentLevel;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        SetLevelPlayerPrefs();
        CallLevel();
    }

    public void SetLevelPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
    }

    public void CallLevel()
    {
        if (currentLevel > levels.Count)
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            levels[PlayerPrefs.GetInt("CurrentLevel") - 1].SetActive(true);
        }
        else
        {
            levels[PlayerPrefs.GetInt("CurrentLevel") - 1].SetActive(true);
        }
    }

    public void NextLevel()
    {
        currentLevel++; 
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}