using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdmob
{
    public static bool isBanner;

    public static void LoadAdmob()
    {
        if (isBanner) { return; }

        //インターネット未接続の場合
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((initStatus) => { });

        BannerManager.RequestBanner();
        InterstitialManager.RequestInterRunAd();
        InterstitialManager.RequestInterBossLoseAd();
        InterstitialManager.RequestInterRouletteAd();
    }
}
