using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace CuriosityEditor;

public class InputManager : MonoBehaviour
{
    public class Inputs {
        public static LogicalBooleanInput ToggleEditor;
        public static LogicalBooleanInput PivotCamera;
        public static LogicalAxisInput Zoom;
        public static Logical2DInput Pan;
        public static Logical2DInput Turn;
    }

    private InputManager Instance;

    public void Start() {
        if (Instance is not null) throw new Exception("Attempted to initialise more than one InputManager");
        Instance = this;
		Main.Console.Info("Initialised Input Manager.");

        foreach (var member in typeof(Inputs).GetFields()) {
            var binding = new InputBinding();
            var logicalInput = (LogicalInput)Activator.CreateInstance(member.FieldType);
            logicalInput._binding = binding;
            member.SetValue(null, logicalInput);
            _bindings.Add(logicalInput, binding);
            Main.Console.Info($"Created input '{member.Name}'.");
        }

        if (!LoadInputConfig()) SetupDefaultInputConfig();
    }

    private readonly Dictionary<LogicalInput, InputBinding> _bindings = [];

    public void SetupDefaultInputConfig() {
        _bindings[Inputs.ToggleEditor].Keys.Add(Key.Semicolon);
        _bindings[Inputs.PivotCamera].MouseButtons.Add(MouseButton.Middle);
        _bindings[Inputs.Pan].DoubleAxes.Add(MouseAxis.mouse);
        _bindings[Inputs.Turn].DoubleAxes.Add(MouseAxis.mouse);
        _bindings[Inputs.Zoom].SingleAxes.Add(MouseAxis.mouseWheel);
        Main.Console.Info("Loaded default input config.");
    }

    public bool LoadInputConfig() {
        var inputConfig = Main.Instance.ModHelper.Storage.Load<Dictionary<string, InputBinding>>("input_config.json");
        if (inputConfig is null) return false;

        foreach (var member in typeof(Inputs).GetFields(BindingFlags.Static)) {
            if (inputConfig.TryGetValue(member.Name, out var configBinding)) {
                var binding = _bindings[(LogicalInput)member.GetValue(null)];
                binding.CopyFrom(configBinding);
            }
        }

        Main.Console.Info("Loaded input config.");
        return true;
    }

    public void SaveInputConfig() {
        Dictionary<string, InputBinding> inputConfig = [];
        foreach (var member in typeof(Inputs).GetFields(BindingFlags.Static)) {
            inputConfig.Add(member.Name, _bindings[(LogicalInput)member.GetValue(null)]);
        }
        Main.Instance.ModHelper.Storage.Save(inputConfig, "input_config.json");
        Main.Console.Info("Saved input config.");
    }
}