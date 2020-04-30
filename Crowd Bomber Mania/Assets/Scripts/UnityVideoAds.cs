using System.Collections;
using UnityEngine;
using UnityEngine.Monetization;

public class UnityVideoAds : MonoBehaviour {

    public string placementId = "video";

    public string gameId = "3580709";
    public bool testMode = true;

    private static UnityVideoAds _videoAd;
    
    public void Start()
    {
        _videoAd = this;
        Monetization.Initialize(gameId, testMode);
    }

    public static void ShowAd()
    {
        _videoAd._ShowAd();
    }
    
    private void _ShowAd() {
        StartCoroutine (ShowAdWhenReady ());
    }

    private IEnumerator ShowAdWhenReady () {
        while (!Monetization.IsReady (placementId)) {
            yield return new WaitForSeconds(0.25f);
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent (placementId) as ShowAdPlacementContent;

        if(ad != null) {
            ad.Show ();
        }
    }
}