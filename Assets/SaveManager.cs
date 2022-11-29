using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static string directory = "/Options/";
    public static string videofileName = "video.json";
    public static string soundfileName = "sound.json";
    public static string gamefileName = "game.json";
    public static string keyboardfileName = "keyboard.json";

    #region VIDEO
    public static void VideoSave(VideoOptions vo)
    {
        //string dir = Application.streamingAssetsPath + directory;
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(vo);
        File.WriteAllText(dir + videofileName, json);
    }

    public static VideoOptions VideoLoad()
    {
        string fullPath = Application.persistentDataPath + directory + videofileName;
        VideoOptions vo = new VideoOptions();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            vo = JsonUtility.FromJson<VideoOptions>(json);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }

        return vo;
    }
    #endregion

    #region SOUND
    public static void SoundSave(SoundOptions so)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(so);
        File.WriteAllText(dir + soundfileName, json);
    }

    public static SoundOptions SoundLoad()
    {
        string fullPath = Application.persistentDataPath + directory + soundfileName;
        SoundOptions so = new SoundOptions();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            so = JsonUtility.FromJson<SoundOptions>(json);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }

        return so;
    }
    #endregion

    #region GAME
    public static void GameSave(GameOptions go)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(go);
        File.WriteAllText(dir + gamefileName, json);
    }

    public static GameOptions GameLoad()
    {
        string fullPath = Application.persistentDataPath + directory + gamefileName;
        GameOptions go = new GameOptions();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            go = JsonUtility.FromJson<GameOptions>(json);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }

        return go;
    }
    #endregion

    #region KEYBOARD
    public static void KeyboardSave(KeyboardOptions ko)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(ko);
        File.WriteAllText(dir + keyboardfileName, json);
    }

    public static KeyboardOptions KeyboardLoad()
    {
        string fullPath = Application.streamingAssetsPath + directory + keyboardfileName;
        KeyboardOptions ko = new KeyboardOptions();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            ko = JsonUtility.FromJson<KeyboardOptions>(json);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }

        return ko;
    }
    #endregion

}
