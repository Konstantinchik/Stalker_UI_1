
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;

public class LoadGameMenu : MonoBehaviour
{
    [SerializeField] GameObject loadGameMenu;
    [SerializeField] GameObject confirmDelete;
    [SerializeField] GameObject confirmLoad;       // TODO

    [Space]
    [SerializeField] RectTransform content;
    [SerializeField] RectTransform savedGameItemPrefab;

    List<SaveGameDataItem> savedGames;
    SaveGameDataItem savedGameItem;             // данные со структурой сохранки
    string path;                                // используетс для confirmLoad

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


    public void RemoveItem()
    {
        // TODO проверить его id

        int saveNum = content.childCount;

        for (int i = 0; i < saveNum; i++)
        {
            if (m_toggleItems[i].isOn)
            {
                string path = Application.streamingAssetsPath + "/SaveGames/" + savedGames[i].save_name + ".json";
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



    public void Load()
    {
        bool saved = false;     // TODO
        
        int saveNum = content.childCount;
        string saveName = string.Empty;

        for (int i = 0; i < saveNum; i++)
        {
            if (m_toggleItems[i].isOn)
            {
                saveName =  savedGames[i].save_name + ".json";
            }
        }

        Debug.Log(saveName + "= savedGames[i].save_name");

        //if (string.IsNullOrEmpty(saveName))
        //    return;

        string path = Application.streamingAssetsPath + "/SaveGames/" + saveName;
        Debug.Log(path + " path");

        FileInfo fileInf = new FileInfo(path);
        if (fileInf.Exists)
        {
            // добавить проверку, если текущая игра не сохранена запросить подтверждение - несохраненные данные
            if (!saved)
            {
                confirmLoad.SetActive(true);
                return;
            }
            savedGameItem = JsonController.Instance.LoadFile(path);
            Debug.Log("Загрузка SAVED");
        }
        
        loadGameMenu.SetActive(false);

        //
        GameManager.Instance.StartLoadGame(savedGameItem);
    }

    public void LoadGameData()
    {
        savedGameItem = JsonController.Instance.LoadFile(path);
        confirmLoad.SetActive(false);
        loadGameMenu.SetActive(false);
        Debug.Log("Загрузка NOT SAVED");

        //
    }

    public void CancelLoad()
    {
        confirmLoad.SetActive(false);
    }

    public void Cancel()
    {
        loadGameMenu.SetActive(false);
    }

    public void CancelDelete()
    {
        confirmDelete.SetActive(false);
    }

}
