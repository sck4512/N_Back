using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem;

public class N_BackPlayManager : MonoBehaviour
{
    [SerializeField] CameraShake cameraShake;
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] TextMeshProUGUI curN_BackTextUI;
    [SerializeField] Timer timer;
    [SerializeField] ScoreMessageBox scoreMessageBox;
    [SerializeField] N_BackNumberText n_BackNumberText;


    [Header("��ư��")]
    [SerializeField] GameObject buttonsParent;
    [SerializeField] Button O;
    [SerializeField] Button X;

    [Header("����")]
    [SerializeField] AudioSource correctSound;
    [SerializeField] AudioSource inCorrectSound;

    const float numberGapTime = 0.6f;
    int N;
    float numberShowTime;
    int playTime;
    List<int> n_backNumbers;
    int correctCount = 0;
    int incorrectCount = 0;
    event Action onCorrect;
    event Action onInCorrect;
    event Action onEnd;
    Coroutine n_BackCoroutine;

    int sameNumberCount = 0; //�������� ���� ���� ������ �� üũ
    int differentNumberCount = 0; //�������� �ٸ� ���� ������ �� üũ

    int differentNumberSpawnCountMaximum; 

    private void Awake()
    {
        N = DataManager.N_BackData.N;
        numberShowTime = 2.6f - 0.3f * (DataManager.N_BackData.Speed - 1);
        playTime = DataManager.N_BackData.PlayTime;
        n_backNumbers = new List<int>();

        //�̰Ͷ��� ����(4, 5, 6)
        if(N == 1)
        {
            differentNumberSpawnCountMaximum = Random.Range(4, 6);
        }
        else
        {
            differentNumberSpawnCountMaximum = Random.Range(4, 7);
        }


        curN_BackTextUI.text = N + " back";
        buttonsParent.SetActive(false);
        timer.Init(playTime);



        onCorrect += () => ++correctCount;
        onCorrect += () =>
        {
            O.interactable = false;
            X.interactable = false;
        };
        onCorrect += correctSound.Play;


        onInCorrect += () => ++incorrectCount;
        onInCorrect += () =>
        {
            O.interactable = false;
            X.interactable = false;
        };
        onInCorrect += () => n_BackNumberText.ChangeColor(Color.red);
        onInCorrect += inCorrectSound.Play;
        onInCorrect += Handheld.Vibrate;


        onEnd += () => InputManager.ESCInput.Disable();
        onEnd += () => scoreMessageBox.ShowScore(correctCount, incorrectCount);
        onEnd += () => StopCoroutine(n_BackCoroutine);



        scoreMessageBox.onRestart += () => InputManager.ESCInput.Enable();
        scoreMessageBox.onRestart += () =>
        {
            O.interactable = false;
            X.interactable = false;
        };
        scoreMessageBox.onRestart += () =>
        {
            correctCount = 0;
            incorrectCount = 0;
        };
        scoreMessageBox.onRestart += timer.ResetTimer;
        scoreMessageBox.onRestart += n_backNumbers.Clear;
        scoreMessageBox.onRestart += OnClickStart;
    }


    private void OnEnable()
    {
        InputManager.ESCInput.Enable();
        InputManager.ESCInput.performed += OnPerformESC;
    }

    private void OnDisable()
    {
        InputManager.ESCInput.Disable();
        InputManager.ESCInput.performed -= OnPerformESC;
    }
    void OnPerformESC(InputAction.CallbackContext _CallbackContext)
    {
        MyTool.LoadScene(Constants.MainSceneName);
    }




    public void OnClickStart()
    {
        n_BackCoroutine = StartCoroutine(RunN_Back());
    }

    IEnumerator RunN_Back()
    {
        sameNumberCount = 0;
        differentNumberCount = 0;

        numberText.text = string.Empty;
        buttonsParent.SetActive(false);
        //�ణ ��ٷȴٰ� ����
        yield return YieldContainer.GetWaitForSeconds(0.3f);

        //���� ó���� N�� ���� �޾���
        for (int i = 1; i <= N; i++)
        {
            var randomNumber = Random.Range(0, 10);
            n_backNumbers.Add(randomNumber);
            numberText.text = randomNumber.ToString();
            numberText.gameObject.SetActive(true); //����

            //�� �ð���ŭ ������
            yield return YieldContainer.GetWaitForSeconds(numberShowTime);
            numberText.gameObject.SetActive(false); //����
            yield return YieldContainer.GetWaitForSeconds(numberGapTime);
        }

        //���⼭ Ÿ�̸� ����
        timer.PlayTimer(onEnd);
        buttonsParent.SetActive(true);

        while (true)
        {
            var randomNumber = Random.Range(0, 10);
            if(Random.Range(0f, 1f) < RemoteConfigData.SameNumberPercentage && n_backNumbers[n_backNumbers.Count - N] != randomNumber)
            {
                randomNumber = n_backNumbers[n_backNumbers.Count - N];
            }

            //�������� ���� ���ڰ� 3�� �̻� �������� �ٽ� �̾���
            if(sameNumberCount >= 3 && n_backNumbers[n_backNumbers.Count - N] == randomNumber)
            {
                randomNumber = GetOtherNumber(randomNumber);
                sameNumberCount = 0;
                differentNumberCount = 0;
            } //�������� �ٸ� ���ڰ� Ư�� �̻� �������� �����ɷ� ����
            else if (differentNumberCount >= differentNumberSpawnCountMaximum && n_backNumbers[n_backNumbers.Count - N] != randomNumber)
            {
                randomNumber = n_backNumbers[n_backNumbers.Count - N];
                sameNumberCount = 0;
                differentNumberCount = 0;
            }



            n_backNumbers.Add(randomNumber);
            
            numberText.text = randomNumber.ToString();
            numberText.gameObject.SetActive(true); //����

            var n_BackIndex = n_backNumbers.Count - 1 - N;

            O.interactable = true;
            X.interactable = true;

            O.onClick.RemoveAllListeners();
            X.onClick.RemoveAllListeners();


     
            if(n_backNumbers[n_BackIndex] == randomNumber)
            {
                O.onClick.AddListener(() => onCorrect.Invoke());
                X.onClick.AddListener(() => onInCorrect.Invoke());

                sameNumberCount++;
                differentNumberCount = 0;
            }
            else
            {
                O.onClick.AddListener(() => onInCorrect.Invoke());
                X.onClick.AddListener(() => onCorrect.Invoke());

                sameNumberCount = 0;
                differentNumberCount++;
            }

            //�� �ð���ŭ ������
            yield return YieldContainer.GetWaitForSeconds(numberShowTime);

            //�� ��쵵 Ʋ���ɷ� ����
            if (O.interactable)
            {
                ++incorrectCount;
                Handheld.Vibrate();
                inCorrectSound.Play();
                //Ʋ�������� �ɱ� �� ��ٸ�
                yield return YieldContainer.GetWaitForSeconds(0.2f);
            }

            O.interactable = false;
            X.interactable = false;
            numberText.gameObject.SetActive(false); //����


            yield return YieldContainer.GetWaitForSeconds(numberGapTime);
        }
    }   

    int GetOtherNumber(int _TargetNumber)
    {
        int result = Random.Range(0, 10);
        while (result == _TargetNumber)
        {
            result = Random.Range(0, 10);
        }
        return result;
    }

    public void OnClickBack()
    {
        MyTool.LoadScene(Constants.MainSceneName);
    }
}
