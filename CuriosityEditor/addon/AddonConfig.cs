
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWML.Common;

namespace CuriosityEditor;

public class AddonConfig {
    private readonly IModBehaviour mod;
    public IModManifest Manifest => mod.ModHelper.Manifest;

    public string UniqueName => Manifest.UniqueName;
    public string Name => Manifest.Name;

    public readonly Dictionary<string, JObject> Planets = [];

    public AddonConfig(IModBehaviour mod) {
        this.mod = mod;

        var planetsFolderPath = $"{Manifest.ModFolderPath}/planets/";
        if (Directory.Exists(planetsFolderPath)) {
            var planetFilepaths = Directory.GetFiles(planetsFolderPath, "*.json", SearchOption.AllDirectories)
                .Select(x => x.StartsWith(planetsFolderPath) ? x.Substring(planetsFolderPath.Length) : x);
            
            foreach (var planetFilepath in Directory.GetFiles(planetsFolderPath, "*.json", SearchOption.AllDirectories)) {

                string jsonText = File.ReadAllText(planetFilepath);
                var json = JObject.Parse(jsonText);

                var planetFilepathRelative = planetFilepath.Substring(planetsFolderPath.Length);
                Planets[planetFilepathRelative] = json;
            }
        }
    }
}