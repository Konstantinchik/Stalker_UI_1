
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;

/*
tooltTipText = "new tooltip \n second row";
Canvas.ForceUpdateCanvases(); 
tooltTipText.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false; 
tooltTipText.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
 */

public class SaveGameMenu : MonoBehaviour
{
    [SerializeField] GameObject saveGameMenu;
    [SerializeField] GameObject confirmDelete;

    [Space]
    [SerializeField] InputField inputSaveNameText;

    [SerializeField] RectTransform content;
    [SerializeField] RectTransform savedGameItemPrefab;

    List<SaveGameDataItem> savedGames;
    SaveGameDataItem savedGameItem;

    [Space]
    [SerializeField] ToggleGroup m_ToggleGroup;
    [SerializeField] List<Toggle> m_toggleItems;
    List<GameObject> items;

    public Toggle currentSelection
    {
        get { return m_ToggleGroup.ActiveToggles().FirstOrDefault(); }
    }

    private void Awake()
    {
        savedGameItem = new SaveGameDataItem();
        savedGames = new List<SaveGameDataItem>();

        m_toggleItems = new List<Toggle>();
        items = new List<GameObject>();
    }

    void Start()
    {
        savedGames = GameManager.Instance.savedGames;

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        int max_ID = GameManager.Instance.max_ID;

        for (int currentIndex = 0; currentIndex < max_ID; currentIndex++)
        {
            var instance = Instantiate(savedGameItemPrefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            items.Add(instance);
            m_toggleItems.Add(instance.GetComponent<Toggle>());
            instance.GetComponent<Toggle>().GetComponentInChildren<Text>().text = savedGames[currentIndex].save_name;
            instance.transform.GetChild(2).GetComponent<Text>().text = "[" + savedGames[currentIndex].save_datetime + "]";
        }
    }


    public void AddItem()
    {
        if (string.IsNullOrEmpty(inputSaveNameText.text) || string.IsNullOrWhiteSpace(inputSaveNameText.text))
        {
            return;
        }
        else
        {
            Save();
        }
    }

    public void RemoveItem()
    {
        // TODO проверить его id

        int saveNum = content.childCount;

        for (int i = 0; i < saveNum; i++)
        {
            if (m_toggleItems[i].isOn)
            {
                string path = Application.streamingAssetsPath + "/" + savedGames[i].save_name + ".json";
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                {
                    File.Delete(path);
                }
                Destroy(items[i]);
            }
        }
        confirmDelete.SetActive(false);
    }


    public void Save()
    {
        var instance = Instantiate(savedGameItemPrefab.gameObject) as GameObject;
        instance.transform.SetParent(content, false);
        string str = inputSaveNameText.text.Trim();
        instance.GetComponent<Toggle>().GetComponentInChildren<Text>().text = str;
        instance.transform.GetChild(2).GetComponent<Text>().text = "[" + "01/01/2021 01:50" + "]";      // потом будем считывать из конфига

        //
        //  ЗАПИСЬ ДАННЫХ ДЛЯ СОХРАНЕНИЯ
        //

        // нужен алгоритм вычисления max_ID
        savedGameItem.id = GameManager.Instance.max_ID + 1;     // надо доработать
        savedGameItem.save_name = str;
        savedGameItem.save_datetime = "01/01/2021 01:50";       // String.Format("{0:dd/MM/yyyy hh:mm}", dt);

        GameManager.Instance.savedGames.Add(savedGameItem);
        GameManager.Instance.max_ID++;                          // надо доработать
        JsonController.Instance.SaveFile(str, savedGameItem);

        inputSaveNameText.text = string.Empty;
    }

    public void Delete()
    {
        int saveNum = content.childCount;

        for (int i = 0; i < saveNum; i++)
        {
            if (m_toggleItems[i].isOn)
            {
                confirmDelete.SetActive(true);
            }
        }
    }

    public void Cancel()
    {
        saveGameMenu.SetActive(false);
    }

    public void CancelDelete()
    {
        confirmDelete.SetActive(false);
    }


}
