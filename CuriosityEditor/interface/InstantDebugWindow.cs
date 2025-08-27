
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using Steamworks;

namespace CuriosityEditor.Interface;

public class InstantDebugWindow : Window {
    private readonly Dictionary<string, List<string>> messages = [];
    private readonly HashSet<string> disabledKeys = [];

    public InstantDebugWindow() {
        Console.OnInstantDebugMessage += OnInstantDebugMessage;
    }
    ~InstantDebugWindow() {
        Console.OnInstantDebugMessage -= OnInstantDebugMessage;
    }

    private void OnInstantDebugMessage(string key, string message) {
        if (!messages.ContainsKey(key)) messages.Add(key, []);
        messages[key].Add(message);
    }

    public override ImGuiWindowFlags WindowConfig() {
        ImGui.SetNextWindowPos(new Vector2(5f, 20f), ImGuiCond.Appearing);
        return ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoBackground
            | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMove;
    }
    public override void Content() {
        foreach (var (key, lines) in messages) {
            bool enabled = !disabledKeys.Contains(key);
            if (key != "") {
                ImGui.SeparatorText($"{key}  ");
                ImGui.SameLine(); ImGui.SetCursorPosX(ImGui.GetCursorPosX() - ImGui.CalcTextSize("   ").X);
                ImGui.PushID($"{key}_toggle");
                if (ImGui.Checkbox("", ref enabled)) { if (enabled) disabledKeys.Remove(key); else disabledKeys.Add(key); }
                ImGui.PopID();
            }
            if (enabled) { foreach (var line in lines) { ImGui.Text(line); } }
        }
        messages.Clear();
    }
}