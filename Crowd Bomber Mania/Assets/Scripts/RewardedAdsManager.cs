using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAdsManager : MonoBehaviour, IUnityAdsListener {

    private string gameId = "3580709";
    public bool testMode = true;
    public string myPlacementId = "rewardedVideo";

    private static RewardedAdsManager _rewardedAds;
    private static bool _adReady;

    void Awake ()
    {
        _rewardedAds = this;
        _adReady = false;
        
        // Initialize the Ads listener and service:
        Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, testMode);
    }

    // Implement a function for showing a rewarded video ad:
    public static void ShowRewardedVideo () {
        Advertisement.Show (_rewardedAds.myPlacementId);
    }
    
    public static bool IsAdReady()
    {
        _adReady = Advertisement.IsReady (_rewardedAds.myPlacementId);
        Debug.Log("ad ready: "+ _adReady);
        return _adReady;
    }
    
    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId) {        
            _adReady = true;
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            // Reward the user for watching the ad to completion.
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
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