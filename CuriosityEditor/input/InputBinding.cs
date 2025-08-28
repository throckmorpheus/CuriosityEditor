using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace CuriosityEditor;

public enum SingleAxis {
    WheelHorizontal,
    WheelVertical,
    MouseHorizontal,
    MouseVertical
}

public enum DoubleAxis {
    Wheel,
    Mouse
}

[Serializable]
public class InputBinding {
    public bool PriorityBehindImGui = true;

    public readonly List<Key> Keys = [];
    public readonly List<MouseButton> MouseButtons = [];
    public readonly List<GamepadButton> GamepadButtons = [];

    public readonly List<SingleAxis> SingleAxes = [];
    public readonly List<DoubleAxis> DoubleAxes = [];

    public readonly List<InputBinding> Required = [];
    public readonly List<InputBinding> Disqualifying = [];

    public void Add(params Key[] keys)                          { Keys.AddRange(keys); }
    public void Add(params MouseButton[] buttons)               { MouseButtons.AddRange(buttons); }
    public void Add(params GamepadButton[] buttons)             { GamepadButtons.AddRange(buttons); }
    public void Add(params SingleAxis[] axes)                   { SingleAxes.AddRange(axes); }
    public void Add(params DoubleAxis[] axes)                   { DoubleAxes.AddRange(axes); }

    public InputBinding With(params Key[] keys)                 { Add(keys); return this; }
    public InputBinding With(params MouseButton[] buttons)      { Add(buttons); return this; }
    public InputBinding With(params GamepadButton[] buttons)    { Add(buttons); return this; }
    public InputBinding With(params SingleAxis[] axes)          { Add(axes); return this; }
    public InputBinding With(params DoubleAxis[] axes)          { Add(axes); return this; }

    public InputBinding When(InputBinding required)             { Required.Add(required); return this; }
    public InputBinding When(params Key[] keys)                 { When(From(keys)); return this; }
    public InputBinding When(params MouseButton[] buttons)      { When(From(buttons)); return this; }
    public InputBinding When(params GamepadButton[] buttons)    { When(From(buttons)); return this; }
    public InputBinding When(params SingleAxis[] axes)          { When(From(axes)); return this; }
    public InputBinding When(params DoubleAxis[] axes)          { When(From(axes)); return this; }
    
    public InputBinding WhenNot(InputBinding disqualifying)     { Disqualifying.Add(disqualifying); return this; }
    public InputBinding WhenNot(params Key[] keys)              { WhenNot(From(keys)); return this; }
    public InputBinding WhenNot(params MouseButton[] buttons)   { WhenNot(From(buttons)); return this; }
    public InputBinding WhenNot(params GamepadButton[] buttons) { WhenNot(From(buttons)); return this; }
    public InputBinding WhenNot(params SingleAxis[] axes)       { WhenNot(From(axes)); return this; }
    public InputBinding WhenNot(params DoubleAxis[] axes)       { WhenNot(From(axes)); return this; }

    public InputBinding DeferToImGui(bool defer)                { PriorityBehindImGui = defer; return this; }
    
    public static InputBinding From(params Key[] keys)              => new InputBinding().With(keys);
    public static InputBinding From(params MouseButton[] buttons)   => new InputBinding().With(buttons);
    public static InputBinding From(params GamepadButton[] buttons) => new InputBinding().With(buttons);
    public static InputBinding From(params SingleAxis[] axes)       => new InputBinding().With(axes);
    public static InputBinding From(params DoubleAxis[] axes)       => new InputBinding().With(axes);

    public void CopyFrom(InputBinding binding) {
        Keys.Clear(); Keys.AddRange(binding.Keys);
        MouseButtons.Clear(); MouseButtons.AddRange(binding.MouseButtons);
    }
    
    private bool KeyboardIsCaptured => EditorManager.InEditor && PriorityBehindImGui && ImGui.GetIO().WantCaptureKeyboard;
    private bool MouseIsCaptured    => EditorManager.InEditor && PriorityBehindImGui && ImGui.GetIO().WantCaptureMouse;

    public bool RequirementsMet => (Required.Count == 0 || Required.All(x => x.BooleanValue)) && !Disqualifying.Any(x => x.BooleanValue);

    public IEnumerable<ButtonControl> ButtonControls {
        get {
            List<ButtonControl> final = [];
            // Keyboard
            if (!KeyboardIsCaptured) final.AddRange(Keys.Select(key => Keyboard.current[key]));
            // Mouse
            if (!MouseIsCaptured) final.AddRange(MouseButtons.Select(button => button switch {
                MouseButton.Left => Mouse.current.leftButton, MouseButton.Right => Mouse.current.rightButton, MouseButton.Middle => Mouse.current.middleButton,
                MouseButton.Forward => Mouse.current.forwardButton, MouseButton.Back => Mouse.current.backButton
            }));
            // Gamepad
            final.AddRange(GamepadButtons.Select(button => Gamepad.current[button]));

            return final;
        }
    }
    
    public bool BooleanValue => RequirementsMet && ButtonControls.Any(x => x.IsPressed());
    
    public float SingleAxisValue => !RequirementsMet ? 0f :
        SingleAxes.Select(axis => axis switch {
            SingleAxis.MouseHorizontal or SingleAxis.MouseVertical
                or SingleAxis.WheelHorizontal or SingleAxis.WheelVertical when MouseIsCaptured => 0f,
            SingleAxis.MouseHorizontal => Mouse.current.delta.right.ReadValue(),
            SingleAxis.MouseVertical => Mouse.current.delta.up.ReadValue(),
            SingleAxis.WheelHorizontal => Mouse.current.scroll.ReadValue().normalized.x,
            SingleAxis.WheelVertical => Mouse.current.scroll.ReadValue().normalized.y,
            _ => 0f
        })
        .Concat(ButtonControls.Select(x => x.pressPoint))
        .OrderBy(x => x).FirstOrDefault();

    public Vector2 DoubleAxisValue => !RequirementsMet ? new Vector2() :
        DoubleAxes.Select(axis => axis switch {
            DoubleAxis.Mouse or DoubleAxis.Wheel when MouseIsCaptured => new(),
            DoubleAxis.Mouse => Mouse.current.delta.ReadValue(),
            DoubleAxis.Wheel => Mouse.current.scroll.ReadValue().normalized,
            _ => new()
        }).OrderBy(x => x.magnitude).FirstOrDefault();
}