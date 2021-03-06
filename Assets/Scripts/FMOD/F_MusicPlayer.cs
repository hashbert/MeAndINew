using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class F_MusicPlayer : MonoBehaviour
    {
    public EventReference _menuMusic;
    public EventReference _gameplayMusic;
    public EventInstance MenuMusicInst;
    public EventInstance GameplayMusicInst;
    private EventInstance CurrentMusicInst;
    public string CurrentSceneName { get; private set; }
    public bool SceneRepeated { get; private set; }

    private static F_MusicPlayer _instance;
    public static F_MusicPlayer instance {
        get {
            if (_instance == null) {
                Object.Instantiate(Resources.Load<F_MusicPlayer>("Prefabs/MusicObject"));
            }
            return _instance;
        }

        private set => _instance = value;
    }

    private EventDescription EventDes;
    private PARAMETER_DESCRIPTION ParamDes;

    void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
        SceneRepeated = false;
    }

    void OnEnable() {
        SceneManager.activeSceneChanged += PlayMusicForCurrentScene;

        EventDes = RuntimeManager.GetEventDescription("event:/Music/Gameplay Music");
        EventDes.getParameterDescriptionByName("CharacterSwitch", out ParamDes);
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= PlayMusicForCurrentScene;
    }

    private SceneMusicData sceneMusicData;
    private MusicType currentMusicType;
    private void PlayMusicForCurrentScene(Scene oldScene, Scene newScene) {
        Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);
        IsSceneRepeated();
        sceneMusicData = FindObjectOfType<SceneMusicData>();
        if (sceneMusicData == null)
        {
            return;
        }
        MusicType nextSceneMusic = sceneMusicData.music;

        if (currentMusicType == nextSceneMusic)
        {
            return;
        }
        StopCurrentMusic();
        EventInstance musicToStart;
        if (newScene.name == "MainMenuScene" || newScene.name == "CreditsScene") {
            MenuMusicInst = RuntimeManager.CreateInstance(_menuMusic);
            musicToStart = MenuMusicInst;
            currentMusicType = MusicType.MainMenu;
            Debug.Log("This is the MainMenuScene");
        } else if (SceneManager.GetActiveScene().name.Contains("Cutscene")) {
            GameplayMusicInst = RuntimeManager.CreateInstance(_gameplayMusic);
            instance.SetMusicParameter(3f);
            musicToStart = GameplayMusicInst;
            currentMusicType = MusicType.Gameplay;
            Debug.Log("This is a Cutscene");
        } else {
            //Should be dialogue scene and both instruments play
            GameplayMusicInst = RuntimeManager.CreateInstance(_gameplayMusic);
            instance.SetMusicParameter(3f);
            musicToStart = GameplayMusicInst;
            currentMusicType = MusicType.Gameplay;
            Debug.Log("This is any other level");
        }
        StartMusic(musicToStart);
    }

    private void IsSceneRepeated()
    {
        if (CurrentSceneName == SceneManager.GetActiveScene().name)
        {
            SceneRepeated = true;
        }
        else
        {
            SceneRepeated = false;
        }
        CurrentSceneName = SceneManager.GetActiveScene().name;
    }
    private void StopCurrentMusic() {
        CurrentMusicInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void StartMusic(EventInstance music) {
        music.start();
        music.release();
        CurrentMusicInst = music;
    }

    public void SetMusicParameter(float value) {
        GameplayMusicInst.setParameterByID(ParamDes.id, value);
        //Debug.Log(value);
    }
}