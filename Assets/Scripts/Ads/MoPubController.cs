using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoPubController
{
    const string sdkKey = "pqmRNEGalvEv_fDg8rTeqAKaki5s0BhRNzRVgddKvarTsMIIN71JNep_ZZ9FpshV0x2s1I1FgFQX-c8EPisy2M";

    const string interstitialAdUnitAndroid = "4717ae658965d2f8";
    const string interstitialAdUnitiOS = "b3fcc627a8305746";

    string interstitialAdUnit;

    Action onFailed;
    Action onClosed;

    public void Init()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += _Configuration =>
        {
            if (_Configuration.IsSuccessfullyInitialized)
            {

            }
        };

#if UNITY_ANDROID
    interstitialAdUnit = interstitialAdUnitAndroid;
#elif UNITY_IOS || UNITY_IPHONE
        interstitialAdUnit = interstitialAdUnitiOS;
#endif


        MaxSdk.SetSdkKey(sdkKey);
        MaxSdk.InitializeSdk();

        Regist();


        MaxSdk.LoadInterstitial(interstitialAdUnit);
    }


    void Regist()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (_, _2) => onFailed?.Invoke();
        //MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (_, _2) => Debug.Log("???? ??????");
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (_, _2, _3) => onFailed?.Invoke();
        //MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (_, _2, _3) => Debug.Log("???? ???????? ??????");

        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (_, _2) => onClosed?.Invoke();



        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (_, _2) => onFailed?.Invoke();
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (_, _2, _3) => onFailed?.Invoke();
        //MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (_, _2) => Debug.Log("?????? ???? ??????");
        //MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (_, _2, _3) => Debug.Log("?????? ???????? ??????");
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (_, _2, _3) => onClosed?.Invoke();
    }


    public void ShowInterstitialAd(Action _OnFailed, Action _OnClosed)
    {
        onFailed = _OnFailed;
        onClosed = _OnClosed;

        CoroutineExecuter.Excute(ShowInterstitialAdRoutine());
        IEnumerator ShowInterstitialAdRoutine()
        {
            if (!MaxSdk.IsInterstitialReady(interstitialAdUnit))
            {
                MaxSdk.LoadInterstitial(interstitialAdUnit);
            }

            float timer = 0.5f;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                if (MaxSdk.IsInterstitialReady(interstitialAdUnit))
                {
                    MaxSdk.ShowInterstitial(interstitialAdUnit);
                    MaxSdk.LoadInterstitial(interstitialAdUnit);
                    yield break;
                }
                yield return null;
            }

            onFailed?.Invoke();
        }
    }


}

