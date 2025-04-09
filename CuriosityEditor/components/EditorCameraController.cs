using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuriosityEditor.Components;

public class EditorCameraController : MonoBehaviour
{
    //public void Start() => ParentToPlayer(true);

    public void Update() {
        if (InputManager.Inputs.ToggleEditor.JustPressed) Main.InEditor = !Main.InEditor;

        if (!Main.InEditor) return;

        var lookRate = OWInput.UsingGamepad() ? PlayerCameraController.GAMEPAD_LOOK_RATE_Y : PlayerCameraController.LOOK_RATE;

        //var lookDelta = OWInput.GetAxisValue(InputLibrary.look, InputMode.All) * lookRate * Time.unscaledDeltaTime;
        var lookDelta = InputManager.Inputs.Turn.Value * lookRate * Time.unscaledDeltaTime;

        if (OWInput.IsPressed(InputLibrary.rollMode)) transform.Rotate(Vector3.forward, -lookDelta.x);
        else transform.Rotate(Vector3.up, lookDelta.x);
        transform.Rotate(Vector3.right, -lookDelta.y);
    }

    public void ParentToPlayer(bool warp = false) {
        var playerCameraTransform = Locator.GetPlayerCamera().transform;
        transform.parent = playerCameraTransform;
        if (warp) {
            transform.position = playerCameraTransform.position;
            transform.rotation = playerCameraTransform.rotation;
        }
    }

    public void ParentToAstroObject(AstroObject astroObject, bool warp = false) {
        var astroObjectTransform = astroObject.gameObject.transform;
        transform.parent = astroObjectTransform;
        if (warp) {
            transform.position = astroObjectTransform.position;
        }
    }
}