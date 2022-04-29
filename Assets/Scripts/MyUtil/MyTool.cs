using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MJ
{
    public static class MyTool
    {
        public static float GetMagnitude(Color _Color)
        {
            var result = 0f;
            result += _Color.r * _Color.r + _Color.g * _Color.g + _Color.b * _Color.b;
            return Mathf.Sqrt(result);
        }

        public static void LoadScene(string _SceneName)
        {
            SceneManager.LoadScene(_SceneName);
        }
    }
}
