using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;

public class UIDisplay : MonoBehaviour {
    //public PlayerInputMapping playerInput;

    private GameObject controlsOverlay;
    bool inDialogue;
    bool inCameraZone;

    private Scene currentScene;

    // Track whether a gamepad was connected last update, see UpdateControlsUI
    private bool gamepadConnected = false;

    // Call to update UI to reflect whether a gamepad is currently plugged in or not
    // If yes, displays UI for gamepad controls, and if not, displays UI for keyboard controls
    // This gets called at Start() and by Update(), but only when the current state of the attached gamepad changes.
    private void UpdateControlsUI() {
        // use this to monitor when the gamepad status changes, see Update()
        gamepadConnected = Gamepad.current != null;

        // Switch between gamepad and keyboard controls overlays depending on if a controller is connected
        if (Gamepad.current != null) {
            print(Gamepad.current);
            controlsOverlay = gameObject.transform.GetChild(1).gameObject;
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            controlsOverlay.SetActive(true);
        }
        else {
            controlsOverlay = gameObject.transform.GetChild(0).gameObject;
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            controlsOverlay.SetActive(true);
        }
        controlsOverlay.transform.GetChild(2).gameObject.SetActive(false);

        // Display layer switching controls if applicable to current level
        if (currentScene.name.Equals("5. Suburb Puzzles")) {
            controlsOverlay.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            controlsOverlay.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        else  {
            controlsOverlay.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            controlsOverlay.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
    }

    public void Start() {
        currentScene = SceneManager.GetActiveScene();
        UpdateControlsUI();

        // Hide camera controls
        controlsOverlay.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
    }
    public void Update() {
        // If the last observed state of whether a gamepad was detected does not match the current state, update the
        // controls UI. Yes, we could do this with callbacks, but this is simpler and not likely to cause errors.
        if (gamepadConnected != (Gamepad.current != null)) {
            UpdateControlsUI();

            if (inDialogue)
                ShowDialogueControls();
            else
                HideDialogueControls();

            if (inCameraZone)
                ShowCameraControls();
            else
                HideCameraControls();
        }

        // Toggle controls UI on or off when Tab is pressed
        if (Input.GetKeyDown(KeyCode.Tab)) {
            controlsOverlay.SetActive(!controlsOverlay.activeSelf);
        }
    }

    public void ShowDialogueControls() {
        controlsOverlay.transform.GetChild(2).gameObject.SetActive(true);
        controlsOverlay.transform.GetChild(1).gameObject.SetActive(false);
        inDialogue = true;

    }

    public void HideDialogueControls() {
        controlsOverlay.transform.GetChild(2).gameObject.SetActive(false);
        controlsOverlay.transform.GetChild(1).gameObject.SetActive(true);
        inDialogue = false;
    }

    public void  ShowCameraControls() {
        controlsOverlay.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        inCameraZone = true;

    }

    public void HideCameraControls() {
        controlsOverlay.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
        inCameraZone = false;
    }
}
