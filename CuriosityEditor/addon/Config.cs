
using System;
using System.IO;
using System.Reflection;
using ImGuiNET;
using Newtonsoft.Json.Linq;

namespace CuriosityEditor.Config;

public abstract class Config {
    protected ModHandle Mod { get; private set; }
    private string FullPath { get; set; }
    public string RelativePath { get; private set; }
    
    public Config(ModHandle mod, string fullPath) {
        Mod = mod;
        FullPath = fullPath;
        RelativePath = fullPath.StripFront(Mod.Path).StripFront("/").StripBack(".json");

        Load(File.ReadAllText(fullPath));
    }

    public void Load(string jsonString) {
        var json = JObject.Parse(jsonString);
        foreach (var prop in GetType().GetFields()) {
            if (prop.GetCustomAttribute<MetaAttribute>() is MetaAttribute meta && meta.Ignore) continue;
            if (json.TryGetValue(prop.Name, out var val)) prop.SetValue(this, Convert.ChangeType(val, prop.FieldType));
        }
    }

    public virtual void ImDraw() {
        int columnsCountRestore = ImGui.GetColumnsCount();
        ImGui.Columns(2);
        foreach (var prop in GetType().GetFields()) {
            string displayName = null;
            string nullMessage = "null";
            if (prop.GetCustomAttribute<MetaAttribute>() is MetaAttribute meta) {
                if (meta.Ignore) continue;
                displayName ??= HandleMetaString(meta.DisplayName);
                if (meta.Assumption is not null) nullMessage = $"null ({HandleMetaString(meta.Assumption.ToString())})";
            }
            displayName ??= prop.Name;

            ImGui.Text(displayName);
            ImGui.NextColumn();
            if (prop.GetValue(this) is object val) ImGui.Text(val.ToString()); else ImGui.TextDisabled(nullMessage); 
            ImGui.NextColumn();
        }
        ImGui.Columns(columnsCountRestore);
    }

    protected string HandleMetaString(string metaString) => metaString switch {
        "$$filename" => Path.GetFileNameWithoutExtension(RelativePath),
        _ => metaString
    };
}