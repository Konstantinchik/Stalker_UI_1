using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

/*
  Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
  Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
  Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
  Screen.fullScreenMode = FullScreenMode.Windowed;
*/

public class OptionsVideoMenu : MonoBehaviour
{
    [SerializeField] GameObject OptionsMenu;

    [SerializeField] Dropdown RenderTypeDrop;
    [SerializeField] Dropdown QualityDrop;
    [SerializeField] Dropdown ResolutionDrop;
    [SerializeField] Slider GammaSlider;
    [SerializeField] Slider ContrastSlider;
    [SerializeField] Slider BrightnessSlider;
    [SerializeField] GameObject FullscreenCheckbox;

    Resolution currentResolution;

    CheckBox checkBox;

    bool justopen;

    private void OnEnable()
    {
        justopen = true;

        // returns a list of the text properties of the options
        //var listAvailableStrings = RenderTypeDrop.options.Select(option => option.text).ToList();

        // returns the index of the given string
        //RenderTypeDrop.value = listAvailableStrings.IndexOf(GameManager.Instance.currentResolution.ToString());
    }

    private void OnDisable()
    {
        justopen = false;
    }

    void Start()
    {
        checkBox = FullscreenCheckbox.GetComponent<CheckBox>();

        ResolutionDrop.onValueChanged.AddListener(delegate { DropdownItemSelected(ResolutionDrop); });
    }

    // вызывается когда значение выпадающего списка разрешения изменилось
    public void DropdownItemSelected(Dropdown resolutionDrop)
    {
        // здесь мы получаем текущее значение списка
        int index = resolutionDrop.value;

        // из него получаем значение индекса из доступоно списка видеорежимов
        //int realIndex = usedResolutionIndex[index];

        currentResolution = GameManager.Instance.resolutions[index];


        Screen.SetResolution(currentResolution.width, currentResolution.height, checkBox.IsChecked);
    }

    void SetControls()
    {
        RenderTypeDrop.value = GameManager.Instance.currentRenderTypeIndex;
        QualityDrop.value = GameManager.Instance.currentRenderQualityIndex;
        ResolutionDrop.value = GameManager.Instance.currentResolutionIndex;

        GammaSlider.value = GameManager.Instance.gamma;
        ContrastSlider.value = GameManager.Instance.contrast;
        BrightnessSlider.value = GameManager.Instance.brightness;

        if (GameManager.Instance.isFullscreen)
        {
            checkBox.SetChecked();
        }
        else
        {
            checkBox.SetUnchecked();
        }

        justopen = false;
    }

    void Update()
    {
        if (justopen)
            SetControls();


    }

    public void VideoApply()
    {
        // Нажимаем "Применить" 
        GameManager.Instance.DropdownItemSelected(ResolutionDrop);
        
        GameManager.Instance.currentRenderType = (RenderType)(RenderTypeDrop.value);
        GameManager.Instance.currentRenderQuality = (RenderQuality)(QualityDrop.value);
        GameManager.Instance.currentResolution = GameManager.Instance.availableResolutions[ResolutionDrop.value];

        GameManager.Instance.gamma = GammaSlider.value;
        GameManager.Instance.contrast = ContrastSlider.value;
        GameManager.Instance.brightness = BrightnessSlider.value;

        VideoOptions vo = new VideoOptions();

        vo.renderType = RenderTypeDrop.value.ToString();            //RenderType.MIXED.ToString();
        vo.renderQuality = QualityDrop.value.ToString();            //RenderQuality.MIDDLE.ToString();
        vo.currentResolution = Screen.currentResolution.ToString();

        vo.gamma = GammaSlider.value;
        vo.contrast = ContrastSlider.value;
        vo.brightness = BrightnessSlider.value;
        vo.fullscreen = checkBox.IsChecked;
        if (checkBox.IsChecked)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;


        if (vo != null)
            SaveManager.VideoSave(vo);

        OptionsMenu.SetActive(false);
    }

    public void VideoCancel()
    {
        OptionsMenu.SetActive(false);
    }
}
