using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------Audio Source------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-------Audio Clip--------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip Jump;
    public AudioClip NextLevel;
    public AudioClip swordSlash;
    public AudioClip buttonClick;
    private void Start()
    {
        PlayBackGroundMusic();
    }

    public void PlayBackGroundMusic()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void DeathSource()
    {
        SFXSource.PlayOneShot(death);
    }

    public void CheckPointSource()
    {
        SFXSource.PlayOneShot(checkpoint);
    }
    public void PlayJumpSound()
    {
        SFXSource.PlayOneShot(Jump);
    }

    public void PlayNextLevelSound()
    {
        SFXSource.PlayOneShot(NextLevel);
    }

    public void SwordSlash()
    {
        SFXSource.PlayOneShot(swordSlash);
    }

    public void PlayClick()
    {
        SFXSource.PlayOneShot(buttonClick);
    }
}