using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdmob : MonoBehaviour
{
    public static bool isBanner;

    private void Awake()
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
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
