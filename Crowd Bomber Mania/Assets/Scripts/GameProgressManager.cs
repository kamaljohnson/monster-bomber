using System;
using UnityEngine;
using UnityEngine.Playables;
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

    public Animator animator;
    
    private static GameProgressManager _gameProgressManager;

    private static GameProgressState _progressState;

    private static bool _notifiedProgressMax;
    
    public void Start()
    {
        _notifiedProgressMax = false;
        _gameProgressManager = this;
        GetProgressPerCashFromPref();
    }

    public static void UpdateProgress()
    {
        _gameProgressManager.animator.Play("ProgressBarAnimation", -1, 0f);

        _gameProgressManager.gameProgress += 6f / (PersonSpawner.GetPersonCount() * 5f);
        
        if (_gameProgressManager.gameProgress > _gameProgressManager.progressSlider.maxValue)
        {
            if (!_notifiedProgressMax)
            {
                NotificationManager.Notify(NotificationType.LevelUp);
                _notifiedProgressMax = true;
            }
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
        _notifiedProgressMax = false;
    }

    private void UpdateUi()
    {
        progressSlider.value = gameProgress;
    }
    
}
