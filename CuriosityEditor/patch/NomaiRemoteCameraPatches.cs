using HarmonyLib;

namespace CuriosityEditor.Patches;

[HarmonyPatch(typeof(NomaiRemoteCamera))]
internal static class NomaiRemoteCameraPatches
{
    private static InputMode _oldInputMode;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(NomaiRemoteCamera.Activate))]
    public static void NomaiRemoteCamera_Activate() => _oldInputMode = OWInput.GetInputMode();

    [HarmonyPostfix]
    [HarmonyPatch(nameof(NomaiRemoteCamera.Deactivate))]
    public static void NomaiRemoteCamera_Deactivate() => OWInput.ChangeInputMode(_oldInputMode);
}