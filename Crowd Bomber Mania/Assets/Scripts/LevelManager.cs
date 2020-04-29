using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TMP_Text levelText;

    public static int currentLevel = 1;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        }
        UpdateUi();
    }
    
    public void IncrementLevel()
    {
        currentLevel ++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        GameProgressManager.UpdateProgressPerCash();
        Person.UpdatePersonCash();
        UpdateUi();
    }

    private void UpdateUi()
    {
        levelText.text = "Level " + currentLevel;
    }
}
