using CommonCameraUtil.API;
using HarmonyLib;
using ImGuiNET;
using ImGuiOW.API;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;
using UnityEngine;

namespace NHEditor;

public class NHEditor : ModBehaviour
{
	public static NHEditor Instance;
	public static IModConsole Console => Instance?.ModHelper?.Console;

	public ICommonCameraAPI CommonCameraAPI { get; private set; }
	public IImGuiAPI ImGuiAPI { get; private set; }
	public IGizmosAPI GizmosAPI { get; private set; }

	public OWCamera EditorCamera { get; private set; }

	public void Awake() => Instance = this;

	public void Start() {
		Console.Success("NHEditor loaded.");

		new Harmony("Throckmorpheus.NHEditor").PatchAll(Assembly.GetExecutingAssembly());

		CommonCameraAPI = ModHelper.Interaction.TryGetModApi<ICommonCameraAPI>("xen.CommonCameraUtility");
		ImGuiAPI = ModHelper.Interaction.TryGetModApi<IImGuiAPI>("Throckmorpheus.ImGuiOW");
		GizmosAPI = ModHelper.Interaction.TryGetModApi<IGizmosAPI>("Locochoco.GizmosLibrary");

		(EditorCamera, _) = CommonCameraAPI.CreateCustomCamera("NHEditorCamera");

		ImGuiAPI.Layout += OnLayout;
	}

	private bool _inEditorCamera = false;

	private void OnLayout() {
		ImGui.SetNextWindowSize(new(100, 100), ImGuiCond.Once);
		ImGui.Begin("NHEditor");
		if (ImGui.Checkbox("Editor Cam", ref _inEditorCamera)) {
			if (!_inEditorCamera) CommonCameraAPI.ExitCamera(EditorCamera); else CommonCameraAPI.EnterCamera(EditorCamera);
		}
		ImGui.End();
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