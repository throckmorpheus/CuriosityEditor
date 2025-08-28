
using ImGuiNET;

namespace CuriosityEditor.Interface;

public class ModWindow : Window {
    public override ImGuiWindowFlags WindowConfig() {
        return ImGuiWindowFlags.None;
    }

    public override void Content() {
        foreach (var addon in AddonManager.Addons) {
            ImGui.PushID(addon.UniqueName);
            if (ImGui.CollapsingHeader($"{addon.Name} v{addon.Manifest.Version} by {addon.Manifest.Author}")) {
                if (addon.Planets.Count == 0) ImGui.Text("Contains no planets.");
                foreach (var (path, obj) in addon.Planets) {
                    if (ImGui.CollapsingHeader(path)) {
                        ImGui.Text(obj.ToString());
                    }
                }
            }
            ImGui.PopID();
        }
    }
}