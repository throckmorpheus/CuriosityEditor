using HarmonyLib;

namespace CuriosityEditor.Patches;

[HarmonyPatch(typeof(MapController))]
internal static class MapControllerPatches
{
    private static InputMode _oldInputMode;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MapController.EnterMapView))]
    public static void MapController_EnterMapView() => _oldInputMode = OWInput.GetInputMode();

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapController.ExitMapView))]
    public static void MapController_ExitMapView() => OWInput.ChangeInputMode(_oldInputMode);
}