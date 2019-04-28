﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;

public class UIDisplay : MonoBehaviour {
    public PlayerInputMapping playerInput;

    private GameObject minCtrls;
    private GameObject detailedCtrls;
    private bool usingGamepadUI = false;
    
    // Start is called before the first frame update
    void Start() {
        UpdateGamepadOrKeyboardControls();
    }
    private void UpdateGamepadOrKeyboardControls () {
        // Switch between gamepad and keyboard controls overlays depending on if a controller is connected
        usingGamepadUI = Gamepad.current != null;
        if (Gamepad.current != null) {
            minCtrls = gameObject.transform.GetChild(1).GetChild(0).gameObject;
            detailedCtrls = gameObject.transform.GetChild(1).GetChild(1).gameObject;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else {
            minCtrls = gameObject.transform.GetChild(0).GetChild(0).gameObject;
            detailedCtrls = gameObject.transform.GetChild(0).GetChild(1).gameObject;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        // Turn on layer switching controls if applicable to current level
        if (SceneManager.GetActiveScene().path.Contains("2nd Area - Suburbs")) {
            minCtrls.transform.GetChild(1).gameObject.SetActive(false);
            minCtrls.transform.GetChild(2).gameObject.SetActive(true);
        }
        else {
            minCtrls.transform.GetChild(1).gameObject.SetActive(true);
            minCtrls.transform.GetChild(2).gameObject.SetActive(false);
        }

        // Turn on camera zoom controls if applicable to current level
        if (SceneManager.GetActiveScene().name.Equals("TwoLaserDoorLayer") ||
            SceneManager.GetActiveScene().name.Equals("Outside Mall+TTD, MPT")) {
            minCtrls.transform.GetChild(3).gameObject.SetActive(true);
        }
        else {
            minCtrls.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (usingGamepadUI != (Gamepad.current != null)) {
            UpdateGamepadOrKeyboardControls();
        }

        // Switch between minimalist and detailed controls UI when Tab is pressed
        if (Input.GetKeyDown(KeyCode.Tab)) {

            if (!minCtrls.activeSelf && !detailedCtrls.activeSelf || minCtrls.activeSelf && detailedCtrls.activeSelf) {
                minCtrls.SetActive(true);
                detailedCtrls.gameObject.SetActive(false);
            }
            else if (minCtrls.activeSelf && !detailedCtrls.activeSelf) {
                minCtrls.SetActive(false);
                detailedCtrls.gameObject.SetActive(true);
            }
            else if (!minCtrls.activeSelf && detailedCtrls.activeSelf) {
                minCtrls.SetActive(false);
                detailedCtrls.gameObject.SetActive(false);
            }
        }
    }
}
