using Assets.Scripts;
using Assets.Scripts.Model;
using Assets.Scripts.Ui;
using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    private VisualElement _uiRoot;
    /// <summary>
    /// Контроллер визуальных элементов настроек игры
    /// </summary>
    private SettingsController _settingsController;
    public Button NewGameButton;
    public Button ContinueButton;
    public Button CloseButton;
    public Label FinishLabel;

    public GameController GameController;
    // Start is called before the first frame update
    void Start()
    {
        _uiRoot = GetComponent<UIDocument>().rootVisualElement;
        if (_uiRoot == null)
        {
            Debug.LogError("Root Visual Element is not added");
            return;
        }
        NewGameButton = _uiRoot.Q<Button>("new-game-btn");
        ContinueButton = _uiRoot.Q<Button>("continue-btn");
        CloseButton = _uiRoot.Q<Button>("close-btn");
        FinishLabel = _uiRoot.Q<Label>("finish-title");
        NewGameButton.clicked += OnNewGamePressed;
        ContinueButton.clicked += OnContinuePressed;
        CloseButton.clicked += OnClosePressed;
        GameController.GameStateChanged += UpdateControlsConfiguration;
        UpdateControlsConfiguration();
        _settingsController = new SettingsController(GameController, _uiRoot);
    }

    

    void UpdateControlsConfiguration()
    {
        FinishLabel.visible = GameController.GameState == EGameState.Finished;
        ContinueButton.visible = GameController.GameState == EGameState.Pause;
        if (GameController.GameState == EGameState.Finished)
        {
            _uiRoot.visible = true;
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_uiRoot.visible)
            {
                OnContinuePressed();
            }
            else
            {
                GameController.Pause();
                _uiRoot.visible = true;
            }
        }
    }
    public void OnNewGamePressed()
    {
        _uiRoot.visible = false;
        _settingsController.HideSettingsSliders();
        GameController.Restart();
    }
    public void OnContinuePressed()
    {
        _uiRoot.visible = false;
        _settingsController.HideSettingsSliders();
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
