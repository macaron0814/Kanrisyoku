using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerManager
{
    private static BannerView bannerView;

    public static void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3937099152123084/7556581929";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3937099152123084/2603949867";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopRight);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);

        GoogleAdmob.isBanner = true;
    }
}
