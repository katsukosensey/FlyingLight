using Assets.Scripts;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    private VisualElement root;
    public Button NewGameButton;
    public Button ContinueButton;
    public Button CloseButton;
    public Label FinishLabel;
    public SliderInt speedSlider;
    public SliderInt accelerationSlider;
    public SliderInt mouseZoneSlider;

    public GameController GameController;
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        if (root == null)
        {
            Debug.Log("Root Visual Element is not added");
            return;
        }
        NewGameButton = root.Q<Button>("new-game-btn");
        ContinueButton = root.Q<Button>("continue-btn");
        CloseButton = root.Q<Button>("close-btn");
        FinishLabel = root.Q<Label>("finish-title");
        NewGameButton.clicked += OnNewGamePressed;
        ContinueButton.clicked += OnContinuePressed;
        CloseButton.clicked += OnClosePressed;
        GameController.GameStateChanged += UpdateControlsConfiguration;
        var speedBtn = root.Q<Button>("speed-btn");
        var accelerationBtn = root.Q<Button>("accel-btn");
        var mouseZoneBtn = root.Q<Button>("mouse-zone-btn");
        speedSlider = root.Q<SliderInt>("speed-slider");
        accelerationSlider = root.Q<SliderInt>("accel-slider");
        mouseZoneSlider = root.Q<SliderInt>("mouse-zone-slider");
        var mainContainer = root.Q<VisualElement>("main-container");
        HideSettingsSliders();
        speedBtn.RegisterCallback<MouseOverEvent>(evt => speedSlider.visible = true);
        accelerationBtn.RegisterCallback<MouseOverEvent>(evt => accelerationSlider.visible = true);
        mouseZoneBtn.RegisterCallback<MouseOverEvent>(evt => mouseZoneSlider.visible = true);
        speedSlider.RegisterValueChangedCallback(x=>GameController.BallController.Speed = x.newValue);
        accelerationSlider.RegisterValueChangedCallback(x => GameController.BallController.MinAcceleration = x.newValue);
        mouseZoneSlider.RegisterValueChangedCallback(x => GameController.BallController.SecureMouseDistance = x.newValue);

        mainContainer.RegisterCallback<MouseUpEvent>(evt => HideSettingsSliders());
    }

    void HideSettingsSliders()
    {
        speedSlider.visible = false;
        accelerationSlider.visible = false;
        mouseZoneSlider.visible = false;
    }

    public void DisplaySlider(SliderInt slider)
    {
        slider.visible = !slider.visible;
    }

    void UpdateControlsConfiguration()
    {
        FinishLabel.visible = GameController.GameState == EGameState.Finished;
        ContinueButton.visible = GameController.GameState == EGameState.Pause;
        if (GameController.GameState == EGameState.Finished)
        {
            root.visible = true;
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (root.visible)
            {
                GameController.Continue();
                HideSettingsSliders();
            }
            else
            {
                GameController.Pause();
            }
            root.visible = !root.visible;
        }
    }
    public void OnNewGamePressed()
    {
        root.visible = false;
        HideSettingsSliders();
        GameController.Restart();
    }
    public void OnContinuePressed()
    {
        root.visible = false;
        HideSettingsSliders();
        GameController.Continue();
    }

    public void OnClosePressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
