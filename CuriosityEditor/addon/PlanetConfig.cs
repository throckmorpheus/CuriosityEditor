
using ImGuiNET;

namespace CuriosityEditor.Config;

public class PlanetConfig(ModHandle mod, string fullPath) : Config(mod, fullPath) {
    [Meta(assume: "$$filename")]
    public string name;
    [Meta(assume: "SolarSystem")]
    public string starSystem;

    public bool? isQuantumState;
    public bool? isStellarRemnant;
    [Meta(assume: true)]
    public bool? canShowOnTitle;
    public bool? destroy;
    [Meta(assume: true)]
    public bool? trackForSolarSystemRadius;
    [Meta(assume: true)]
    public bool? checkForExisting;
}