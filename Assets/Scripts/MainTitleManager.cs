using System.Collections;
using UnityEngine;
using TMPro;
using MJ;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainTitleManager : MonoBehaviour
{
    [Header("옵션 세팅")]
    [SerializeField] TextMeshProUGUI n_ValueText;
    [SerializeField] TextMeshProUGUI playTimeText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] GameObject loading;


    [Header("팝업")]
    [SerializeField] GameObject n_backisPopup;
    [SerializeField] GameObject settingPopup;


    [Header("사운드")]
    [SerializeField] AudioClip optionSettingSound;

    [Header("버튼들")]
    [SerializeField] Button n_ValueLeftButton;
    [SerializeField] Button n_ValueRightButton;
    [SerializeField] Button playTimeLeftButton;
    [SerializeField] Button playTimeRightButton;
    [SerializeField] Button speedLeftButton;
    [SerializeField] Button speedRightButton;

    [Header("터치 버튼들")]
    [SerializeField] Image[] touchButtons;

    int currentNValue;
    int currentPlayTimeIndex;
    int currentSpeed;
    //bool isButtonClickable = true; //연속 클릭 막으려고 함
    const float buttonClickDelayTime = 0.1f;

    private void Awake()
    {
        RemoteConfigData.LoadData();
        MJ.Ads.AdsManager.Init();
        StartCoroutine(InitSettingDataRoutine());


        n_ValueLeftButton.onClick.AddListener(OnClickNValueLeft);
        n_ValueRightButton.onClick.AddListener(OnClickNValueRight);
        playTimeLeftButton.onClick.AddListener(OnClickPlayTimeLeft);
        playTimeRightButton.onClick.AddListener(OnClickPlayTimeRight);
        speedLeftButton.onClick.AddListener(OnClickSpeedLeft);
        speedRightButton.onClick.AddListener(OnClickSpeedRight);
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
        if(n_backisPopup.activeSelf)
        {
            n_backisPopup.SetActive(false);
            return;
        }

        if (settingPopup.activeSelf)
        {
            settingPopup.SetActive(false);
            return;
        }


        //바로 끔
        Application.Quit();
    }



    IEnumerator InitSettingDataRoutine()
    {
        if (!RemoteConfigData.IsLoaded)
        {
            InputManager.ESCInput.Disable();
            loading.SetActive(true);
            //다 불러올때까지 기다림
            yield return new WaitUntil(() => RemoteConfigData.IsLoaded);
            InputManager.ESCInput.Enable();
            loading.SetActive(false);
        }


        if (PlayerPrefs.HasKey(Constants.CurrentNValueSettingDataSaveKey))
        {
            currentNValue = PlayerPrefs.GetInt(Constants.CurrentNValueSettingDataSaveKey);
            if(currentNValue > RemoteConfigData.NValueMaximum)
            {
                currentNValue = RemoteConfigData.NValueMaximum;
            }
        }
        else
        {
            currentNValue = 1;
            PlayerPrefs.SetInt(Constants.CurrentNValueSettingDataSaveKey, currentNValue);
        }


        if (PlayerPrefs.HasKey(Constants.CurrentPlayTimeSettingDataSaveKey))
        {
            currentPlayTimeIndex = PlayerPrefs.GetInt(Constants.CurrentPlayTimeSettingDataSaveKey);
            if(RemoteConfigData.PlayTime.Length - 1 < currentPlayTimeIndex)
            {
                currentPlayTimeIndex = RemoteConfigData.PlayTime.Length - 1;
            }
        }
        else
        {
            currentPlayTimeIndex = 0;
            PlayerPrefs.SetInt(Constants.CurrentPlayTimeSettingDataSaveKey, currentPlayTimeIndex);
        }


        if (PlayerPrefs.HasKey(Constants.CurrentSpeedSettingDataSaveKey))
        {
            currentSpeed = PlayerPrefs.GetInt(Constants.CurrentSpeedSettingDataSaveKey);
        }
        else
        {
            currentSpeed = 1;
            PlayerPrefs.SetInt(Constants.CurrentSpeedSettingDataSaveKey, currentSpeed);
        }

        //데이터 세팅
        DataManager.N_BackData.SetN(currentNValue);
        DataManager.N_BackData.SetPlayTime(RemoteConfigData.PlayTime[currentPlayTimeIndex]);
        DataManager.N_BackData.SetSpeed(currentSpeed);


        //UI세팅
        n_ValueText.text = DataManager.N_BackData.N.ToString();
        playTimeText.text = DataManager.N_BackData.PlayTime.ToString();
        speedText.text = DataManager.N_BackData.Speed.ToString();
    }


    public void OnClickNValueLeft()
    {
        SoundManager.Instance.PlayEffectSound(optionSettingSound);



        //1이면 가장 작은거라 안 함
        if (currentNValue == 1)
        {
            n_ValueText.color = new Color(1, 1, 1, 0);
            CoroutineExecuter.ExcuteAfterWaitTime(() => n_ValueText.color = Color.black, 0.1f);
            return;
        }



        --currentNValue;
        DataManager.N_BackData.SetN(currentNValue);
        PlayerPrefs.SetInt(Constants.CurrentNValueSettingDataSaveKey, currentNValue);
        n_ValueText.text = DataManager.N_BackData.N.ToString();
    }
    public void OnClickNValueRight()
    {
        SoundManager.Instance.PlayEffectSound(optionSettingSound);


        //가장 클 경우 안 함
        if (currentNValue == RemoteConfigData.NValueMaximum)
        {
            n_ValueText.color = new Color(1, 1, 1, 0);
            CoroutineExecuter.ExcuteAfterWaitTime(() => n_ValueText.color = Color.black, 0.1f);
            return;
        }

        ++currentNValue;
        DataManager.N_BackData.SetN(currentNValue);
        PlayerPrefs.SetInt(Constants.CurrentNValueSettingDataSaveKey, currentNValue);
        n_ValueText.text = DataManager.N_BackData.N.ToString();
    }



    public void OnClickPlayTimeLeft()
    {
        SoundManager.Instance.PlayEffectSound(optionSettingSound);

        if (currentPlayTimeIndex == 0)
        {
            playTimeText.color = new Color(1, 1, 1, 0);
            CoroutineExecuter.ExcuteAfterWaitTime(() => playTimeText.color = Color.black, 0.1f);
            return;
        }



        --currentPlayTimeIndex;
        DataManager.N_BackData.SetPlayTime(RemoteConfigData.PlayTime[currentPlayTimeIndex]);
        PlayerPrefs.SetInt(Constants.CurrentPlayTimeSettingDataSaveKey, currentPlayTimeIndex);
        playTimeText.text = DataManager.N_BackData.PlayTime.ToString();
    }
    public void OnClickPlayTimeRight()
    {
        SoundManager.Instance.PlayEffectSound(optionSettingSound);

        if (currentPlayTimeIndex == RemoteConfigData.PlayTime.Length - 1)
        {
            playTimeText.color = new Color(1, 1, 1, 0);
            CoroutineExecuter.ExcuteAfterWaitTime(() => playTimeText.color = Color.black, 0.1f);
            return;
        }


        ++currentPlayTimeIndex;
        DataManager.N_BackData.SetPlayTime(RemoteConfigData.PlayTime[currentPlayTimeIndex]);
        PlayerPrefs.SetInt(Constants.CurrentPlayTimeSettingDataSaveKey, currentPlayTimeIndex);
        playTimeText.text = DataManager.N_BackData.PlayTime.ToString();
    }



    public void OnClickSpeedLeft()
    {
        SoundManager.Instance.PlayEffectSound(optionSettingSound);

        if (currentSpeed == 1)
        {
            speedText.color = new Color(1, 1, 1, 0);
            CoroutineExecuter.ExcuteAfterWaitTime(() => speedText.color = Color.black, 0.1f);
            return;
        }


        --currentSpeed;
        DataManager.N_BackData.SetSpeed(currentSpeed);
        PlayerPrefs.SetInt(Constants.CurrentSpeedSettingDataSaveKey, currentSpeed);
        speedText.text = DataManager.N_BackData.Speed.ToString();
    }
    public void OnClickSpeedRight()
    {
        SoundManager.Instance.PlayEffectSound(optionSettingSound);

        if (currentSpeed == Constants.SpeedMaximum)
        {
            speedText.color = new Color(1, 1, 1, 0);
            CoroutineExecuter.ExcuteAfterWaitTime(() => speedText.color = Color.black, 0.1f);
            return;
        }



        ++currentSpeed;
        DataManager.N_BackData.SetSpeed(currentSpeed);
        PlayerPrefs.SetInt(Constants.CurrentSpeedSettingDataSaveKey, currentSpeed);
        speedText.text = DataManager.N_BackData.Speed.ToString();
    }


    public void OnClickMainBG()
    {
        n_backisPopup.SetActive(false);
        settingPopup.SetActive(false);
    }

    public void OnClickStart()
    {
        MyTool.LoadScene(Constants.PlaySceneName);
    }



    public void OnButtonPress(Button _Button)
    {
        _Button.image.color = _Button.colors.pressedColor;
    }

    public void InvokeButton(Button _Button)
    {
        _Button.onClick.Invoke();
        for (int i = 0; i < touchButtons.Length; i++)
        {
            touchButtons[i].raycastTarget = false;
        }
        StartCoroutine(ChangeColor(_Button.image, _Button.colors.normalColor));

        IEnumerator ChangeColor(Image _Image, Color _TargetColor)
        {
            while (MyTool.GetMagnitude(_Image.color - _TargetColor) > 0.001f)
            {
                _Image.color = Color.Lerp(_Image.color, _TargetColor, Time.deltaTime * 31f);
                yield return null;
            }
            _Image.color = _TargetColor;

            for (int i = 0; i < touchButtons.Length; i++)
            {
                touchButtons[i].raycastTarget = true;
            }
        }
    }
}
