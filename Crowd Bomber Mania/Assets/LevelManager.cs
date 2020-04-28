using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
    }

    private void Update()
    {
        levelText.text = "Level " + _currentLevel;
    }

    public void IncrementLevel()
    {
        _currentLevel ++;
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
    }
}
