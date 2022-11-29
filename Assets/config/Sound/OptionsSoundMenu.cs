using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsSoundMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsSoundMenu;

    [SerializeField] AudioMixer soundMixer;
    [SerializeField] AudioMixer musicMixer;
    [Space]
    [SerializeField] Slider SoundSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] GameObject EaxCheckbox;

    public float m_soundVolume;
    public float m_musicVolume;

    private float m_soundVolumePrev;
    private float m_musicVolumePrev;
    private bool eax_prev;

    CheckBox checkBox;

    bool justopen;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        justopen = true;
        SetControls();
    }

    private void SetControls()
    {
        m_soundVolume = GameManager.Instance.SoundVolume;
        m_musicVolume = GameManager.Instance.MusicVolume;

        checkBox = EaxCheckbox.GetComponent<CheckBox>();

        if (GameManager.Instance.Eax)
        {
            checkBox.SetChecked();
        }
        else
        {
            checkBox.SetUnchecked();
        }

        eax_prev = checkBox.IsChecked;
        m_soundVolumePrev = m_soundVolume;  //SoundSlider.value;
        m_musicVolumePrev = m_musicVolume;  //MusicSlider.value;

        SoundSlider.value = m_soundVolume;
        MusicSlider.value = m_musicVolume;

        justopen = false;
    }

    public void SetSoundVolume(float sliderValue)
    {
        this.soundMixer.SetFloat("SoundVolume", Mathf.Log10(sliderValue) * 20);
        GameManager.Instance.SoundVolume = sliderValue;
    }

    public void SetMusicVolume(float sliderValue)
    {
        this.musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        GameManager.Instance.MusicVolume = sliderValue;
    }


    public void SoundApply()
    {

        GameManager.Instance.isSoundSaved = true;

        Debug.Log("SoundApply currentSoundVolume " + m_soundVolume.ToString());
        Debug.Log("SoundApply currentMusicVolume " + m_musicVolume.ToString());
        Debug.Log("SoundApply currentSoundVolume " + SoundSlider.value.ToString());
        Debug.Log("SoundApply currentMusicVolume " + MusicSlider.value.ToString());

        GameManager.Instance.SoundVolume = m_soundVolume;
        GameManager.Instance.MusicVolume = m_musicVolume;
        GameManager.Instance.Eax = checkBox.IsChecked;

        SoundOptions so = new SoundOptions();

        so.soundVolume = m_soundVolume;
        so.musicVolume = m_musicVolume;
        so.eax = checkBox.IsChecked;

        SaveManager.SoundSave(so);

        optionsSoundMenu.SetActive(false);
    }

    public void SoundCancel()
    {
        // OptionsSoundMenu
        GameManager.Instance.SoundVolume = m_soundVolumePrev;
        GameManager.Instance.MusicVolume = m_musicVolumePrev;
        GameManager.Instance.Eax = eax_prev;

        SetSoundVolume(m_soundVolumePrev);
        SetMusicVolume(m_musicVolumePrev);

        optionsSoundMenu.SetActive(false);
    }

    private void Update()
    {
        if (justopen)
            SetControls();
    }
}
