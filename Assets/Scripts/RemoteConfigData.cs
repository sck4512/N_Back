using System;
using System.Collections.Generic;
using System.Linq;
namespace MJ
{
    using Unity.RemoteConfig;
    using UnityEngine;

    public static class RemoteConfigData
    {
        static readonly string environmentID = "821fc3ba-4040-4a5b-8754-7f5244551755";
        struct A
        { }
        struct B
        { }
        public static bool IsLoaded { get; set; }

        public static int NValueMaximum => nValueMaximum;
        private static int nValueMaximum;

        public static int[] PlayTime => playTime;
        private static int[] playTime;

        public static float SameNumberPercentage => sameNumberPercentage;
        private static float sameNumberPercentage;

        static RemoteConfigData()
        {
            ConfigManager.SetEnvironmentID(environmentID);
            ConfigManager.FetchCompleted += OnFetchCompleted;
        }

        public static void LoadData()
        {
            if (IsLoaded)
            {
                return;
            }

            ConfigManager.FetchConfigs<A, B>(new A(), new B());
        }

        static void OnFetchCompleted(ConfigResponse _ConfigResponse)
        {
            nValueMaximum = ConfigManager.appConfig.GetInt("NValueMaximum");

            var temp = ConfigManager.appConfig.GetJson("PlayTime");
            playTime = JsonUtility.FromJson<JsonData<int[]>>(temp).Data;

            sameNumberPercentage = ConfigManager.appConfig.GetFloat("SameNumberPercentage");


            IsLoaded = true;
        }
    }
}
