using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioClipID
{
    MenuBGM,
    LevelBGM,
    ClickUI,
    CloseUI,
    HUDNotification,
    LevelStart,
    LevelCompleted,
    InteractionSuccess,
    InteractionFail,
    Timer,
    EndTimer,
    GameOver,
}

public class AudioManager : MonoBehaviour
{
    [Serializable]
    private class AudioData
    {
        public AudioClipID ID;
        public AudioClip audioClip;
        public AudioSource audioSource;

        [Range(0f, 1f)]
        public float volume;
        public bool loop;
    }

    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<AudioData> audioDataList;
    [SerializeField] private AudioMixerGroup bgmAudioMixerGroup;
    [SerializeField] private AudioMixerGroup sfxAudioMixerGroup;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        foreach(AudioData audioData in audioDataList)
        {
            audioData.audioSource = gameObject.AddComponent<AudioSource>();
            audioData.audioSource.clip = audioData.audioClip;
            audioData.audioSource.volume = audioData.volume;
            audioData.audioSource.loop = audioData.loop;

            audioData.audioSource.outputAudioMixerGroup = audioData.loop ? bgmAudioMixerGroup : sfxAudioMixerGroup;
        }
    }

    private void Start()
    {
        ChangeBGMVolume(PlayerPrefs.GetFloat("BGM Volume", 1));
        ChangeSFXVolume(PlayerPrefs.GetFloat("SFX Volume", 1));

        PlayAudio(AudioClipID.MenuBGM);
    }

    public void PlayAudio(AudioClipID audioID)
    {
        AudioData audioData = audioDataList.Find(data => data.ID == audioID);

        if(audioData == null)
        {
            return;
        }

        AudioSource audioSource = audioData.audioSource;
        audioSource.Play();
    }

    public void StopAudio(AudioClipID audioID)
    {
        AudioData audioData = audioDataList.Find(data => data.ID == audioID);
        audioData.audioSource.Stop();
    }

    public void ChangeBGMVolume(float volumeBGM)
    {
        bgmAudioMixerGroup.audioMixer.SetFloat("BGM Volume", Mathf.Log10(volumeBGM) * 20);
    }

    public void ChangeSFXVolume(float volumeSFX)
    {
        sfxAudioMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(volumeSFX) * 20);
    }

    public void SetBGMFrequency(bool reset)
    {
        bgmAudioMixerGroup.audioMixer.SetFloat("BGM Frequency", reset ? 1f : 0.3f);
    }
}
