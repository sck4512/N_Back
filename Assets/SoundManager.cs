using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if(instance == null)
                {
                    instance = new GameObject().AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    AudioSource effectSoundSpeaker;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        effectSoundSpeaker = GetComponent<AudioSource>();
    }

    public void PlayEffectSound(AudioClip _Clip)
    {
        effectSoundSpeaker.clip = _Clip;
        effectSoundSpeaker.Play();
    }
}
