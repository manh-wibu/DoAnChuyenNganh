using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeControl : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Start()
    {
        // Âm lượng lưu trữ
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        musicSource.volume = savedMusic;
        sfxSource.volume = savedSFX;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
