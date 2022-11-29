
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region FIELDS / ACCESSORS

    [SerializeField] GameObject Intro;
    [SerializeField] GameObject MenuMusic;

    //[Header("Game Data")]


    #region SAVE GAME
    // 
    public List<SaveGameDataItem> savedGames;
    public SaveGameDataItem savedGameItem;

    public List<string> saved_files;    // сейчас это полный путь
    public List<string> savedNames;     // имя_сейва [дата время]

    int num_saves;                      // количество сохранений
    public int max_ID;                  // 
    public List<int> IDs;               // при загрузке сохраняем список id всех сохранений, для последующей сортировки

    #endregion


    #region DISPLAY RESOLUTIONS
    [Header("Display Resolutions")]
    public Resolution[] resolutions;
    public List<Resolution> availableResolutions;
    PreferedResolutions preferedResolutions;
    #endregion

    [Space]
    public CANVAS_TYPE canvasType;

    private bool isPaused;
    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
        set
        {
            isPaused = value;
            Time.timeScale = (isPaused ? 0 : 1);
            // Add stop any animation
        }
    }
    private bool isSaved;
    public bool IsSaved
    {
        get => isSaved;
        set => isSaved = value;
    }
    public GAME_STATE GameState;
    public GAME_MODE GameMode;

    [Header("Keyboard Bind")]
    public bool KeyBind;
    public string Keypressed;
    #endregion

    #region DECLARE

    #region DECLARE MENU CANVASES
    [Header("Menu Canvases")]
    [SerializeField] GameObject MenuCanvas;
    [SerializeField] GameObject IntroCanvas;
    [SerializeField] GameObject MainMenu_new;
    [SerializeField] GameObject MainMenu_resume;
    [SerializeField] GameObject LoadLevelScreen;
    [Space]
    [SerializeField] GameObject LoadGameMenu;
    [SerializeField] GameObject SaveGameMenu;
    [Space]
    [SerializeField] GameObject CreditsScreen;
    [SerializeField] GameObject ConfirmDelete;
    [SerializeField] GameObject ConfirmLoad;
    [SerializeField] GameObject ChangeAfterReload;
    [SerializeField] GameObject QuitWindowsDialog;
    [SerializeField] GameObject LeaveGameDialog;
    [SerializeField] GameObject SaveAttentionLostDataDialog;
    [Space]
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] GameObject OptionsEnchMenu;
    [SerializeField] GameObject OptionsSoundMenu;
    [SerializeField] GameObject OptionsGameMenu;
    [SerializeField] GameObject OptionsKeyboardMenu;
    [Space]
    [SerializeField] GameObject HudCanvas;
    [SerializeField] GameObject DialogCanvas;
    [Space]
    [SerializeField] InventoryController inventoryController;
    [SerializeField] GameObject LootCanvas;
    [SerializeField] GameObject TradeCanvas;
    [SerializeField] GameObject PlayerCanvas;
    #endregion


    
    #region DECLARE VIDEO HANDLES
    // All declarations in - OptionsVideoMenu
    // all Callbacks - from there

    [Header("Video")]
    [SerializeField] Dropdown RenderTypeDrop;
    [SerializeField] Dropdown QualityDrop;
    [SerializeField] Dropdown ResolutionDrop;
    [SerializeField] Slider GammaSlider;
    [SerializeField] Slider ContrastSlider;
    [SerializeField] Slider BrightnessSlider;
    [SerializeField] GameObject FullscreenCheckbox;

    #endregion

    #region DECLARE GAME HANDLES
    [Header("Game")]
    [SerializeField] Dropdown DifficultyDrop;
    [SerializeField] GameObject ShowCrosschairCheckbox;
    [SerializeField] GameObject DynamicCrosschairCheckbox;
    [SerializeField] GameObject ShowWeaponCheckbox;
    [SerializeField] GameObject AimDistanceCheckbox;
    [SerializeField] GameObject NpcRecognitionCheckbox;
    #endregion

    #region DECLARE KEYBOARD HANDLES
    [Header("Keyboard")]
    [SerializeField] Slider MouseSensitivitySlider;
    [SerializeField] GameObject InverseMouse;
    [SerializeField] GameObject KeyboardBind;
    /// <summary>
    /// Сделать привязку кнопок
    /// </summary>
    #endregion
        

    #region OPTIONS SETTINGS

    #region VIDEO
    [Header("Video Settings")]

    VideoOptions vo;

    // Get all possible resolutions index
    private List<int> totalResolutionIndex;
    // Compare prefered list to total
    private List<int> usedResolutionIndex;
    public List<Resolution> usedResolutions;

    public RenderType currentRenderType;
    public int currentRenderTypeIndex;
    public string currentRenderTypeText;

    public RenderQuality currentRenderQuality;
    public int currentRenderQualityIndex;
    public string currentRenderQualityText;

    public Resolution currentResolution;
    public int currentResolutionIndex;
    public string currentResolutionText;

    [Range(0, 1)] public float gamma;
    [Range(0, 1)] public float contrast;
    [Range(0, 1)] public float brightness;
    public bool isFullscreen;

    #endregion

    #region SOUND

    SoundOptions so;

    public bool isSoundSaved;

    [Header("Sound Settings")]
    [Range(0, 1)] public float SoundVolume;

    [Range(0, 1)] public float MusicVolume;
    public bool Eax;
    #endregion

    #region GAME
    [Header("Game Settings")]

    GameOptions go;

    public Difficulty difficulty;
    public bool ShowCrosschair;
    public bool DynamicCrosschair;
    public bool ShowWeapon;
    public bool AimDistance;
    public bool NpcRecognition;

    #endregion

    #region KEYBOARD

    KeyboardOptions ko;

    [Header("Keyboard Settings")]
    public bool isInverseMouse;

    [Range(0, 1)]
    public float mouseSensitivity;

    public Dictionary<string, KeyCode> DefaultActionKey;
    public Dictionary<string, KeyCode> ActionKey;
    #endregion

    #endregion

    #endregion DECLARE

    // PlayerPrefs.DeleteAll(); - HERE !!!!!!!!!!!!!!
    public void Awake()
    {
        Instance = this;

        IsPaused = true;

        //PlayerPrefs.DeleteAll(); // !!! Be careful my yuang friend !!!

        GameStateInit();

        CheckResolutions();

        PlayMenuMusic(true);

        StartLoading();

    }

    private void GameStateInit()
    {
        #region GAME STATE INIT
        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            Screen.fullScreen = true;
            isFullscreen = true;
            PlayerPrefs.SetString("Fullscreen", "fullscreen");
        }
        else
        {
            if (PlayerPrefs.GetString("Fullstring") == "fullscreen")
            {
                Screen.fullScreen = true;
                isFullscreen = true;
            }
            if (PlayerPrefs.GetString("Fullstring") == "windowed")
            {
                Screen.fullScreen = false;
                isFullscreen = false;
            }
        }

        GameState = GAME_STATE.init;
        GameMode = GAME_MODE.main_menu;
        canvasType = CANVAS_TYPE.Default;
        #endregion
    }

    private void CheckResolutions()
    {
        Resolution cres = Screen.currentResolution;
        currentResolutionText = cres.ToString();
        Screen.SetResolution(cres.width, cres.height, isFullscreen);

        resolutions = Screen.resolutions;
        availableResolutions = new List<Resolution>();
        usedResolutions = new List<Resolution>();
        totalResolutionIndex = new List<int>();
        usedResolutionIndex = new List<int>();

        // преобразуем enum в массив
        var resArray = typeof(PreferedResolutions).GetFields().Where(fi => fi.IsLiteral);
        string[] fieldNames = resArray.Select(fi => fi.Name).ToArray();
        PreferedResolutions[] fieldsValue = resArray.Select(fi => fi.GetRawConstantValue()).Cast<PreferedResolutions>().ToArray();

        // индекс который будет использоваться для переборки отсеянного списка разрешений
        int usedIndex = 0;
        foreach (var res in resolutions)
        {
            // заполняем список всеми режимами
            totalResolutionIndex.Add(usedIndex);
            usedIndex++;

            for (int i = 0; i < fieldsValue.Length; i++)
            {
                string curResol = "r" + res.width + "x" + res.height;
                if (curResol.Equals(fieldsValue[i].ToString()))
                {
                    availableResolutions.Add(res);
                    usedResolutions.Add(res);
                    usedResolutionIndex.Add(usedIndex);

                    if (cres.Equals(res))
                        currentResolutionIndex = i;
                    //Debug.Log(res.ToString());
                }
            }
        }
        // Получили список валидных разрешений
        // теперь сохраним их в DropDown в VideoSettings
    }

    private void CheckSavedGames()
    {
        savedGameItem = new SaveGameDataItem();

        savedNames = new List<string>();
        savedGames = new List<SaveGameDataItem>();


        // Создали 1 демо сохранения 
        #region DEMO SAVE  -  all.sav
        /*
        if (!PlayerPrefs.HasKey("save0"))
        {
            save_name = "all";
            PlayerPrefs.SetString("save0", save_name);
            string namePadRight = save_name.PadRight(70);
            DateTime dt = Convert.ToDateTime("01/01/2022 01:30");

            savedNames.Add(String.Format("{0:70} [{1:dd/MM/yyyy hh:mm}]", namePadRight, dt));

            savedGameItem.ID = 0;
            savedGameItem.save_name = save_name;
            savedGameItem.save_datetime = String.Format("{0:dd/MM/yyyy hh:mm}", dt);

            // добавили ID сейва в список количества номеров сохранок List<int>
            savedNum.Add(savedGameItem.ID);

            // сохранили парамерты игры и добавили в список 
            savedGames.Add(savedGameItem);

            // сохранили количество сохраненных игр
            PlayerPrefs.SetInt("num_savedGames", savedNum.Count);
            
        }
        */
        #endregion


        // определяем максимальное ID
        max_ID = GetMaxSaveID();

        for (int i = 0; i < max_ID; i++)
        {
            savedGameItem = JsonController.Instance.LoadFile(saved_files[i]);

            savedGames.Add(savedGameItem);
        }

    }

    private int GetMaxSaveID()
    {
        // список с именами сохраненных файлов
        saved_files = JsonController.Instance.CheckSaveFilesList();

        SaveGameDataItem[] items = new SaveGameDataItem[saved_files.Count];
        int[] id = new int[saved_files.Count];

        for (int i = 0; i < saved_files.Count; i++)
        {
            items[i] = JsonController.Instance.LoadFile(saved_files[i]);
            id[i] = items[i].id;
        }
        Debug.Log(id.Max());
        return id.Max();
    }

    public void StartLoadGame(SaveGameDataItem savedGameItem)
    {
        LoadLevelScreen.SetActive(true);
        MenuMusic.SetActive(false);
        IsPaused = true;

        LoadLevelScreen.SetActive(false);
        HudCanvas.SetActive(true);
        IsPaused = false;
        GameState = GAME_STATE.in_progress;
        GameMode = GAME_MODE.game;
    }

    private void StartLoading()
    {
        #region SET INITIAL CANVAS
        MenuCanvas.SetActive(true);
        MainMenu_new.SetActive(true);
        MainMenu_resume.SetActive(false);

        HudCanvas.SetActive(false);
        DialogCanvas.SetActive(false);

        LootCanvas.SetActive(false);
        TradeCanvas.SetActive(false);
        PlayerCanvas.SetActive(false);
        #endregion


    }

    public void PlayMenuMusic(bool b)
    {
        MenuMusic.SetActive(b);
    }

    private void LoadOptionsSettings()
    {
        LoadVideoSettings();
        LoadSoundSettings();
        LoadGameSettings();
        LoadKeyboardSettings();
    }

    private void LoadVideoSettings()
    {
        // Получили список валидных разрешений в CheckResolutions
        // теперь сохраним их в DropDown в VideoSettings
        //ResolutionDrop.options.Clear();
        List<string> items = new List<string>();

        // преобразуем объект Resolution в string
        foreach (var res in availableResolutions)
        {
            items.Add(res.ToString());
        }

        // fill Dropdown with items
        foreach (var item in items)
        {
            ResolutionDrop.options.Add(new Dropdown.OptionData() { text = item });
        }

        // вызываем событе выбора элемента спика
        // DropdownItemSelected(ResolutionDrop); - меняем после нажати OK
        ResolutionDrop.onValueChanged.AddListener(delegate { DropdownItemSelected(ResolutionDrop); });


        vo = new VideoOptions();

        #region VIDEO JSON INIT
        /*
        vo.renderType = RenderType.MIXED.ToString();
        vo.renderQuality = RenderQuality.MIDDLE.ToString();
        vo.currentResolution = Screen.currentResolution.ToString();

        vo.gamma = 0.5f;
        vo.contrast = 0.5f;
        vo.brightness = 0.5f;
        vo.fullscreen = true;

        if (vo != null)
            SaveManager.VideoSave(vo);
        */
        #endregion

        vo = SaveManager.VideoLoad();

        currentRenderType = RenderType.MIXED;
        currentRenderTypeIndex = (int)(currentRenderType);

        currentRenderQuality = RenderQuality.MAX;
        currentRenderQualityIndex = (int)(currentRenderQuality);

        string loadresolution = vo.currentResolution;
        string[] elements = loadresolution.Split(' ');
        int width = int.Parse(elements[0]);
        int height = int.Parse(elements[2]);

        Screen.SetResolution(width, height, isFullscreen);
        currentResolution = Screen.currentResolution;
        // сравниваем текушее разрешение со списком используемых разрешений.
        // индекс совпавшего сохраняем
        for (int i = 0; i < usedResolutions.Count; i++)
        {
            if (currentResolution.Equals(usedResolutions[i]))
                currentResolutionIndex = i;
        }


        gamma = vo.gamma;
        contrast = vo.contrast;
        brightness = vo.brightness;
        isFullscreen = vo.fullscreen;
    }

    // вызывается когда значение выпадающего списка разрешения изменилось
    public void DropdownItemSelected(Dropdown resolutionDrop)
    {
        // здесь мы получаем текущее значение списка
        int index = resolutionDrop.value;

        // из него получаем значение индекса из доступоно списка видеорежимов
        int realIndex = usedResolutionIndex[index];

        currentResolution = resolutions[realIndex];

        Screen.SetResolution(currentResolution.width, currentResolution.height, isFullscreen);
    }

    private void LoadSoundSettings()
    {
        so = new SoundOptions();

        #region SOUND JSON INIT
        /*
        so.soundVolume = 0.8f;
        so.musicVolume = 0.5f;
        so.eax = true;

        if (so != null)
            SaveManager.SoundSave(so);
        */    
        #endregion

        so = SaveManager.SoundLoad();

        SoundVolume = so.soundVolume;
        MusicVolume = so.musicVolume;
        Eax = so.eax;

        SoundManager.Instance.SetSoundVolume(SoundVolume);
        SoundManager.Instance.SetMusicVolume(MusicVolume);
    }

    private void LoadGameSettings()
    {
        go = new GameOptions();

        #region GAME JSON INIT
        /*
        go.difficulty = 3;
        go.showcrosschair = true;
        go.dynamiccrosschair = true;
        go.showweapon = true;
        go.aimdistance = false;
        go.recognize_npc = true;

        if (go != null)
            SaveManager.GameSave(go);
        */
        #endregion

        go = SaveManager.GameLoad();

        difficulty = (Difficulty)(go.difficulty);
        ShowCrosschair = go.showcrosschair;
        DynamicCrosschair = go.dynamiccrosschair;
        ShowWeapon = go.showweapon;
        AimDistance = go.aimdistance;
        NpcRecognition = go.recognize_npc;
    }

    private void LoadKeyboardSettings()
    {
        ko = new KeyboardOptions();

        #region KEYBOADR JSON INIT
        /*
        ko.mouseSensitivity = 0.3f;
        ko.invertmouse = false;

        // Direction
        ko.kLeft = "LeftArrow";
        ko.kRight = "RightArrow";
        ko.kUp = "UpArrow";
        ko.kDown = "DownArrow";

        // Movement
        ko.kForward = "W";
        ko.kBackward = "S";
        ko.kStepLeft = "A";
        ko.kStepRight = "D";
        ko.kJump = "Space";
        ko.kSit = "C";
        ko.kAllwaysSit = "Capslock";
        ko.kStep = "LeftShift";
        ko.kRun = "X";
        ko.kLookLeft = "Q";
        ko.kLookRight = "E";

        // Weapon
        ko.kWeapon1 = "1";
        ko.kWeapon2 = "2";
        ko.kWeapon3 = "3";
        ko.kWeapon4 = "4";
        ko.kWeapon5 = "5";
        ko.kWeapon6 = "6";
        ko.kBulletType = "Y";
        ko.kNextSlot = "[";
        ko.kPreviousSlot = "]";
        ko.kFire = "Mouse0";
        ko.kOptic = "Mouse1";
        ko.kReload = "R";
        ko.kUnderbarell = "V";
        ko.kFiremodNext = "<";
        ko.kFiremodPrevious = ">";

        // Inventory
        ko.kInventory = "I";
        ko.kActiveTasks = "Tab";
        ko.kMap = "M";
        ko.kContacts = "";
        ko.kLight = "L";
        ko.kNightvision = "N";
        ko.kBandage = "F1";
        ko.kFirstAid = "F3";
        ko.kOffload = ";";
        ko.kDetector = "O";

        // Common
        ko.kPause = "Pause";
        ko.kUse = "F";
        ko.kQuickSave = "F5";
        ko.kQuickLoad = "F9";

        if (ko != null)
            SaveManager.KeyboardSave(ko);
        */
        #endregion

        #region KEYBOARD DEFAULT SETTINGS

        // Load defaul Keyboard settings
        DefaultActionKey = new Dictionary<string, KeyCode>();
        // Direction
        DefaultActionKey.Add("kLeft", KeyCode.LeftArrow);
        DefaultActionKey.Add("kRight", KeyCode.RightArrow);
        DefaultActionKey.Add("kUp", KeyCode.UpArrow);
        DefaultActionKey.Add("kDown", KeyCode.DownArrow);
        // Movement
        DefaultActionKey.Add("kForward", KeyCode.W);
        DefaultActionKey.Add("kBackward", KeyCode.S);
        DefaultActionKey.Add("kStepLeft", KeyCode.A);
        DefaultActionKey.Add("kStepRight", KeyCode.D);
        DefaultActionKey.Add("kJump", KeyCode.Space);
        DefaultActionKey.Add("kSit", KeyCode.C);
        DefaultActionKey.Add("kAllwaysSit", KeyCode.CapsLock);
        DefaultActionKey.Add("kStep", KeyCode.LeftShift);
        DefaultActionKey.Add("kRun", KeyCode.X);
        DefaultActionKey.Add("kLookLeft", KeyCode.Q);
        DefaultActionKey.Add("kLookRight", KeyCode.E);
        // Weapon
        DefaultActionKey.Add("kWeapon1", KeyCode.Keypad1);
        DefaultActionKey.Add("kWeapon2", KeyCode.Keypad2);
        DefaultActionKey.Add("kWeapon3", KeyCode.Keypad3);
        DefaultActionKey.Add("kWeapon4", KeyCode.Keypad4);
        DefaultActionKey.Add("kWeapon5", KeyCode.Keypad5);
        DefaultActionKey.Add("kWeapon6", KeyCode.Keypad6);
        DefaultActionKey.Add("kBulletType", KeyCode.Y);
        DefaultActionKey.Add("kNextWeapon", KeyCode.Backslash);
        DefaultActionKey.Add("kPrevWeapon", KeyCode.Slash);
        DefaultActionKey.Add("kFire", KeyCode.Mouse0);
        DefaultActionKey.Add("kOptic", KeyCode.Mouse1);
        DefaultActionKey.Add("kReload", KeyCode.R);
        DefaultActionKey.Add("kUnderbarrel", KeyCode.V);
        DefaultActionKey.Add("kFiremodeNext", KeyCode.Keypad0);
        DefaultActionKey.Add("kFiremodPrev", KeyCode.Keypad9);
        // Inventory
        DefaultActionKey.Add("kInventory", KeyCode.Tab);
        DefaultActionKey.Add("kTasks", KeyCode.P);
        DefaultActionKey.Add("kMap", KeyCode.M);
        DefaultActionKey.Add("kContacts", KeyCode.H);
        DefaultActionKey.Add("kLight", KeyCode.L);
        DefaultActionKey.Add("kNightVision", KeyCode.N);
        DefaultActionKey.Add("kBandage", KeyCode.F1);
        DefaultActionKey.Add("kFirtAid", KeyCode.F3);
        DefaultActionKey.Add("kDropWeapon", KeyCode.G);
        // Common
        DefaultActionKey.Add("kPause", KeyCode.Pause);
        DefaultActionKey.Add("kUse", KeyCode.F);
        DefaultActionKey.Add("kSave", KeyCode.F5);
        DefaultActionKey.Add("kLoad", KeyCode.F9);
        #endregion
    }

    void Start()
    {
        LoadOptionsSettings();
        CheckSavedGames();

    }


    void Update()
    {
        #region GAME MODE KEY HANDLE
        if (Input.GetKey(KeyCode.Escape))
        {
            // HudCanvas.SetActive(false);

            // (GameState == GAME_STATE.in_progress)
            if (GameMode == GAME_MODE.dialog)
            {
                // выходим из диалога
                GameMode = GAME_MODE.game;
                DialogCanvas.SetActive(false);
                HudCanvas.SetActive(true);
                canvasType = CANVAS_TYPE.Default;

            }
            if ((GameMode == GAME_MODE.game))
            {
                // Идет игра, нажали Esc, вышли в меню
                IsPaused = true;
                HudCanvas.SetActive(false);
                DialogCanvas.SetActive(false);

                LootCanvas.SetActive(false);
                TradeCanvas.SetActive(false);
                PlayerCanvas.SetActive(false);
                canvasType = CANVAS_TYPE.Default;

                MenuCanvas.SetActive(true);
                MainMenu_new.SetActive(false);
                MainMenu_resume.SetActive(true);
                MenuMusic.SetActive(true);

            }
            if (GameMode == GAME_MODE.inventory)
            {
                OptionsMenu.SetActive(false);
                OptionsSoundMenu.SetActive(false);
                OptionsGameMenu.SetActive(false);
                OptionsKeyboardMenu.SetActive(false);
                GameMode = GAME_MODE.main_menu;

            }

            if (GameState == GAME_STATE.init)
            {
                // Игру еще не запустили, просто выходим в начальное меню
                IsPaused = true;
                canvasType = CANVAS_TYPE.Default;

                MenuCanvas.SetActive(true);
                MainMenu_new.SetActive(true);
                MainMenu_resume.SetActive(false);
            }
        }
        #endregion


        #region CHANGE CANVASES - DEBUG

        if (Input.GetKeyDown(KeyCode.L))
        {
            HudCanvas.SetActive(false);
            DialogCanvas.SetActive(false);

            LootCanvas.SetActive(true);
            TradeCanvas.SetActive(false);
            PlayerCanvas.SetActive(false);
            canvasType = CANVAS_TYPE.Loot;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            HudCanvas.SetActive(false);
            DialogCanvas.SetActive(false);

            LootCanvas.SetActive(false);
            TradeCanvas.SetActive(true);
            PlayerCanvas.SetActive(false);
            canvasType = CANVAS_TYPE.Trade;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            HudCanvas.SetActive(false);
            DialogCanvas.SetActive(false);

            LootCanvas.SetActive(false);
            TradeCanvas.SetActive(false);
            PlayerCanvas.SetActive(true);
            canvasType = CANVAS_TYPE.Player;

        }
        #endregion

        #region CHECK KEY - FOR OPTIONS

        if (Input.anyKey && Instance.KeyBind)
        {

            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                {
                    Keypressed = kcode.ToString();   //
                    Debug.Log("KeyCode down: " + kcode);
                    KeyBind = false;
                    return;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Current solution for realloc Keyboard settings
    /// </summary>
    /// <param name="m_input"></param>
    public void CheckKey(InputField m_input)
    {
        m_input.text = Keypressed;
        m_input.GetComponent<Image>().color = Color.white;
        //m_input.OnUpdateSelected();
        //return Keypressed;
    }


    #region PLAY INTRO
    IEnumerator PlayIntro()
    {
        VideoPlayer vp = Intro.GetComponent<VideoPlayer>();
        LoadLevelScreen.SetActive(false);
        vp.Play();
        vp.loopPointReached += EndReached;

        yield return null;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.Stop();
        LoadLevelScreen.SetActive(true);
        IntroCanvas.gameObject.SetActive(false);
    }

    public void GoAhead()
    {
        LoadLevelScreen.SetActive(false);
        HudCanvas.SetActive(true);
        IsPaused = false;
        GameState = GAME_STATE.in_progress;
        GameMode = GAME_MODE.game;
        //MenuMusic.SetActive(true);
        // Проиграть писк КПК
    }

    #endregion

    #region MAIN MENU BUTTON

    public void OnNewGameButtonClick()
    {
        LoadLevelScreen.SetActive(true);
        MenuMusic.SetActive(false);
        IsPaused = true;
        MainMenu_new.gameObject.SetActive(false);
        IntroCanvas.gameObject.SetActive(true);
        StartCoroutine(PlayIntro());
    }

    public void OnResumeButtonClick()
    {
        MenuMusic.SetActive(false);
        MainMenu_resume.SetActive(false);
        MenuCanvas.SetActive(false);
        HudCanvas.SetActive(true);
        IsPaused = false;
    }

    public void OnLoadGameButtonClick()
    {
        LoadGameMenu.SetActive(true);
    }

    public void OnLastSaveButtonClick()
    {
        // TODO
    }

    public void OnSaveGameButtonClick()
    {
        SaveGameMenu.SetActive(true);
    }

    public void OnNetGameButtonClick()
    {
        // TODO
    }

    public void OnOptionsButtonClick()
    {
        OptionsMenu.SetActive(true);

        //FullscreenCheckbox.
        GameMode = GAME_MODE.inventory;
    }

    public void OnCreditsButtonClick()
    {
        MainMenu_new.SetActive(false);
        MainMenu_resume.SetActive(false);
        MenuMusic.SetActive(false);
        CreditsScreen.SetActive(true);
    }

    public void OnExitGameButtonClick()
    {
        // ...  это приведет к потере всех несохраненных данных
        // LoadDefault()
        GameState = GAME_STATE.init;
        GameMode = GAME_MODE.main_menu;
        canvasType = CANVAS_TYPE.Default;
        IsPaused = true;

        StartLoading();
    }

    public void OnQuitButtonClick()
    {
        QuitWindowsDialog.SetActive(true);
    }

    public void QuitWindows()
    {
        // TODO Сохранить игру

        Application.Quit();
    }

    public void CancelQuitWindows()
    {
        QuitWindowsDialog.SetActive(false);
    }

    #endregion

    #region OPTIONS MENU BUTTON

    #region OPTIONS TABS VIDEO SOUND GAME KEYBOARD

    public void VideoBtnClick()
    {
        OptionsMenu.SetActive(true);
        //
        OptionsSoundMenu.SetActive(false);
        OptionsGameMenu.SetActive(false);
        OptionsKeyboardMenu.SetActive(false);
    }

    public void SoundBtnClick()
    {
        OptionsSoundMenu.SetActive(true);
        //
        OptionsMenu.SetActive(false);
        OptionsGameMenu.SetActive(false);
        OptionsKeyboardMenu.SetActive(false);
    }

    public void GameBtnClick()
    {
        OptionsGameMenu.SetActive(true);
        //
        OptionsSoundMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        OptionsKeyboardMenu.SetActive(false);
    }

    public void KeyboardClick()
    {
        OptionsKeyboardMenu.SetActive(true);
        //
        OptionsSoundMenu.SetActive(false);
        OptionsGameMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    #endregion


    #region VIDEO - надо рефакторинг
    
    public void EnchencedBtnClick()
    {
        // TODO
    }

    public void BaseBtnClick()
    {
        // TODO
    }

    public void VideoApply()
    {
        // Нажимаем "Применить" 
        DropdownItemSelected(ResolutionDrop);

        OptionsMenu.SetActive(false);
    }

    public void VideoCancel()
    {
        OptionsMenu.SetActive(false);
    }
    
    #endregion VIDEO

    #region SOUND - надо рефакторинг
    
    public void SoundApply()
    {
        // Apply changes
        
        OptionsSoundMenu.SetActive(false);
    }
    
    public void SoundCancel()
    {
        // OptionsSoundMenu
        OptionsSoundMenu.SetActive(false);
    }
    #endregion SOUND

    /*
    #region GAME - надо рефакторинг
    
    public void GameApply()
    {
        // Apply changes
        OptionsGameMenu.SetActive(false);
    }

    public void GameCancel()
    {
        OptionsGameMenu.SetActive(false);
    }
    #endregion GAME
    */

    #region KEYBOARD
    public void KeyboardDefaultBtnClick()
    {
        // Load default
        LoadKeyboardSettings();
    }

    public void KeyboardApply()
    {
        // Apply changes
        SaveKeyboardSettings();
        OptionsKeyboardMenu.SetActive(false);
    }

    public void SaveKeyboardSettings()
    {
        //
    }

    public void KeyboardCancel()
    {
        OptionsKeyboardMenu.SetActive(false);
    }

    #endregion KEYBOARD

    #endregion OPTIONS MENU BUTTON

}


///
/// OPTIONS SETTINGS enums
///
#region OPTIONS SETTINGS enums

public enum RenderType
{
    STATIC,
    MIXED,
    DYNAMIC
}

public enum RenderQuality
{
    MINIMAL,
    LOW,
    MIDDLE,
    NORMAL,
    MAX
}

public enum PreferedResolutions
{
    // Здесь мы по сути задаем список разрешений которые хотим видеть
    // Далее мы работаем с List<Resolution>
    r800x600,
    r1024x768,
    r1280x600,
    r1280x768,
    r1280x800,
    r1400x1050,
    r1440x900,
    r1600x900,
    r1920x1080
}

public enum Difficulty
{
    NOVICE,
    NORMAL,
    STALKER,
    MASTER
}
#endregion


#region GAME STATE enums

public enum CANVAS_TYPE
{
    Default,
    Loot,
    Trade,
    Player
}

public enum GAME_MODE
{
    main_menu,
    game,
    inventory,
    dialog,
    pda
}

public enum GAME_STATE
{
    init,
    in_progress
}
#endregion
