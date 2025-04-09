using HarmonyLib;
using ImGuiNET;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;

namespace NHEditor;

public class NHEditor : ModBehaviour
{
	public static NHEditor Instance;
	public static IModConsole Console => Instance?.ModHelper?.Console;

	public ImGuiOW.API.IImGuiAPI ImGuiAPI;

	public void Awake() => Instance = this;

	public void Start() {
		Console.Success("NHEditor loaded.");

		new Harmony("Throckmorpheus.NHEditor").PatchAll(Assembly.GetExecutingAssembly());

		ImGuiAPI = ModHelper.Interaction.TryGetModApi<ImGuiOW.API.IImGuiAPI>("Throckmorpheus.ImGuiOW");

		ImGuiAPI.Layout += () => {
			ImGui.Begin("NHEditor", ImGuiWindowFlags.AlwaysAutoResize);
			ImGui.Text("Test");
			ImGui.End();
		};
	}
}