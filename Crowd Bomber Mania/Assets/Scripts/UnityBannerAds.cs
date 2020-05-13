using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Monetization;

public class UnityBannerAds : MonoBehaviour {

    public string bannerPlacement = "bottomBanner";
    public string gameId = "3580709";
    public bool testMode = true;
    public bool showAds;

    void Start () {

        if (PlayerPrefs.HasKey("NotifyRemoveAdsActivated"))
        {
            if (PlayerPrefs.GetInt("NotifyRemoveAdsActivated") == 0)
            {
                NotificationManager.Notify(NotificationType.RemoveAdsActivated);
                PlayerPrefs.SetInt("NotifyRemoveAdsActivated", 1);
            }
        }
        
        if (PlayerPrefs.HasKey("RemoveAdsActivated"))
        {
            showAds = false;
        }
        else
        {
            showAds = true;
            PlayerPrefs.SetInt("RemoveAdsActivated", 0);
        }


        if (!showAds)
        {
            return;
        }
        
        Advertisement.Initialize(gameId, testMode);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(bannerPlacement);
        StartCoroutine (ShowBannerWhenReady ());
    }

    IEnumerator ShowBannerWhenReady () {
        while (!Advertisement.IsReady (bannerPlacement)) {
            yield return new WaitForSeconds (0.5f);
        }
        Advertisement.Banner.Show (bannerPlacement);
    }
}