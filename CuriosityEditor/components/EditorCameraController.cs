using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuriosityEditor.Components;

public class EditorCameraController : MonoBehaviour
{
    public readonly Key[] ToggleKeys = [ Key.Semicolon, Key.NumpadPeriod ];
    private bool AnyPressedThisFrame(IEnumerable<Key> keys) => keys.Any(key => Keyboard.current[key].wasPressedThisFrame);

    //public void Start() => ParentToPlayer(true);

    public void Update() {
        if (AnyPressedThisFrame(ToggleKeys)) Main.InEditor = !Main.InEditor;
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