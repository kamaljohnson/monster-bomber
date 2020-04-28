using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TMP_Text levelText;

    private static int _currentLevel;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        }
        UpdateUi();
    }
    
    public void IncrementLevel()
    {
        _currentLevel ++;
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        UpdateUi();
    }

    private void UpdateUi()
    {
        levelText.text = "Level " + _currentLevel;
    }
}
