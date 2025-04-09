using CommonCameraUtil.API;
using HarmonyLib;
using ImGuiNET;
using ImGuiOW.API;
using CuriosityEditor.Components;
using OWML.Common;
using OWML.ModHelper;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

namespace CuriosityEditor;

public class Main : ModBehaviour
{
	public static Main Instance;
	public static IModConsole Console => Instance?.ModHelper?.Console;

	public static ICommonCameraAPI CommonCameraAPI { get; private set; }
	public static IImGuiAPI ImGuiAPI { get; private set; }
	public static IGizmosAPI GizmosAPI { get; private set; }

	private bool _inEditorMode = false;
	public static bool InEditor {
		get => Instance._inEditorMode;
		set {
			if (value != Instance._inEditorMode) { if (value) EnterEditor?.Invoke(); else ExitEditor?.Invoke(); }
			Instance._inEditorMode = value;
		}
	}

	public static event Action EnterEditor;
	public static event Action ExitEditor;

	public static OWCamera EditorCamera { get; private set; }

	private bool _inEditorCamera = false;
	private InputMode _returnInputMode;

	public void Awake() => Instance = this;

	public void Start() {
		Console.Success($"{ModHelper.Manifest.Name} loaded.");
		new Harmony(ModHelper.Manifest.UniqueName).PatchAll(Assembly.GetExecutingAssembly());

		CommonCameraAPI = ModHelper.Interaction.TryGetModApi<ICommonCameraAPI>("xen.CommonCameraUtility");
		ImGuiAPI = ModHelper.Interaction.TryGetModApi<IImGuiAPI>("Throckmorpheus.ImGuiOW");
		GizmosAPI = ModHelper.Interaction.TryGetModApi<IGizmosAPI>("Locochoco.GizmosLibrary");

		CreateCamera();
		SceneManager.sceneLoaded += OnSceneLoaded;

		GlobalMessenger<OWCamera>.AddListener("SwitchActiveCamera", OnSwitchActiveCamera);

		EnterEditor += OnEnterEditor;
		ExitEditor += OnExitEditor;
	}

	public void OnDestroy() {
		GlobalMessenger<OWCamera>.RemoveListener("SwitchActiveCamera", OnSwitchActiveCamera);
		SceneManager.sceneLoaded -= OnSceneLoaded;
		EnterEditor -= OnEnterEditor;
		ExitEditor -= OnExitEditor;
	}

	private void CreateCamera() {
		_inEditorCamera = false;

		(EditorCamera, _) = CommonCameraAPI.CreateCustomCamera("NHEditorCamera", (owCamera) => {
			if (owCamera?._postProcessing?.profile?.eyeMask is EyeMaskModel eyeMask) eyeMask.enabled = false;
			owCamera.mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
			owCamera.mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HeadsUpDisplay"));
			owCamera.mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HelmetUVPass"));
		});

		EditorCamera.gameObject.AddComponent<EditorCameraController>();

		EditorCamera.gameObject.SetActive(true);
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => CreateCamera();

	private void OnSwitchActiveCamera(OWCamera camera) {
		_inEditorCamera = camera == EditorCamera;
		// Possibly disable editor mode and set to return to editor mode on return to editor camera?
	}

	private void OnEnterEditor() {
		if (!Instance._inEditorCamera) CommonCameraAPI.EnterCamera(EditorCamera);
		_returnInputMode = OWInput.GetInputMode();
		OWInput.ChangeInputMode(InputMode.None);
	}
	private void OnExitEditor() {
		if (Instance._inEditorCamera) CommonCameraAPI.ExitCamera(EditorCamera);
		OWInput.ChangeInputMode(_returnInputMode == InputMode.None ? InputMode.Character : _returnInputMode);
	}

	public void OnRenderObject() {
		GizmosAPI.SetDefaultMaterialPass();
        GizmosAPI.DrawOnGlobalReference(() =>
        {
            //You can call DrawAxis outside of DrawOnGlobalReference, but it is recommended to do it inside one
            //if you want the global reference frame, as this pushes and pops the identity transform matrix for you
            GizmosAPI.DrawAxis(0.25f, Color.green, Vector3.zero);
        });
        GizmosAPI.DrawAxis(0.25f, Color.red, Vector3.zero);

        if (Locator.GetPlayerTransform() == null)
            return;

        //Draw Player Transform Example
        GizmosAPI.DrawWithReference(Locator.GetPlayerTransform(), () =>
        {
            //If inside DrawWithReference it will draw as if it was a child of the transform,
            //so its position, rotation and scale will affect the final draw.
            //This is the recommended way to draw relative stuff like axis

            GizmosAPI.DrawAxis(0.25f, Color.green, Vector3.zero);
            //The head size scales with the transform scale, so 0.25f is more a 25%
        });
    }
}