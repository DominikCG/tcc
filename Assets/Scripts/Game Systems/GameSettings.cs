using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private float currentBGMVolume;
    private float currentSFXVolume;

    private bool finishedInitialSet = false;

    private void Awake() => LoadSettings();

    private void LoadSettings()
    {
        SetBGMVolume(currentBGMVolume = PlayerPrefs.GetFloat("BGM Volume", 1));
        SetSFXVolume(currentSFXVolume = PlayerPrefs.GetFloat("SFX Volume", 1));

        ApplyChanges();

        finishedInitialSet = true;
    }

    private void ApplyChanges()
    {
        ApplyBGMVolume();
        ApplySFXVolume();
    }

    public void BGMVolumeSlider(float value)
    {
        if(finishedInitialSet)
        {
            currentBGMVolume = value;
            ApplyBGMVolume();
        }
    }

    public void SFXVolumeSlider(float value)
    {
        if(finishedInitialSet)
        {
            currentSFXVolume = value;
            ApplySFXVolume();
        }
    }

    private void SetBGMVolume(float volume) => bgmSlider.value = volume;

    private void SetSFXVolume(float volume) => sfxSlider.value = volume;

    private void ApplyBGMVolume()
    {
        AudioManager.Instance.ChangeBGMVolume(currentBGMVolume);
        PlayerPrefs.SetFloat("BGM Volume", currentBGMVolume);
    }

    private void ApplySFXVolume()
    {
        AudioManager.Instance.ChangeSFXVolume(currentSFXVolume);
        PlayerPrefs.SetFloat("SFX Volume", currentSFXVolume);
    }
}
