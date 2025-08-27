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

	public static ICommonCameraAPI CommonCameraAPI { get; private set; }
	public static IImGuiAPI ImGuiAPI { get; private set; }
	public static IGizmosAPI GizmosAPI { get; private set; }

	public void Awake() => Instance = this;

	public void Start() {
		Console.Success($"{ModHelper.Manifest.Name} loaded.");
		new Harmony(ModHelper.Manifest.UniqueName).PatchAll(Assembly.GetExecutingAssembly());

		CommonCameraAPI = ModHelper.Interaction.TryGetModApi<ICommonCameraAPI>("xen.CommonCameraUtility");
		ImGuiAPI = ModHelper.Interaction.TryGetModApi<IImGuiAPI>("Throckmorpheus.ImGuiOW");
		GizmosAPI = ModHelper.Interaction.TryGetModApi<IGizmosAPI>("Locochoco.GizmosLibrary");

		gameObject.AddComponent<InputManager>();
		gameObject.AddComponent<EditorManager>();
	}

	/*public void OnRenderObject() {
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
    }*/
}