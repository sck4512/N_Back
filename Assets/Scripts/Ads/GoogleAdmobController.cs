using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJ
{
    namespace Ads
    {
        public class GoogleAdmobController : MonoBehaviour
        {
            InterstitialAd interstitialAd;
            Action onFailed;
            Action onClosed;
            bool isClick = false;
            //bool isInterstitialLoadFailed = false;

     
            public void Init()
            {
                //    interstitialAdID = "ca-app-pub-3940256099942544/1033173712";
                //    rewardedAdID = "ca-app-pub-3940256099942544/5354046379";

                //    List<string> deviceIds = new List<string>();
                //    deviceIds.Add("A1455337DC567126");
                //    RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetTestDeviceIds(deviceIds).build();
                //    MobileAds.SetRequestConfiguration(requestConfiguration);




                MobileAds.SetiOSAppPauseOnBackground(true);
                RequestConfiguration requestConfiguration = null;
                if (AdsManager.IsTestMode)
                {
                    List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

                    // Add some test device IDs (replace with your own device IDs).
#if UNITY_IPHONE
        deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
                    deviceIds.Add("75EF8D155528C04DACBBA6F36F433035");
                    deviceIds.Add("2E3B5F652C849BA944FF091E84B6DD2B");
#endif
                   
                        // Configure TagForChildDirectedTreatment and test device IDs.
                        requestConfiguration =
                        new RequestConfiguration.Builder()
                        .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
                        .SetTestDeviceIds(deviceIds).build();
                }
                else
                {
                    // Configure TagForChildDirectedTreatment and test device IDs.
                    requestConfiguration =
                        new RequestConfiguration.Builder()
                        .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
                        .build();
                }

                MobileAds.SetRequestConfiguration(requestConfiguration);

                // Initialize the Google Mobile Ads SDK.
                MobileAds.Initialize(HandleInitCompleteAction);



                RequestInterstitialAD();
            }

            private void HandleInitCompleteAction(InitializationStatus initstatus)
            {
                // Callbacks from GoogleMobileAds are not guaranteed to be called on
                // main thread.
                // In this example we use MobileAdsEventExecutor to schedule these calls on
                // the next Update() loop.
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    //RequestBannerAd();
                });
            }


            public void RequestInterstitialAD(/*Action _OnAdLoaded, Action _OnFailed, Action _OnAdClosed*/)
            {
                string adUnitId = string.Empty;
                if (AdsManager.IsTestMode)
                {
#if UNITY_EDITOR
                    adUnitId = "unused";
#elif UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE || UNITY_iOS
        adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        adUnitId = "unexpected_platform";
#endif
                }
                else
                {
#if UNITY_EDITOR
                    adUnitId = "unused";
#elif UNITY_ANDROID
        adUnitId = "ca-app-pub-1270408828484515/4504659513";
#elif UNITY_IPHONE || UNITY_iOS
        adUnitId = "ca-app-pub-1270408828484515/9003459104";
#else
        adUnitId = "unexpected_platform";
#endif
                }




                if (interstitialAd != null)
                {
                    interstitialAd.Destroy();
                }


                interstitialAd = new InterstitialAd(adUnitId);
                //interstitialAd.OnAdFailedToLoad += (a, b) => { isInterstitialLoadFailed = true; };
                //interstitialAd.OnAdFailedToShow += (a, b) => { _OnFailed?.Invoke(); };
                interstitialAd.OnAdClosed += (a, b) => onClosed?.Invoke();
                AdRequest request = new AdRequest.Builder().AddKeyword("unity-admob-sample").Build();
                interstitialAd.LoadAd(request);
            }
            public void ShowInterstitialAd(Action _OnFailed, Action _OnAdClosed)
            {
                if (isClick)
                {
                    return;
                }


                CoroutineExecuter.Excute(ShowInterstitialRoutine(_OnFailed, _OnAdClosed));
            }

            IEnumerator ShowInterstitialRoutine(Action _OnFailed, Action _OnAdClosed)
            {
                isClick = true;

                onFailed = _OnFailed;
                onClosed = _OnAdClosed;

                //isInterstitialLoadFailed = false;
                //RequestInterstitialAD();

                float timer = 0.5f;
                while (timer > 0f)
                {
                    timer -= Time.deltaTime;
                    //if (isInterstitialLoadFailed)
                    //{
                    //    //???? ?????????? ?????????? ???? ????
                    //    isClick = false;
                    //    yield break;
                    //}
                    if (interstitialAd.IsLoaded())
                    {
                        interstitialAd.Show();
                        RequestInterstitialAD();
                        isClick = false;
                        yield break;
                    }
                    yield return null;
                }
                onFailed?.Invoke();
                RequestInterstitialAD();
                isClick = false;
            }
        }

    }
}