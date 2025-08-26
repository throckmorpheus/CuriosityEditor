using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuriosityEditor.Components;

public class EditorCameraController : MonoBehaviour
{
    //public void Start() => ParentToPlayer(true);

    public void Update() {
        var lookRate = 10f;
        var lookDelta = InputManager.Inputs.Turn.Value * lookRate * Time.unscaledDeltaTime;

        if (InputManager.Inputs.PivotCamera.Down) {

        }

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