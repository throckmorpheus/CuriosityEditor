using System;
using System.Collections.Generic;
using System.Linq;
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
    public InputBinding When(params Key[] keys)                 { When(new InputBinding().With(keys)); return this; }
    public InputBinding When(params MouseButton[] buttons)      { When(new InputBinding().With(buttons)); return this; }
    public InputBinding When(params GamepadButton[] buttons)    { When(new InputBinding().With(buttons)); return this; }
    public InputBinding When(params SingleAxis[] axes)          { When(new InputBinding().With(axes)); return this; }
    public InputBinding When(params DoubleAxis[] axes)          { When(new InputBinding().With(axes)); return this; }
    
    public InputBinding WhenNot(InputBinding disqualifying)     { Disqualifying.Add(disqualifying); return this; }
    public InputBinding WhenNot(params Key[] keys)              { WhenNot(new InputBinding().With(keys)); return this; }
    public InputBinding WhenNot(params MouseButton[] buttons)   { WhenNot(new InputBinding().With(buttons)); return this; }
    public InputBinding WhenNot(params GamepadButton[] buttons) { WhenNot(new InputBinding().With(buttons)); return this; }
    public InputBinding WhenNot(params SingleAxis[] axes)       { WhenNot(new InputBinding().With(axes)); return this; }
    public InputBinding WhenNot(params DoubleAxis[] axes)       { WhenNot(new InputBinding().With(axes)); return this; }

    public void CopyFrom(InputBinding binding) {
        Keys.Clear(); Keys.AddRange(binding.Keys);
        MouseButtons.Clear(); MouseButtons.AddRange(binding.MouseButtons);
    }

    public bool RequirementsMet => Required.Count == 0 || Required.All(x => x.BooleanValue) && !Disqualifying.Any(x => x.BooleanValue);

    public IEnumerable<ButtonControl> ButtonControls =>
        Keys.Select(key => Keyboard.current[key])
        .Concat(GamepadButtons.Select(button => Gamepad.current[button]))
        .Concat(MouseButtons.Select(button => button switch {
            MouseButton.Left => Mouse.current.leftButton, MouseButton.Right => Mouse.current.rightButton,
            MouseButton.Middle => Mouse.current.middleButton,
            MouseButton.Forward => Mouse.current.forwardButton, MouseButton.Back => Mouse.current.backButton
        }));
    
    public bool BooleanValue => RequirementsMet && ButtonControls.Any(x => x.IsPressed());
    
    public float SingleAxisValue => !RequirementsMet ? 0f :
        SingleAxes.Select(axis => axis switch {
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
            DoubleAxis.Mouse => Mouse.current.delta.ReadValue(),
            DoubleAxis.Wheel => Mouse.current.scroll.ReadValue().normalized,
            _ => new()
        }).OrderBy(x => x.magnitude).FirstOrDefault();
}