using System;
using CuriosityEditor.Components;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

namespace CuriosityEditor;

public class EditorManager : MonoBehaviour {
    private static EditorManager Instance;

    private bool _inEditorMode = false;
	public static bool InEditor {
		get => Instance._inEditorMode;
		set {
			if (value != Instance._inEditorMode) {
                if (value) OnEnterEditor?.Invoke();
                else OnExitEditor?.Invoke();
            }
			Instance._inEditorMode = value;
		}
	}

    public static event Action OnEnterEditor;
	public static event Action OnExitEditor;
    
	private PauseMenuManager _pauseMenuManager;
    
	public static OWCamera EditorCamera { get; private set; }
	private bool _inEditorCamera = false;

	private InputMode _returnInputMode;
    
    public void Start() {
        if (Instance is not null) throw new Exception("Attempted to initialise more than one EditorManager");
        Instance = this;
		Main.Console.Info("Initialised Editor Manager.");
        
		OnEnterEditor += EnterEditor;
		OnExitEditor += ExitEditor;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
		GlobalMessenger<OWCamera>.AddListener("SwitchActiveCamera", OnSwitchActiveCamera);

        SceneInit();
    }

    public void OnDestroy() {
        SceneCleanup();
		GlobalMessenger<OWCamera>.RemoveListener("SwitchActiveCamera", OnSwitchActiveCamera);
		OnEnterEditor -= EnterEditor;
		OnExitEditor -= ExitEditor;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

    public void Update() {
        if (InputManager.Inputs.ToggleEditor.JustPressed && !IsInPauseMenu()) InEditor = !InEditor;
    }

    private bool IsInPauseMenu() => _pauseMenuManager?.IsOpen() ?? false;
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) { if (loadSceneMode == LoadSceneMode.Single) SceneInit(); }
    private void OnSceneUnloaded(Scene scene) { SceneCleanup(); }
    private void OnSwitchActiveCamera(OWCamera camera) { _inEditorCamera = camera == EditorCamera; }

    private void SceneInit() {
        // Find vanilla Pause Menu Manager
		_pauseMenuManager = FindObjectOfType<PauseMenuManager>();

        // Create editor camera
        (EditorCamera, _) = Main.CommonCameraAPI.CreateCustomCamera("CuriosityEditorCamera", (owCamera) => {
			if (owCamera?._postProcessing?.profile?.eyeMask is EyeMaskModel eyeMask) eyeMask.enabled = false;
            owCamera.renderSkybox = false;
			owCamera.mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
			owCamera.mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HeadsUpDisplay"));
			owCamera.mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HelmetUVPass"));
		});
        
		EditorCamera.gameObject.AddComponent<EditorCameraController>();
		EditorCamera.gameObject.SetActive(true);
        _inEditorCamera = false;

        if (InEditor) EnterEditor();
    }

    private void SceneCleanup() {
        _pauseMenuManager = null;
        if (InEditor) ExitEditor();
        // Currently it still breaks while switching scenes in editor mode
    }

    private void EnterEditor() {
		if (!_inEditorCamera && EditorCamera is not null) Main.CommonCameraAPI.EnterCamera(EditorCamera);
		_returnInputMode = OWInput.GetInputMode(); OWInput.ChangeInputMode(InputMode.Menu);

		OWTime.Pause(OWTime.PauseType.Menu);
		EditorCamera.enabled = true;
    }

    private void ExitEditor() {
		if (_inEditorCamera && EditorCamera is not null) Main.CommonCameraAPI.ExitCamera(EditorCamera);
		OWInput.ChangeInputMode(_returnInputMode == InputMode.None ? InputMode.Character : _returnInputMode);

		OWTime.Unpause(OWTime.PauseType.Menu);
		EditorCamera.enabled = false;
    }
}