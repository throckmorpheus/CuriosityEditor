
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OWML.Common;

namespace CuriosityEditor;

public class AddonConfig {
    private readonly IModBehaviour mod;
    public IModManifest Manifest => mod.ModHelper.Manifest;

    public string Name => Manifest.UniqueName;

    public readonly Dictionary<string, string> Planets = [];

    public AddonConfig(IModBehaviour mod) {
        this.mod = mod;

        var planetsFolderPath = $"{Manifest.ModFolderPath}/planets/";
        if (Directory.Exists(planetsFolderPath)) {
            var planetFilepaths = Directory.GetFiles(planetsFolderPath, "*.json", SearchOption.AllDirectories)
                .Select(x => x.StartsWith(planetsFolderPath) ? x.Substring(planetsFolderPath.Length) : x);
            
            foreach (var planetFilepath in Directory.GetFiles(planetsFolderPath, "*.json", SearchOption.AllDirectories)) {
                var planetFilepathRelative = planetFilepath.Substring(planetsFolderPath.Length);
                Planets[planetFilepathRelative] = File.ReadAllText(planetFilepath);
            }
        }
    }
}