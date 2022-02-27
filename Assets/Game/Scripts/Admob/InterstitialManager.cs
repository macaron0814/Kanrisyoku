using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class InterstitialManager
{
    private static InterstitialAd interRunAd;
    private static InterstitialAd interRouletteAd;
    private static InterstitialAd interBossLoseAd;
    private static AdRequest request;

    public static int interBossLoseCount = 0;

    public static void OnInterRouletteAd()
    {
        if (interRouletteAd.IsLoaded())
        {
            interRouletteAd.Show();
        }
    }

    public static void RequestInterRouletteAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3937099152123084/6291779327";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3937099152123084/8111987833";
#else
        string adUnitId = "unexpected_platform";
#endif
        interRouletteAd = new InterstitialAd(adUnitId);

        // Called when the ad is closed.
        interRouletteAd.OnAdClosed += HandleOnInterRouletteAdClosed;

        interRouletteAd.OnAdFailedToShow += HandleOnInterRouletteAdFailedToShow;

        // Create an empty ad request.
        request = new AdRequest.Builder().Build();

        interRouletteAd.LoadAd(request);
    }
    public static void HandleOnInterRouletteAdFailedToShow(object sender, EventArgs args)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) interRouletteAd.Destroy();
    }

    public static void HandleOnInterRouletteAdClosed(object sender, EventArgs args)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) interRouletteAd.Destroy();
    }





    public static void OnInterRunAd()
    {
        if (interRunAd.IsLoaded())
        {
            interRunAd.Show();
        }
    }

    public static void RequestInterRunAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3937099152123084/6243500254";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3937099152123084/8962035073";
#else
        string adUnitId = "unexpected_platform";
#endif
        interRunAd = new InterstitialAd(adUnitId);

        // Called when the ad is closed.
        interRunAd.OnAdClosed += HandleOnInterRunAdClosed;

        interRunAd.OnAdFailedToShow += HandleOnInterRunAdFailedToShow;

        // Create an empty ad request.
        request = new AdRequest.Builder().Build();

        interRunAd.LoadAd(request);
    }
    public static void HandleOnInterRunAdFailedToShow(object sender, EventArgs args)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) interRunAd.Destroy();
    }

    public static void HandleOnInterRunAdClosed(object sender, EventArgs args)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) interRunAd.Destroy();
    }




    public static void OnInterBossLoseAd()
    {
        if (interBossLoseAd.IsLoaded())
        {
            interBossLoseAd.Show();
        }
    }

    public static void RequestInterBossLoseAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3937099152123084/7796432680";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3937099152123084/1275773284";
#else
        string adUnitId = "unexpected_platform";
#endif
        interBossLoseAd = new InterstitialAd(adUnitId);

        // Called when the ad is closed.
        interBossLoseAd.OnAdClosed += HandleOnInterBossLoseAdClosed;

        interBossLoseAd.OnAdFailedToShow += HandleOnInterBossLoseAdFailedToShow;

        // Create an empty ad request.
        request = new AdRequest.Builder().Build();

        interBossLoseAd.LoadAd(request);
    }
    public static void HandleOnInterBossLoseAdFailedToShow(object sender, EventArgs args)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) interBossLoseAd.Destroy();
    }

    public static void HandleOnInterBossLoseAdClosed(object sender, EventArgs args)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) interBossLoseAd.Destroy();
    }
}
