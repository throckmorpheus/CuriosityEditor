
using ImGuiNET;
namespace CuriosityEditor.Interface;

public class ModWindow : Window {
    public override ImGuiWindowFlags WindowConfig() {
        return ImGuiWindowFlags.None;
    }

    public override void Content() {
        foreach (var addon in AddonManager.Addons) {
            ImGui.PushID(addon.Mod.UniqueName);
            if (ImGui.CollapsingHeader($"{addon.Mod.Name} v{addon.Mod.Manifest.Version} by {addon.Mod.Author}")) {
                ImGui.Indent(10f);
                if (addon.Planets.Count > 0 && ImGui.CollapsingHeader("Planets")) {
                    ImGui.Indent(10f);
                    foreach (var planet in addon.Planets) {
                        if (ImGui.CollapsingHeader(planet.RelativePath.StripFront("planets/"))) {
                            ImGui.Indent(10f);
                            planet.ImDraw();
                            ImGui.Unindent(10f);
                        }
                    }
                    ImGui.Unindent(10f);
                }
                ImGui.Unindent(10f);
            }
            ImGui.PopID();
        }
    }
}