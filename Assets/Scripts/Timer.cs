using System.Collections;
using UnityEngine;
using TMPro;
using System;
using MJ;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI timerText;
    int playTime = 0;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    public void Init(int _PlayTime)
    {
        playTime = _PlayTime;
        timerText.text = playTime + "s";
    }

    public void ResetTimer()
    {
        timerText.text = playTime + "s";
    }

    public void PlayTimer(Action _OnEnd)
    {
        StartCoroutine(DoTimer(_OnEnd));
    }

    IEnumerator DoTimer(Action _OnEnd)
    {
        timerText.gameObject.SetActive(true);
        int curTime = playTime;
        while (curTime > 0)
        {
            timerText.text = curTime + "s";
            yield return YieldContainer.GetWaitForSeconds(1f);
            curTime--;
        }
        timerText.text = 0 + "s";
        _OnEnd.Invoke();
    }
}
