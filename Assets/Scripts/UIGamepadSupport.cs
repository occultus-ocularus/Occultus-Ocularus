﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    [Tooltip("Delay before input begins repeating for up / down navigation in game menus")]
    public float navInputInitialRepeatDelay = 0.35f;
    [Tooltip("Delay between input repeats for up / down navigation in game menus")]
    public float navInputRepeatDelay = 0.15f;
    
    private float timeOfLastNavigationEvent = 0.0f;
    private bool isNavInputRepeating = false;

    [Tooltip("Input threshold (distance from 0.0) for activating navigation actions from left stick (range [-1.0, +1.0]"
             +" and dpad (values { -1.0, 0.0, +1.0 }) input")]
    public float navigationInputThreshold = 0.5f;
    
    private enum Direction { None, Up, Down }
    
    // encapsulates stateful handling of one scalar input axis, specifically raw analog to discrete with
    // simulated key repeats (which is mostly what we're interested in here)
    struct NavigationAxis {
        public Direction dir { get; private set; }
        private Direction lastDir;
        private float lastNavTime;
        private bool isRepeating;
        public NavigationAxis(int _) {
            dir = Direction.None;
            lastDir = Direction.None;
            lastNavTime = 0f;
            isRepeating = false;
        }
        private Direction toDirection(float value, float threshold) {
            return Mathf.Abs(value) < threshold ? Direction.None
                : value < 0f ? Direction.Down : Direction.Up;
        }
        public void Update(float value, float threshold, float repeatDelay, float initialRepeatDelay) {
            dir = toDirection(value, threshold);
//            Debug.Log("rd = "+repeatDelay+", ird = "+initialRepeatDelay);
            if (dir != lastDir || Time.unscaledTime >= lastNavTime + (isRepeating ? repeatDelay : initialRepeatDelay)) {
                lastNavTime = Time.unscaledTime;
                isRepeating = (dir == lastDir);
                lastDir = dir;
            } else {
                dir = Direction.None;
            }
        }
    }
    private NavigationAxis xNav = new NavigationAxis(0), yNav = new NavigationAxis(0);
    
    bool directionDiffersFromLastInput(Vector3 input, Vector3 lastInput) {
        return (input.x < 0f) != (lastInput.x < 0f) ||
               (input.x > 0f) != (lastInput.x > 0f) ||
               (input.y < 0f) != (lastInput.y < 0f) ||
               (input.y > 0f) != (lastInput.y > 0f);
    }

    private float lastInputEventTime = 0f;
    void Update() {
        var gamepad = Gamepad.current;
        if (gamepad.enabled) {
            // Handle menu navigation
            var dpadInput = new Vector3(gamepad.dpad.x.ReadValue(), gamepad.dpad.y.ReadValue(), 0f);
            var leftStickInput = new Vector3(gamepad.leftStick.x.ReadValue(), gamepad.leftStick.y.ReadValue(), 0f);
            var compositeInput = dpadInput + leftStickInput;
            xNav.Update(compositeInput.x, navigationInputThreshold, navInputRepeatDelay, navInputInitialRepeatDelay);
            yNav.Update(compositeInput.y, navigationInputThreshold, navInputRepeatDelay, navInputInitialRepeatDelay);
            if (xNav.dir != Direction.None || yNav.dir != Direction.None) {
//                Debug.Log("x nav dir: " + xNav.dir + ", y nav dir: " + yNav.dir + "after "+(Time.unscaledTime - lastInputEventTime));
                lastInputEventTime = Time.unscaledTime;
                if (!currentSelection) {
                    currentSelection = startingSelection;
                } else {
                    var nextSelection = currentSelection.FindSelectable(compositeInput);
                    if (nextSelection) currentSelection = nextSelection;
                }
                currentSelection.Select();
            }
            
            // Handle confirm
            if (gamepad.buttonSouth.wasPressedThisFrame) {
                
                // if no selection, select the default thing
                if (!currentSelection || !currentSelection.enabled) {
                    currentSelection = startingSelection;
                    currentSelection.Select();
                }
                
                // if selected thing is a button, click it
                var button = (Button) currentSelection;
                if (button) {
                    // OnSubmit does not do anything with its event data, so... this is okay. I think.
                    button.OnSubmit(null);
                }
            }
        }
    }
}
