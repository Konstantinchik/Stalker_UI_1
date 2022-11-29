using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    
    [HideInInspector]
    public static SoundManager Instance;

    [SerializeField] AudioMixer soundMixer;
    [SerializeField] AudioMixer musicMixer;

    private void Awake()
    {
        Instance = this;
    }

    
    public void SetSoundVolume(float sliderValue)
    {
        this.soundMixer.SetFloat("SoundVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        this.musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
    

}

