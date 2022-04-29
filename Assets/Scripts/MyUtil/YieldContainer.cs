
namespace MJ
{
    using System.Collections.Generic;
    using UnityEngine;
    public static class YieldContainer
    {
        public static WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        static Dictionary<float, WaitForSeconds> WaitForSeconds = new Dictionary<float, WaitForSeconds>();
        static Dictionary<float, WaitForSecondsRealtime> WaitForSecondsRealtime = new Dictionary<float, WaitForSecondsRealtime>();


        public static WaitForSeconds GetWaitForSeconds(float _Time)
        {
            if(!WaitForSeconds.ContainsKey(_Time))
            {
                WaitForSeconds.Add(_Time, new WaitForSeconds(_Time));
            }
            return WaitForSeconds[_Time];
        }

        public static WaitForSecondsRealtime GetWaitForSecondsRealtime(float _Time)
        {
            if (!WaitForSecondsRealtime.ContainsKey(_Time))
            {
                WaitForSecondsRealtime.Add(_Time, new WaitForSecondsRealtime(_Time));
            }
            return WaitForSecondsRealtime[_Time];
        }
    }
}
