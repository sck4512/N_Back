using UnityEngine;
using TMPro;
using System;
using System.Collections;
using MJ;

public class ScoreMessageBox : MonoBehaviour
{
    [SerializeField] GameObject serverLoading;
    [SerializeField] GameObject parentObj;
    [SerializeField] RectTransform totalScoreTextParentRect;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI incorrectText;
    [SerializeField] TextMeshProUGUI correctText;
    [SerializeField] GameObject blocker;
    public event Action onRestart;
 

    public void ShowScore(int _Correct, int _InCorrect)
    {
        parentObj.SetActive(true);


        correctText.text = _Correct.ToString();
        incorrectText.text = _InCorrect.ToString();
        var scoreTemp = _Correct / (float)(_InCorrect + _Correct);
        scoreTemp *= 100;
        int score = Mathf.RoundToInt(scoreTemp);

          

        if(score == 100 && _InCorrect > 0)
        {
            score = 99;
        }
       
        if (score == 100)
        {
            //안 맞아서 옮겨줌
            totalScoreTextParentRect.anchoredPosition = Vector2.right * 46f;
        }
        else
        {
            totalScoreTextParentRect.anchoredPosition = Vector2.zero;
        }
        totalScoreText.text = score.ToString();
    }

    public void OnClickRestart()
    {
        StartCoroutine(LoadAdsRoutine(onRestart));
        IEnumerator LoadAdsRoutine(Action _OnEnd)
        {
            serverLoading.SetActive(true);

            bool isDone = false;
            MJ.Ads.AdsManager.ShowInterstitialAd(() => isDone = true, () => isDone = true);
            yield return new WaitUntil(() => isDone);

            serverLoading.SetActive(false);
            _OnEnd.Invoke();
        }
    }



    public void OnClickHome()
    {
        StartCoroutine(OnClickHomeRoutine());
        IEnumerator OnClickHomeRoutine()
        {
            serverLoading.SetActive(true);

            bool isDone = false;
            MJ.Ads.AdsManager.ShowInterstitialAd(() => isDone = true, () => isDone = true);
            yield return new WaitUntil(() => isDone);
            serverLoading.SetActive(false);

            MyTool.LoadScene(Constants.MainSceneName);
        }
    }
}
