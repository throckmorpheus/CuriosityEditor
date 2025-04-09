using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace CuriosityEditor;

[Serializable]
public class InputBinding {
    public readonly List<Key> Keys = [];
    public readonly List<MouseButton> MouseButtons = [];
    public readonly List<GamepadButton> GamepadButtons = [];

    public readonly List<SingleAxis> SingleAxes = [];
    public readonly List<DoubleAxis> DoubleAxes = [];

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
}