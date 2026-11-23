using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";

    //references for the ui elements, filled in on the OnEnable by searching them up in the UXML
    private Button PlayButton, SettingsButton, QuitButton, BackButton;
    private VisualElement Buttons, SettingsPanel, Title;
    private Slider VolumeSlider;
    private DropdownField ResolutionDropDown, FPSDropDown;
    private Toggle FullScreenToggle, FPSToggle;
    private Label FPSLabel;
    private readonly List<Vector2Int> resolutions = new List<Vector2Int>();
    private readonly int[] fpsOptions = { 30, 60, 120, 144, 240, -1 };

    private float fpsTimer; //calculates the FPS rate for the counter
    private int framecount;

    private void OnEnable()
    {
        //finds all the elements by their name in the ui builder 
        var root = GetComponent<UIDocument>().rootVisualElement;
        PlayButton = root.Q<Button>("PlayButton");
        SettingsButton = root.Q<Button>("SettingsButton");
        QuitButton = root.Q<Button>("QuitButton");
        BackButton = root.Q<Button>("BackButton");
        Title = root.Q<VisualElement>("Title");
        Buttons = root.Q<VisualElement>("Buttons");
        SettingsPanel = root.Q<VisualElement>("SettingsPanel");
        VolumeSlider = root.Q<Slider>("VolumeSlider");
        ResolutionDropDown = root.Q<DropdownField>("ResolutionDropDown");
        FPSDropDown = root.Q<DropdownField>("FPSDropDown");
        FullScreenToggle = root.Q<Toggle>("FullScreenToggle");
        FPSToggle = root.Q<Toggle>("ShowFPSToggle");
        FPSLabel = root.Q<Label>("FPSLabel");

        //sets up the options you can select in the drop downs
        SetupResolutionDropDown();
        SetupFPSDropDown();

        FullScreenToggle.SetValueWithoutNotify(Screen.fullScreen);
        FPSToggle.SetValueWithoutNotify(false);
        VolumeSlider.SetValueWithoutNotify(AudioListener.volume);

        //hides the fps and settings panel by default till selected
        FPSLabel.style.display = DisplayStyle.None;
        SettingsPanel.style.display = DisplayStyle.None;

        //calls the event for each element when interacted with
        PlayButton.clicked += OnPlayClicked;
        SettingsButton.clicked += OnSettingsClicked;
        QuitButton.clicked += OnQuitClicked;
        BackButton.clicked += OnBackClicked;
        VolumeSlider.RegisterValueChangedCallback(OnVolumeChanged);
        ResolutionDropDown.RegisterValueChangedCallback(OnResolutionChanged);
        FPSDropDown.RegisterValueChangedCallback(OnFPSChanged);
        FullScreenToggle.RegisterValueChangedCallback(OnFullScreenChanged);
        FPSToggle.RegisterValueChangedCallback(OnShowFPSChanged);
    }

    private void OnDisable()
    {
        //unhooks the events so nothing is calle multiple times
        PlayButton.clicked -= OnPlayClicked;
        SettingsButton.clicked -= OnSettingsClicked;
        QuitButton.clicked -= OnQuitClicked;
        BackButton.clicked -= OnBackClicked;
        VolumeSlider.UnregisterValueChangedCallback(OnVolumeChanged);
        ResolutionDropDown.UnregisterValueChangedCallback(OnResolutionChanged);
        FPSDropDown.UnregisterValueChangedCallback(OnFPSChanged);
        FullScreenToggle.UnregisterValueChangedCallback(OnFullScreenChanged);
        FPSToggle.UnregisterValueChangedCallback(OnShowFPSChanged);

    }
    private void Update() //runs the fps system every frame
    {
        if (FPSLabel.style.display == DisplayStyle.None) return;  // doesn't check the frame rate if the label is hidden

        framecount++;
        fpsTimer += Time.unscaledDeltaTime; //counts the frames in real time
        if (fpsTimer >= 0.5f) // works out the average fps and updates the visible label
        {
            FPSLabel.text = Mathf.RoundToInt(framecount / fpsTimer) + " FPS";
            framecount = 0;
            fpsTimer = 0f;
        }
    }

    private void SetupResolutionDropDown() //sets the resolution dropdown options with res the screen supports
    {
        var choices = new List<string>();
        resolutions.Clear();

        foreach (var r in Screen.resolutions)
        {
            var size = new Vector2Int(r.width, r.height);
            if (resolutions.Contains(size)) continue; //avoids duplicate resolutions
            resolutions.Add(size);
            choices.Add(r.width + " x " + r.height);
        }

        ResolutionDropDown.choices = choices; // sets the selected res to the current

        int current = resolutions.IndexOf(new Vector2Int(Screen.width, Screen.height));
        ResolutionDropDown.index = current >= 0 ? current : resolutions.Count - 1;
    }

    private void SetupFPSDropDown() //sets up the fps limit from the fps options array
    {
        var choices = new List<string>();
        foreach (int f in fpsOptions)
            choices.Add(f == -1 ? "Unlimited" : f + "FPS");

        FPSDropDown.choices = choices;

        int index = System.Array.IndexOf(fpsOptions, Application.targetFrameRate);
        FPSDropDown.index = index >= 0 ? index : 1;
    }

    private void OnResolutionChanged(ChangeEvent<string> evt)
    {
        var size = resolutions[ResolutionDropDown.index];
        Screen.SetResolution(size.x, size.y, Screen.fullScreenMode);
    }

    private void OnFullScreenChanged(ChangeEvent<bool> evt) //switches between window mode full screen and boarderless fullscreen
    {
        Screen.fullScreenMode = evt.newValue
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;
    }

    private void OnFPSChanged(ChangeEvent<string> evt) // sets the fps cap
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fpsOptions[FPSDropDown.index];
    }

    private void OnShowFPSChanged(ChangeEvent<bool> evt) //toggle fps on and off
    {
        FPSLabel.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    private void OnVolumeChanged(ChangeEvent<float> evt) //sets the master audio for all audio
    {
        AudioListener.volume = evt.newValue;
    }
    private void OnPlayClicked() //loads game scene
    {
        SceneManager.LoadScene("TEST_MO");
    }
    private void OnSettingsClicked() //hides title and main button and shows the settings pannel
    {
        Title.style.display = DisplayStyle.None;
        Buttons.style.display = DisplayStyle.None;
        SettingsPanel.style.display = DisplayStyle.Flex;
    }
    
    private void OnBackClicked() //goes back to the main menu from settings
    {
        SettingsPanel.style.display = DisplayStyle.None;
        Title.style.display = DisplayStyle.Flex;
        Buttons.style.display = DisplayStyle.Flex;
    }
    private void OnQuitClicked() //closes the game
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
