using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

// Polyfill for missing gamepad support in UI menus, etc,
// implemented using the new experimental input support.
// Note: this expects legacy input to be turned off!!!
// If UI ever gets full gamepad suppport, remove this
public class UIGamepadSupport : MonoBehaviour {
    [Tooltip("Element to select iff none selected")]
    public Selectable startingSelection;
    private Selectable currentSelection;

    [Tooltip("Delay before input is repeated for up / down navigation in game menus")]
    public float repeatingInputDelay = 0.15f;
    private float timeOfLastNavigationEvent = 0.0f;

    [Tooltip("Input threshold (distance from 0.0) for activating navigation actions from left stick (range [-1.0, +1.0]"
             +" and dpad (values { -1.0, 0.0, +1.0 }) input")]
    public float navigationInputThreshold = 0.1f;

    void Update() {
        var gamepad = Gamepad.current;
        if (gamepad.enabled) {
            // Handle menu navigation
            var dpadInput = new Vector3(gamepad.dpad.x.ReadValue(), gamepad.dpad.y.ReadValue(), 0f);
            var leftStickInput = new Vector3(gamepad.leftStick.x.ReadValue(), gamepad.leftStick.y.ReadValue(), 0f);
            var compositeInput = dpadInput + leftStickInput;
            if (compositeInput.magnitude > navigationInputThreshold &&
                Time.unscaledTime > timeOfLastNavigationEvent + repeatingInputDelay
            ) {
                Debug.Log("directional nav input in game menu! " + compositeInput);
                timeOfLastNavigationEvent = Time.unscaledTime;
                
                // if nothing selected, select the starting item
                if (!currentSelection) {
                    currentSelection = startingSelection;
                    
                // otherwise, try selecting something in the given direction, iff this returns a selectable
                } else {
                    var nextSelection = currentSelection.FindSelectable(compositeInput);
                    if (nextSelection) currentSelection = nextSelection;
                }
                
                // select the current (updated) selection
                currentSelection.Select();
            }
            
            // Handle confirm
            if (gamepad.buttonSouth.wasPressedThisFrame) {
                var button = (Button) currentSelection;
                if (button) {
                    // OnSubmit does not do anything with its event data, so... this is okay. I think.
                    button.OnSubmit(null);
                }
            }
        }
    }
}
