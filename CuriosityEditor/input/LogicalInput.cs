using System.Linq;
using UnityEngine;

namespace CuriosityEditor;

public abstract class LogicalInput {
    internal InputBinding _binding;
    public abstract bool Down { get; }
}

public class LogicalBooleanInput : LogicalInput {
    public override bool Down => _binding.BooleanValue;
    public bool JustPressed => _binding.ButtonControls.Any(x => x.wasPressedThisFrame);
    public bool JustReleased => _binding.ButtonControls.Any(x => x.wasReleasedThisFrame);
}

public class LogicalAxisInput : LogicalInput {
    public override bool Down => _binding.BooleanValue || Mathf.Abs(_binding.SingleAxisValue) > 0f;
    public float Value => _binding.SingleAxisValue;
}

public class Logical2DInput : LogicalInput {
    public override bool Down => _binding.DoubleAxisValue.magnitude > 0f;
    public Vector2 Value => _binding.DoubleAxisValue;
}