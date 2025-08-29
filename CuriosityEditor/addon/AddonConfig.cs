
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CuriosityEditor.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWML.Common;

namespace CuriosityEditor;

public class AddonConfig {
    public readonly ModHandle Mod;

    public readonly List<PlanetConfig> Planets = [];

    public AddonConfig(IModBehaviour mod) {
        Mod = new ModHandle(mod);

        var planetsFolderPath = $"{Mod.Path}/planets/";
        if (Directory.Exists(planetsFolderPath)) {
            var planetFilepaths = Directory.GetFiles(planetsFolderPath, "*.json", SearchOption.AllDirectories)
                .Select(x => x.StartsWith(planetsFolderPath) ? x.Substring(planetsFolderPath.Length) : x);
            
            foreach (var planetFilepath in Directory.GetFiles(planetsFolderPath, "*.json", SearchOption.AllDirectories)) {
                Planets.Add(new PlanetConfig(Mod, planetFilepath));
            }
        }
    }
}