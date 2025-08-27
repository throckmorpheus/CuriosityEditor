
using System.Text.RegularExpressions;
using ImGuiNET;

namespace CuriosityEditor.Interface;

public abstract class Window
{
    private bool _visible = false;
    public bool Visible { get => _visible; set => _visible = value; }

    private string _name = null;
    public string Name { get => _name ?? DefaultName; set => _name = value; }

    private string _defaultName = null;
    private string DefaultName {
        get {
            if (_defaultName is not null) return _defaultName;
            string name = GetType().Name;
            if (name.EndsWith("Window")) name = name.Substring(0, name.Length - "Window".Length);
            _defaultName = Regex.Replace(Regex.Replace(name, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
            return _defaultName;
        }
    }

    public virtual ImGuiWindowFlags WindowConfig() { return ImGuiWindowFlags.None; }
    public virtual void Content() {}

    public void Draw() {
        if (!Visible) return;

        var windowFlags = WindowConfig();
        if (ImGui.Begin(Name, ref _visible, windowFlags)) Content();
        ImGui.End();
    }

    public void ToggleMenuItem() {
        if (ImGui.MenuItem(Name, Visible)) { Visible = !Visible; }
    }
}