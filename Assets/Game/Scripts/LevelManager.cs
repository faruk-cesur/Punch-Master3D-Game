using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Transform> levels;
    public LevelSetting currentLevelSetting;
    private int currentLevel;
    public static LevelManager instance;
    private void Awake()
    {
      //PlayerPrefs.SetInt("currentLevel", 0);

      instance = this;
      currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
      currentLevelSetting = levels[currentLevel].GetComponent<LevelSetting>();
      levels[currentLevel].gameObject.SetActive(true);

    }

    void Start()
    {
        UIManager.Instance.distanceFinish = currentLevelSetting.distanceFinish;
    }

    void Update()
    {
        
    }

    public void NextLevel()
    {
        levels[currentLevel].gameObject.SetActive(false);
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
    }
}
