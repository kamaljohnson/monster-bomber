using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAdsManager : MonoBehaviour, IUnityAdsListener {

    public string myPlacementId = "rewardedVideo";
    public string gameId = "3580709";
    public bool testMode = true;

    private static RewardedAdsManager _rewardedAds;
    private static bool _adReady;

    void Awake ()
    {
        _rewardedAds = this;
        
        // Initialize the Ads listener and service:
        Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, testMode);
        IsAdReady();
    }

    // Implement a function for showing a rewarded video ad:
    public static void ShowRewardedVideo ()
    {
        GameManager.CanPlay = false;
        Advertisement.Show (_rewardedAds.myPlacementId);
    }
    
    public static bool IsAdReady()
    {
        _adReady = Advertisement.IsReady (_rewardedAds.myPlacementId);
        return _adReady;
    }
    
    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string placementId) {
        if (string.CompareOrdinal(placementId, myPlacementId) == 0)
        {
            PowerUp.adAvailable = true;
            _adReady = true;
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        if(placementId != myPlacementId) return;
        GameManager.CanPlay = true;
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            Debug.Log("watched");
            PowerUp.UpdateFreePowerUpStatus(true);
            // Reward the user for watching the ad to completion.
        } else if (showResult == ShowResult.Skipped) {
            Debug.Log("skipped");
            PowerUp.UpdateFreePowerUpStatus(false);
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            Debug.Log("failed");
            PowerUp.UpdateFreePowerUpStatus(false);
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }
    
    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
}