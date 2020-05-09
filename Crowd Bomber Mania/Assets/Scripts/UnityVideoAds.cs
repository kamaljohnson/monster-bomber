using UnityEngine;
using UnityEngine.Advertisements;

public class UnityVideoAds : MonoBehaviour, IUnityAdsListener {

    public string placementId = "video";

    public string gameId = "3580709";
    public bool testMode = true;

    private static UnityVideoAds _videoAd;
    private static bool _adReady;

    void Awake ()
    {
        _videoAd = this;
        
        // Initialize the Ads listener and service:
        Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, testMode);
        IsAdReady();
    }

    // Implement a function for showing a rewarded video ad:
    public static void ShowAd ()
    {
        Debug.Log("got response to show ad");
        GameManager.CanPlay = false;
        Advertisement.Show (_videoAd.placementId);
    }
    
    public static bool IsAdReady()
    {
        _adReady = Advertisement.IsReady (_videoAd.placementId);
        return _adReady;
    }
    
    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string placementId) {
        if (string.CompareOrdinal(placementId, placementId) == 0)
        {
            _adReady = true;
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        if(placementId != this.placementId) return;
        GameManager.CanPlay = true;
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            Debug.Log("watched");
            // Reward the user for watching the ad to completion.
        } else if (showResult == ShowResult.Skipped) {
            Debug.Log("skipped");
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            Debug.Log("failed");
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