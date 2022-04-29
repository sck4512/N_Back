

namespace MJ
{
    namespace Ads
    {
        using System;
        public static class AdsManager
        {
            public static bool IsTestMode => isTestMode;
            private static bool isTestMode = false;
            private static bool isInit = false;

            static GoogleAdmobController googleAdmobController;
            static MoPubController moPubController;


            public static void Init()
            {
                if(isInit)
                {
                    return;
                }

#if UNITY_EDITOR
                isTestMode = true;
#endif


                googleAdmobController = new GoogleAdmobController();
                googleAdmobController.Init();

                moPubController = new MoPubController();
                moPubController.Init();


                isInit = true;
            }


            public static void ShowInterstitialAd(Action _OnFailed, Action _OnClosed)
            {
                googleAdmobController.ShowInterstitialAd(() => moPubController.ShowInterstitialAd(_OnFailed, _OnClosed), _OnClosed);
            }
        }

    }
}
