
using System.IO;
using OWML.Common;

namespace CuriosityEditor.Config;

public class ModHandle(IModBehaviour mod)
{
    private readonly IModBehaviour mod = mod;
    
    public IModManifest Manifest => mod.ModHelper.Manifest;

    public string UniqueName => Manifest.UniqueName;
    public string Name => Manifest.Name;
    public string Author => Manifest.Author;

    public string Path => Manifest.ModFolderPath;

    public bool HasPlanets => Directory.Exists($"{Path}/planets/");
}