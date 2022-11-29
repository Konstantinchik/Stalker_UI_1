using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class JsonController : MonoBehaviour
{
    #region STATIC INSTANCE
    public static JsonController Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public VideoOptions videoOptions;
    public AdvancedOptions advancedOptions;
    public SoundOptions soundOptions;
    public GameOptions gameOptions;
    public KeyboardOptions keyboardOptions;

    public SaveGameDataItem item;

    #region GAMEDATA save/load
    //[ContextMenu("Load")]
    public SaveGameDataItem LoadFile(string saveName)
    {
        //item = JsonUtility.FromJson<SaveGameDataItem>(File.ReadAllText(Application.streamingAssetsPath + "/" + saveName ));
        item = JsonUtility.FromJson<SaveGameDataItem>(File.ReadAllText(saveName));
        return item;
    }

   
    public void SaveFile(string saveName)
    {
        File.WriteAllText(Application.streamingAssetsPath + "/SaveGames/" + saveName + ".json", JsonUtility.ToJson(item));
    }

    public void SaveFile(string saveName, SaveGameDataItem m_item)
    {
        File.WriteAllText(Application.streamingAssetsPath + "/SaveGames/" + saveName + ".json", JsonUtility.ToJson(m_item));
    }

    public List<string> CheckSaveFilesList()
    {
        List<string> saves = new List<string>();

        var files = Directory.GetFiles(Application.streamingAssetsPath + "/SaveGames/", "*.json"); //, SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {
            saves.Add(files[i]);
            Debug.Log(files[i]);
        }

        return saves;
    }
    #endregion

}

#region OPTIONS SERIALIZABLE
[System.Serializable]
public class VideoOptions
{ 
    /*
    public RenderType renderType;
    public RenderQuality renderQuality;
    public PreferedResolutions preferedResolution;
    */

    public string renderType;
    public string renderQuality;
    public string currentResolution;

    public float gamma;
    public float contrast;
    public float brightness;
    public bool fullscreen;
}

[System.Serializable]
public class AdvancedOptions
{
    public RenderType renderType;
    public RenderQuality renderQuality;
    public PreferedResolutions preferedResolution;

    public float gamma;
    public float contrast;
    public float brightness;
    public bool fullscreen;
}

[System.Serializable]
public class SoundOptions
{
    public float soundVolume;
    public float musicVolume;
    public bool eax;
}

[System.Serializable]
public class GameOptions
{
    //public Difficulty difficulty;
    public int difficulty;
    public bool showcrosschair;
    public bool dynamiccrosschair;
    public bool showweapon;
    public bool aimdistance;
    public bool recognize_npc;
}

[System.Serializable]
public class KeyboardOptions
{
    public float mouseSensitivity;
    public bool invertmouse;

    // Direction
    public string kLeft;
    public string kRight;
    public string kUp;
    public string kDown;

    // Movement
    public string kForward;
    public string kBackward;
    public string kStepLeft;
    public string kStepRight;
    public string kJump;
    public string kSit;
    public string kAllwaysSit;
    public string kStep;
    public string kRun;
    public string kLookLeft;
    public string kLookRight;

    // Weapon
    public string kWeapon1;
    public string kWeapon2;
    public string kWeapon3;
    public string kWeapon4;
    public string kWeapon5;
    public string kWeapon6;
    public string kBulletType;
    public string kNextSlot;
    public string kPreviousSlot;
    public string kFire;
    public string kOptic;
    public string kReload;
    public string kUnderbarell;
    public string kFiremodNext;
    public string kFiremodPrevious;

    // Inventory
    public string kInventory;
    public string kActiveTasks;
    public string kMap;
    public string kContacts;
    public string kLight;
    public string kNightvision;
    public string kBandage;
    public string kFirstAid;
    public string kOffload;
    public string kDetector;

    // Common
    public string kPause;
    public string kUse;
    public string kQuickSave;
    public string kQuickLoad;

}
#endregion


[System.Serializable]
public class SaveGameDataItem
{
    public int id;
    public string save_name;
    public string save_datetime;

    /*
    public string Level;
    public string Levelname;
    public string GameTime;

    public int Health;
    public int Radiation;
    public int[] Artefact;
    */
}