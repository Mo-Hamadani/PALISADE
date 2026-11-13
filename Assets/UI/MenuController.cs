using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UIButton = UnityEngine.UIElements.Button;

public class MenuController : MonoBehaviour
{
    private UIDocument _Doc;
    private UnityEngine.UIElements.Button _PlayButton;
    private UnityEngine.UIElements.Button _SettingsButton;
    private UnityEngine.UIElements.Button _QuitButton;

    private void Awake()
    {
        _Doc = GetComponent<UIDocument>();
        _PlayButton = _Doc.rootVisualElement.Q<UIButton>("PlayButton");
        _SettingsButton = _Doc.rootVisualElement.Q<UIButton>("SettingsButton");
        _QuitButton = _Doc.rootVisualElement.Q<UIButton>("QuitButton");
        _PlayButton.clicked += PlayButtonOnClicked
    }
}
