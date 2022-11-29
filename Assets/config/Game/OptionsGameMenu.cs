using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsGameMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsGameMenu;

    [SerializeField] Dropdown DifficultyDrop;
    [SerializeField] GameObject ShowCrosschairCheckbox;
    [SerializeField] GameObject DynamicCrosschairCheckbox;
    [SerializeField] GameObject ShowWeaponCheckbox;
    [SerializeField] GameObject AimDistanceCheckbox;
    [SerializeField] GameObject NpcRecognitionCheckbox;

    CheckBox showCrosschair;
    CheckBox dynamicCrosschair;
    CheckBox showWeapon;
    CheckBox aimDistance;
    CheckBox npcRecognition;

    bool justopen;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        showCrosschair = ShowCrosschairCheckbox.GetComponent<CheckBox>();
        dynamicCrosschair = DynamicCrosschairCheckbox.GetComponent<CheckBox>();
        showWeapon = ShowWeaponCheckbox.GetComponent<CheckBox>();
        aimDistance = AimDistanceCheckbox.GetComponent<CheckBox>();
        npcRecognition = NpcRecognitionCheckbox.GetComponent<CheckBox>();

        justopen = true;
    }

    private void OnDisable()
    {
        justopen = false;
    }

    void SetControls()
    {
        DifficultyDrop.value = (int)GameManager.Instance.difficulty;

        showCrosschair.IsChecked = GameManager.Instance.ShowCrosschair;
        dynamicCrosschair.IsChecked = GameManager.Instance.DynamicCrosschair;
        showWeapon.IsChecked = GameManager.Instance.ShowWeapon;
        aimDistance.IsChecked = GameManager.Instance.AimDistance;
        npcRecognition.IsChecked = GameManager.Instance.NpcRecognition;

        justopen = false;
    }

    void Update()
    {
        if (justopen)
            SetControls();
    }

    public void GameApply()
    {
        // Apply changes
        GameManager.Instance.difficulty = (Difficulty)DifficultyDrop.value;

        GameManager.Instance.ShowCrosschair = showCrosschair.IsChecked;
        GameManager.Instance.DynamicCrosschair = dynamicCrosschair.IsChecked;
        GameManager.Instance.ShowWeapon = showWeapon.IsChecked;
        GameManager.Instance.AimDistance = aimDistance.IsChecked;
        GameManager.Instance.NpcRecognition = npcRecognition.IsChecked;

        GameOptions go = new GameOptions();

        go.difficulty = (int)GameManager.Instance.difficulty;
        go.showcrosschair = showCrosschair.IsChecked;
        go.dynamiccrosschair = dynamicCrosschair.IsChecked;
        go.showweapon = showWeapon.IsChecked;
        go.aimdistance = aimDistance.IsChecked;
        go.recognize_npc = npcRecognition.IsChecked;

        if (go != null)
            SaveManager.GameSave(go);

        optionsGameMenu.SetActive(false);
    }

    public void GameCancel()
    {
        optionsGameMenu.SetActive(false);
    }
}
