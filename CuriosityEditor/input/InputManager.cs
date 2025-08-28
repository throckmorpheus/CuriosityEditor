using System;
using System.Collections.Generic;
using System.Reflection;
using ImGuiNET;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace CuriosityEditor;

public class Inputs {
    public static LogicalBooleanInput ToggleEditor;
    public static LogicalAxisInput Zoom;
    public static Logical2DInput Pan;
    public static Logical2DInput Pivot;
}

public class InputManager : MonoBehaviour
{
    private static InputManager Instance;

    public void Start() {
        if (Instance is not null) throw new Exception($"Attempted to initialise more than one {GetType().Name}");
        Instance = this;

        foreach (var member in typeof(Inputs).GetFields()) {
            var binding = new InputBinding();
            var logicalInput = (LogicalInput)Activator.CreateInstance(member.FieldType);
            logicalInput._binding = binding;
            member.SetValue(null, logicalInput);
        }

        if (!LoadInputConfig()) SetupDefaultInputConfig();
    }

    public InputBinding GetBinding(LogicalInput input) => input._binding;
    public void SetBinding(LogicalInput input, InputBinding binding) => input._binding = binding;

    public void SetupDefaultInputConfig() {
        Inputs.ToggleEditor._binding = InputBinding.From(Key.Semicolon).DeferToImGui(false);
        
        // Camera controls
        Inputs.Zoom._binding         = InputBinding.From(SingleAxis.WheelVertical);

        var shiftBinding             = InputBinding.From(Key.LeftShift, Key.RightShift).DeferToImGui(false);
        Inputs.Pivot._binding        = InputBinding.From(DoubleAxis.Mouse).When(MouseButton.Middle).WhenNot(shiftBinding);
        Inputs.Pan._binding          = InputBinding.From(DoubleAxis.Mouse).When(MouseButton.Middle).When(shiftBinding);
        Console.Info(this, "Loaded default input config.");
    }

    public bool LoadInputConfig() {
        var inputConfig = Main.Instance.ModHelper.Storage.Load<Dictionary<string, InputBinding>>("input_config.json");
        if (inputConfig is null) return false;

        foreach (var member in typeof(Inputs).GetFields(BindingFlags.Static)) {
            if (inputConfig.TryGetValue(member.Name, out var configBinding)) {
                ((LogicalInput)member.GetValue(null))._binding = configBinding;
            }
        }

        Console.Info(this, "Loaded input config.");
        return true;
    }

    public void SaveInputConfig() {
        Dictionary<string, InputBinding> inputConfig = [];
        foreach (var member in typeof(Inputs).GetFields(BindingFlags.Static)) {
            inputConfig.Add(member.Name, ((LogicalInput)member.GetValue(null))._binding);
        }
        Main.Instance.ModHelper.Storage.Save(inputConfig, "input_config.json");
        Console.Info(this, "Saved input config.");
    }
}