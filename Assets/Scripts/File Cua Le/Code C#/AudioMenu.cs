using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioMenu : MonoBehaviour
{
    [Header("-------Audio Source------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-------Audio Clip--------")]
    public AudioClip musicMenu;
    public AudioClip buttonClick;
    private void Start()
    {
        MusicMenu();
    }

    public void MusicMenu()
    {
        musicSource.clip = musicMenu;
        musicSource.Play();
    }

    public void PlayClick()
    {
        SFXSource.PlayOneShot(buttonClick);
    }
}