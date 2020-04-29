using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameProgressState
{
    Complete,
    NotComplete
}

public class GameProgressManager : MonoBehaviour
{
    public Slider progressSlider;

    public float progressStepPerCash;

    public float gameProgress;

    public float progressMultiplier;
    
    private static GameProgressManager _gameProgressManager;

    private static GameProgressState _progressState;

    public void Start()
    {
        _gameProgressManager = this;
        GetCurrentProgressState();
    }

    public static void UpdateProgress(int cash)
    {
        _gameProgressManager.gameProgress += _gameProgressManager.progressStepPerCash * cash;
        if (_gameProgressManager.gameProgress > _gameProgressManager.progressSlider.maxValue)
        {
            _progressState = GameProgressState.Complete;
        }
        else
        {
            _progressState = GameProgressState.NotComplete;
        }

        _gameProgressManager.UpdateUi();

    }

    private static void GetProgressPerCash()
    {
        if (PlayerPrefs.HasKey("ProgressPerCash"))
        {
            _gameProgressManager.progressStepPerCash = PlayerPrefs.GetFloat("ProgressPerCash");
        }
        else
        {
            PlayerPrefs.SetFloat("ProgressPerCash", _gameProgressManager.progressStepPerCash);
        }
    }

    public static void UpdateProgressPerCash()
    {
        var currentLevel = LevelManager.currentLevel;
        _gameProgressManager.progressStepPerCash *= currentLevel + _gameProgressManager.progressMultiplier;
    }
    
    public static GameProgressState GetCurrentProgressState()
    {
        return _progressState;
    }

    public static void Reset()
    {
        _gameProgressManager.gameProgress = 0;
        _progressState = GameProgressState.NotComplete;
        _gameProgressManager.UpdateUi();
    }

    private void UpdateUi()
    {
        progressSlider.value = gameProgress;
    }
    
}
