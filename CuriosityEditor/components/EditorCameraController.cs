using UnityEngine;

namespace CuriosityEditor.Components;

public class EditorCameraController : MonoBehaviour
{
    private Transform _target;
    public Transform Target { get => _target ?? transform.parent; set { _target = value; RecalculateTransform(); } }

    private Vector3 _offset;
    public Vector3 Offset { get => _offset; set { _offset = value; RecalculateTransform(); } }

    private float distance = 1f;
    private float yaw;
    private float pitch;

    private readonly float lookRate = 10f;
    private readonly float zoomRate = 0.5f;
    private readonly float zoomMin = 1f;

    public void Start() {
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;
        TargetPlayer();
    }

    public void Update() {
        if (Inputs.Pivot.Down) {
            var lookDelta = Inputs.Pivot.Value * lookRate * Time.unscaledDeltaTime;

            /*if (OWInput.IsPressed(InputLibrary.rollMode)) transform.Rotate(Vector3.forward, -lookDelta.x);
            else transform.Rotate(Vector3.up, lookDelta.x);
            transform.Rotate(Vector3.right, -lookDelta.y);*/
            
            pitch -= lookDelta.y;
            pitch = Mathf.Repeat(pitch, 360f);

            bool isUpsideDown = pitch > 90f && pitch < 270f;
            if (isUpsideDown) yaw -= lookDelta.x; else yaw += lookDelta.x;
            yaw = Mathf.Repeat(yaw, 360f);
        }

        if (Inputs.Pan.Down) {

        }

        if (Inputs.Zoom.Down) {
            var zoomDelta = Inputs.Zoom.Value * zoomRate;

            distance *= 1 - zoomDelta;
            distance = Mathf.Max(distance, zoomMin);
        }

        Console.Instant.Debug(this, $"Yaw:   {yaw}");
        Console.Instant.Debug(this, $"Pitch: {pitch}");
        Console.Instant.Debug(this, $"Dist.: {distance}");

        RecalculateTransform();
    }

    private void RecalculateTransform() {
        transform.rotation = Quaternion.LookRotation(Target.forward, Target.up) * Quaternion.Euler(pitch, yaw, 0);
        transform.position = Target.position + Offset - transform.forward * distance;
    }

    public void TargetPlayer() { Target = Locator.GetPlayerCamera().transform; }
    public void TargetAstroObject(AstroObject astroObject) { Target = astroObject.gameObject.transform; }
}