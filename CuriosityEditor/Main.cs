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
}