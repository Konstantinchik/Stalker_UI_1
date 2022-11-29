using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ResolutionManager : MonoBehaviour
{
    public AudioMixer Mixer;
    public Slider VolumeSlider;
    public Text VolumeText;
    public Resolution[] ResolutionArray;
    public Dropdown ResolutionDropdown;
    public Text ResolutionText;
    public string[] GraphicsQualityArray;
    public Dropdown GraphicsQualityDropdown;
    public Text GraphicsQualityText;
    public Options OptionsScript;

    void Awake()
    {
        if (ResolutionDropdown != null)
        {
            ResolutionArray = Screen.resolutions.Select(resolution => new Resolution
            {
                width = resolution.width,
                height = resolution.height
            }).Distinct().ToArray();
            ResolutionDropdown.ClearOptions();

            int CurrentResolutionIndex = 0;
            List<string> DropdownOptions = new List<string>();
            for (int i = 0; i < ResolutionArray.Length; i++)
            {
                string Option = ResolutionArray[i].width + " x " + ResolutionArray[i].height;
                DropdownOptions.Add(Option);

                if (ResolutionArray[i].width == Screen.width && ResolutionArray[i].height == Screen.height)
                {
                    CurrentResolutionIndex = i;
                }
            }


            ResolutionDropdown.AddOptions(DropdownOptions);
            ResolutionDropdown.value = CurrentResolutionIndex;
            ResolutionDropdown.RefreshShownValue();
        }

        if (GraphicsQualityDropdown != null)
        {
            for (int i = 0; i < GraphicsQualityArray.Length; i++)
            {
                GraphicsQualityArray[i] = GraphicsQualityDropdown.options[i].text;
            }
        }

        SetDefaultOptions();
        LoadOptions();
        SaveOptions();
    }

    public void SetDefaultOptions()
    {
        OptionsScript.Resolution = 0;
        OptionsScript.Fullscreen = false;
        OptionsScript.Volume = -40f;
        OptionsScript.GraphicsQuality = 0;

        //Save Options to a .json file//
        string DefaultOptionsJSON = JsonUtility.ToJson(OptionsScript);
        File.WriteAllText(Application.dataPath + "/defaultoptions.json", DefaultOptionsJSON);

        if (!File.Exists(Application.dataPath + "/options.json"))
        {
            File.WriteAllText(Application.dataPath + "/options.json", DefaultOptionsJSON);
        }
    }

    public void LoadOptions()
    {
        //Load Options from a .json file//
        if (File.Exists(Application.dataPath + "/options.json"))
        {
            string OptionsJSON = File.ReadAllText(Application.dataPath + "/options.json");
            JsonUtility.FromJsonOverwrite(OptionsJSON, OptionsScript);

            SetResolution(OptionsScript.Resolution);
            SetFullscreen(OptionsScript.Fullscreen);
            SetVolume(OptionsScript.Volume);
            SetGraphicsQuality(OptionsScript.GraphicsQuality);
        }
        else
        {
            ResetAllOptions();
        }
    }

    public void SaveOptions()
    {
        //Save Options to a .json file//
        string OptionsJSON = JsonUtility.ToJson(OptionsScript);
        File.WriteAllText(Application.dataPath + "/options.json", OptionsJSON);
    }

    public void ResetAllOptions()
    {
        //Load Options from a .json file//
        if (File.Exists(Application.dataPath + "/defaultoptions.json"))
        {
            string DefaultOptionsJSON = File.ReadAllText(Application.dataPath + "/defaultoptions.json");
            JsonUtility.FromJsonOverwrite(DefaultOptionsJSON, OptionsScript);

            SetResolution(OptionsScript.Resolution);
            SetFullscreen(OptionsScript.Fullscreen);
            SetVolume(OptionsScript.Volume);
            SetGraphicsQuality(OptionsScript.GraphicsQuality);
        }
        else
        {
            SetDefaultOptions();
        }

        SaveOptions();
    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution AppliedResolution = ResolutionArray[ResolutionIndex];
        Screen.SetResolution(AppliedResolution.width, AppliedResolution.height, Screen.fullScreen);

        if (ResolutionText != null)
        {
            ResolutionText.text = "Resolution: " + ResolutionArray[ResolutionIndex].width + " x " + ResolutionArray[ResolutionIndex].height;
        }

        OptionsScript.Resolution = ResolutionIndex;
        SaveOptions();
    }

    public void SetFullscreen(bool Fullscreen)
    {
        Screen.fullScreen = Fullscreen;
        OptionsScript.Fullscreen = Fullscreen;
        SaveOptions();
    }

    public void SetVolume(float Volume)
    {
        Mixer.SetFloat("MasterVolume", Volume);

        if (VolumeSlider != null)
        {
            if (VolumeText != null)
            {
                VolumeText.text = "Volume: " + VolumeSlider.normalizedValue * 100f + "%";
            }
        }

        OptionsScript.Volume = Volume;
        SaveOptions();
    }

    public void SetGraphicsQuality(int Quality)
    {
        QualitySettings.SetQualityLevel(Quality);

        if (GraphicsQualityText != null)
        {
            GraphicsQualityText.text = "Graphics quality: " + GraphicsQualityArray[Quality];
        }

        OptionsScript.GraphicsQuality = Quality;
        SaveOptions();
    }

    
}
