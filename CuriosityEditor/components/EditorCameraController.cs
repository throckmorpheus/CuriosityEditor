using UnityEngine;

namespace CuriosityEditor.Components;

public class EditorCameraController : MonoBehaviour
{
    private Transform _target;
    public Transform Target { get => _target ?? transform.parent; set { _target = value; RecalculateTransform(); } }

    private float distance = 1f;
    private float yaw;
    private float pitch;

    public void Start() {
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;
        TargetPlayer();
    }

    public void Update() {
        if (InputManager.Inputs.PivotCamera.Down) {
            var lookRate = 10f;
            var lookDelta = InputManager.Inputs.Turn.Value * lookRate * Time.unscaledDeltaTime;

            /*if (OWInput.IsPressed(InputLibrary.rollMode)) transform.Rotate(Vector3.forward, -lookDelta.x);
            else transform.Rotate(Vector3.up, lookDelta.x);
            transform.Rotate(Vector3.right, -lookDelta.y);*/

            yaw += lookDelta.x;
            pitch -= lookDelta.y;
            
            Console.Debug(this, $"Zoom | Value: {InputManager.Inputs.Zoom.Value}, Down: {InputManager.Inputs.Zoom.Down}");
        }

        if (InputManager.Inputs.Zoom.Down) {
            var zoomRate = 0.5f;
            var zoomDelta = InputManager.Inputs.Zoom.Value * zoomRate;

            distance *= 1 + zoomDelta;

            Console.Info(this, $"Zoomed to distance of {distance}.");
        }

        RecalculateTransform();
    }

    private void RecalculateTransform() {
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = Target.position - transform.forward * distance;
    }

    public void TargetPlayer() { Target = Locator.GetPlayerCamera().transform; }
    public void TargetAstroObject(AstroObject astroObject) { Target = astroObject.gameObject.transform; }
}