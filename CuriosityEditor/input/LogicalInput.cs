using System.Linq;
using UnityEngine;

namespace CuriosityEditor;

public abstract class LogicalInput {
    internal InputBinding _binding;
}

public class LogicalBooleanInput : LogicalInput {
    public bool Down => _binding.ButtonControls.Any(x => x.isPressed);
    public bool JustPressed => _binding.ButtonControls.Any(x => x.wasPressedThisFrame);
    public bool JustReleased => _binding.ButtonControls.Any(x => x.wasReleasedThisFrame);
}

public class LogicalAxisInput : LogicalInput {
    public bool Down => _binding.ButtonControls.Any(x => x.isPressed);
    public float Value => _binding.ButtonControls.Select(x => x.pressPoint).Append(_binding.SingleAxisValue).OrderBy(x => x).FirstOrDefault();
}

public class Logical2DInput : LogicalInput {
    public Vector2 Value => _binding.DoubleAxisValue;
}