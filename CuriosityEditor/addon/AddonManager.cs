
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CuriosityEditor;

public class AddonManager : MonoBehaviour {
    private static AddonManager Instance;

    private readonly HashSet<AddonConfig> addons = [];
    public static ReadOnlyCollection<AddonConfig> Addons => Instance.addons.ToList().AsReadOnly();
    
    public void Start() {
        if (Instance is not null) throw new Exception($"Attempted to initialise more than one {GetType().Name}");
        Instance = this;

        ReloadAddonModConfigs();
    }

    private void ReloadAddonModConfigs() {
        addons.Clear();
        foreach (var mod in Main.Instance.ModHelper.Interaction.GetMods()) {
            if (Directory.Exists($"{mod.ModHelper.Manifest.ModFolderPath}/planets")) addons.Add(new AddonConfig(mod));
        }
    }
}