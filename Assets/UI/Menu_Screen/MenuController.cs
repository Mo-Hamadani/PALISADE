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
    private Button PlayButton, SettingsButton, QuitButton, BackButton;
    private VisualElement Buttons, SettingsPanel, Title;
    private Slider VolumeSlider;
    private DropdownField ResolutionDropDown, FPSDropDown;
    private Toggle FullScreenToggle, FPSToggle;
    private Label FPSLabel;

    private readonly List<Vector2Int> resolutions = new List<Vector2Int>();
    private readonly int[] fpsOptions = { 30, 60, 120, 144, 240, -1 };

    private float fpsTimer;
    private int framecount;

    private void OnEnable()
    {
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

        SetupResolutionDropDown();
        SetupFPSDropDown();

        FullScreenToggle.SetValueWithoutNotify(Screen.fullScreen);
        FPSToggle.SetValueWithoutNotify(false);
        VolumeSlider.SetValueWithoutNotify(AudioListener.volume);
        FPSLabel.style.display = DisplayStyle.None;
        SettingsPanel.style.display = DisplayStyle.None;

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
    private void Update()
    {
        if (FPSLabel.style.display == DisplayStyle.None) return;

        framecount++;
        fpsTimer += Time.unscaledDeltaTime;
        if (fpsTimer >= 0.5f)
        {
            FPSLabel.text = Mathf.RoundToInt(framecount / fpsTimer) + " FPS";
            framecount = 0;
            fpsTimer = 0f;
        }
    }

    private void SetupResolutionDropDown()
    {
        var choices = new List<string>();
        resolutions.Clear();

        foreach (var r in Screen.resolutions)
        {
            var size = new Vector2Int(r.width, r.height);
            if (resolutions.Contains(size)) continue;
            resolutions.Add(size);
            choices.Add(r.width + " x " + r.height);
        }

        ResolutionDropDown.choices = choices;

        int current = resolutions.IndexOf(new Vector2Int(Screen.width, Screen.height));
        ResolutionDropDown.index = current >= 0 ? current : resolutions.Count - 1;
    }

    private void SetupFPSDropDown()
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

    private void OnFullScreenChanged(ChangeEvent<bool> evt)
    {
        Screen.fullScreenMode = evt.newValue
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;
    }

    private void OnFPSChanged(ChangeEvent<string> evt)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fpsOptions[FPSDropDown.index];
    }

    private void OnShowFPSChanged(ChangeEvent<bool> evt)
    {
        FPSLabel.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    private void OnVolumeChanged(ChangeEvent<float> evt)
    {
        AudioListener.volume = evt.newValue;
    }
    private void OnPlayClicked() 
    {
        SceneManager.LoadScene("TEST_MO");
    }
    private void OnSettingsClicked()
    {
        Title.style.display = DisplayStyle.None;
        Buttons.style.display = DisplayStyle.None;
        SettingsPanel.style.display = DisplayStyle.Flex;
    }
    
    private void OnBackClicked()
    {
        SettingsPanel.style.display = DisplayStyle.None;
        Title.style.display = DisplayStyle.Flex;
        Buttons.style.display = DisplayStyle.Flex;
    }
    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
