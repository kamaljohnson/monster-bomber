using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GameProgressState
{
    Complete,
    NotComplete
}

public class GameProgressManager : MonoBehaviour
{
    public Slider progressSlider;

    public float progressStepPerPersonInfected;

    public float gameProgress;

    public float progressMultiplier;
    
    private static GameProgressManager _gameProgressManager;

    private static GameProgressState _progressState;

    public void Start()
    {
        _gameProgressManager = this;
        GetProgressPerCashFromPref();
    }

    public static void UpdateProgress()
    {
        _gameProgressManager.gameProgress += 6f / ( PersonSpawner.GetPersonCount() * 5f);
        
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

    private static void GetProgressPerCashFromPref()
    {
        if (PlayerPrefs.HasKey("ProgressPerCash"))
        {
            _gameProgressManager.progressStepPerPersonInfected = PlayerPrefs.GetFloat("ProgressPerCash");
        }
        else
        {
            SetProgressPerCashToPref();
        }
    }

    private static void SetProgressPerCashToPref()
    {
        PlayerPrefs.SetFloat("ProgressPerCash", _gameProgressManager.progressStepPerPersonInfected);
    }
    
    public static void UpdateProgressPerCash()
    {
        _gameProgressManager.progressStepPerPersonInfected *= _gameProgressManager.progressMultiplier;
        SetProgressPerCashToPref();
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
