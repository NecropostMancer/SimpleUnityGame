using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//使用音频前先注册，否则不能控制音量。
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource m_MusicAudioSource;
    [SerializeField]
    private AudioSource m_ABEffectAudioSource;
    [SerializeField]
    private AudioMixer m_DefaultGlobalMixer;

    public enum AudioType
    {
        GameSFX,
        UISFX
    };

    // Start is called before the first frame update
    public void PlayOneSound(AudioClip sound,Vector3 at)
    {
        AudioSource.PlayClipAtPoint(sound, at,0.5f);
    }
    public void PlayLongSound(AudioClip audioClip)
    {
        m_MusicAudioSource.clip = audioClip;
        m_MusicAudioSource.loop = false;
        m_MusicAudioSource.Play();
    }
    public void PlayBGM(AudioClip audioClip)
    {
        if(audioClip.name == m_MusicAudioSource.clip.name) { return; }
        m_MusicAudioSource.clip = audioClip;
        m_MusicAudioSource.loop = true;
        m_MusicAudioSource.Play();
    }

    public void AudioRegister(AudioSource source, AudioType type)
    {
        switch (type)
        {
            case AudioType.GameSFX:
                source.outputAudioMixerGroup = m_DefaultGlobalMixer.FindMatchingGroups("GameSFX")[0];
                break;
            case AudioType.UISFX:
                source.outputAudioMixerGroup = m_DefaultGlobalMixer.FindMatchingGroups("UISFX")[0];
                break;
            default:
                break;
        }
        
    }

    public void SetMainVolume(float a)
    {
        m_DefaultGlobalMixer.SetFloat("MainVolume", a);
        
    }
    public void SetGameSFXVolume(float a)
    {
        m_DefaultGlobalMixer.SetFloat("GameSFXVolume", a);
    }
    public void SetUISFXVolume(float a)
    {
        m_DefaultGlobalMixer.SetFloat("UISFXVolume", a);
    }
    public void SetBGMVolume(float a)
    {
        m_DefaultGlobalMixer.SetFloat("BGMVolume", a);
    }

    void Start()
    {
        m_MusicAudioSource = gameObject.AddComponent<AudioSource>();
        m_MusicAudioSource.loop = true;
        m_MusicAudioSource.clip = AudioClip.Create("nope", 1, 1, 44100, false);
        m_MusicAudioSource.outputAudioMixerGroup = m_DefaultGlobalMixer.FindMatchingGroups("BGM")[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     
}
