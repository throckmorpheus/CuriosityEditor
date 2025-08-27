using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace CuriosityEditor;

public enum MouseAxis {
    Wheel,
    Horizontal,
    Vertical,
    Both
}

[Serializable]
public class InputBinding {
    public readonly List<Key> Keys = [];
    public readonly List<MouseButton> MouseButtons = [];
    public readonly List<GamepadButton> GamepadButtons = [];

    public readonly List<MouseAxis> SingleAxes = [];
    public readonly List<MouseAxis> DoubleAxes = [];

    public void CopyFrom(InputBinding binding) {
        Keys.Clear(); Keys.AddRange(binding.Keys);
        MouseButtons.Clear(); MouseButtons.AddRange(binding.MouseButtons);
    }

    public IEnumerable<ButtonControl> ButtonControls =>
        Keys.Select(key => Keyboard.current[key])
        .Concat(GamepadButtons.Select(button => Gamepad.current[button]))
        .Concat(MouseButtons.Select(button => button switch {
            MouseButton.Left => Mouse.current.leftButton, MouseButton.Right => Mouse.current.rightButton,
            MouseButton.Middle => Mouse.current.middleButton,
            MouseButton.Forward => Mouse.current.forwardButton, MouseButton.Back => Mouse.current.backButton
        }));
    
    public float SingleAxisValue => SingleAxes.Select(axis => axis switch {
        MouseAxis.Horizontal => Mouse.current.delta.right.ReadValue(),
        MouseAxis.Vertical => Mouse.current.delta.up.ReadValue(),
        MouseAxis.Wheel => Mouse.current.scroll.ReadValue().normalized.y,
        _ => 0f
    }).OrderBy(x => x).FirstOrDefault();

    public Vector2 DoubleAxisValue => DoubleAxes.Select(axis => axis switch {
        MouseAxis.Both => Mouse.current.delta.ReadValue(),
        MouseAxis.Horizontal => new(Mouse.current.delta.right.ReadValue(), 0f),
        MouseAxis.Vertical => new(0f, Mouse.current.delta.up.ReadValue()),
        MouseAxis.Wheel => Mouse.current.scroll.ReadValue().normalized,
        _ => new()
    }).OrderBy(x => x.magnitude).FirstOrDefault();
}