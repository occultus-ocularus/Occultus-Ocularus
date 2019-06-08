using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;

public class UIDisplay : MonoBehaviour {
    public PlayerInputMapping playerInput;

    private GameObject minCtrls;
    private GameObject detailedCtrls;

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
        if (Gamepad.current != null)
        {
            print(Gamepad.current);
            minCtrls = gameObject.transform.GetChild(1).GetChild(0).gameObject;
            detailedCtrls = gameObject.transform.GetChild(1).GetChild(1).gameObject;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            minCtrls = gameObject.transform.GetChild(0).GetChild(0).gameObject;
            detailedCtrls = gameObject.transform.GetChild(0).GetChild(1).gameObject;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        // Display layer switching controls if applicable to current level
        if (currentScene.path.Contains("Suburbs") && !currentScene.name.Equals("ArielIntro"))
        {
            minCtrls.transform.GetChild(1).gameObject.SetActive(false);
            minCtrls.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            minCtrls.transform.GetChild(1).gameObject.SetActive(true);
            minCtrls.transform.GetChild(2).gameObject.SetActive(false);
        }

        // Display camera zoom controls if applicable to current level
        if (currentScene.name.Equals("Outside Mall+TTD, MPT") ||
            currentScene.name.Equals("SuburbPuzzles"))

        {
            minCtrls.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            minCtrls.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    public void Start() {
        currentScene = SceneManager.GetActiveScene();
        UpdateControlsUI();
    }
    public void Update() {
        // If the last observed state of whether a gamepad was detected does not match the current state, update the
        // controls UI. Yes, we could do this with callbacks, but this is simpler and not likely to cause errors.
        if (gamepadConnected != (Gamepad.current != null)) {
            UpdateControlsUI();
        }
        
        // Switch between minimalist and detailed controls UI when Tab is pressed
        var keyboard = Keyboard.current;
        if (keyboard.tabKey.wasPressedThisFrame) {
            if (!minCtrls.activeSelf && !detailedCtrls.activeSelf || minCtrls.activeSelf && detailedCtrls.activeSelf)
            {
                minCtrls.SetActive(true);
                detailedCtrls.gameObject.SetActive(false);
            }
            else if (minCtrls.activeSelf && !detailedCtrls.activeSelf)
            {
                minCtrls.SetActive(false);
                detailedCtrls.gameObject.SetActive(true);
            }
            else if (!minCtrls.activeSelf && detailedCtrls.activeSelf)
            {
                minCtrls.SetActive(false);
                detailedCtrls.gameObject.SetActive(false);
            }
        }
    }
}
