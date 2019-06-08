using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    private static PauseMenu instance;
    private bool paused = false;
    public static bool isPaused {
        get {
//            Debug.Log(""+instance+" "+instance.paused);
            return instance != null && instance.paused;
        }
    }
    private PlayerController player;
    
    private void SetInputCallbacks() {
//        playerInput.UI.SetCallbacks(this);
    }
    public void Awake() {
        Debug.Log("Awake: "+this);
        instance = this;
        SetInputCallbacks();
        player = GameObject.FindObjectsOfType<PlayerController>()[0];
    }
    public void RestartLevel() {
        foreach (var player in GameObject.FindObjectsOfType<PlayerController>()) {
            player.ResetPlayer();
        }
        Resume();
    }
    public void QuitToMenu()
    {
        Resume();
        SceneManager.LoadScene("Main Menu");
    }
    public void QuitToDesktop()
    {
        Resume();
        Application.Quit();
    }
    [FormerlySerializedAs("uiInput")] public PlayerInputMapping playerInput;
    [SerializeField] public GameObject gameMenu;
    [SerializeField] public GameObject uiSystem;
    public bool onlyShowControlsWhenMenuOpen = false;
    
    public void Update() {
        var gamepad = Gamepad.current;
        var keyboard = Keyboard.current;
        if (!gamepad.enabled) return;
        
        // start / select buttons
        if (gamepad.startButton.wasPressedThisFrame || gamepad.selectButton.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame) {
            if (isPaused) Resume();
            else Pause();
        }
        // back (B) button
        if (isPaused && gamepad.buttonEast.wasPressedThisFrame) {
            Resume();
        }
    }
    public void Pause() {
        instance = this;
        paused = true;
        gameMenu.SetActive(true);
        if (onlyShowControlsWhenMenuOpen)
            uiSystem.SetActive(true);
//        dialogSystem.SetActive(false);
        Time.timeScale = 0;
    }
    public void Resume() {
        instance = this;
        paused = false;
        gameMenu.SetActive(false);
        if (onlyShowControlsWhenMenuOpen)
            uiSystem.SetActive(false);
//        dialogSystem.SetActive(true);
        Time.timeScale = 1;
    }
    public void ToggleMenu() {
        if (gameMenu.activeInHierarchy) {
            ResumeNextFrame();
        } else {
            Pause();
        }
    }
    IEnumerable ResumeNextFrame() {
        yield return new WaitForFixedUpdate();
        Resume();
    }
    public void OnClick(InputAction.CallbackContext context) {}

    public void OnOpenMenu(InputAction.CallbackContext context) {
        if (context.performed) {
            ToggleMenu();
        }
    }

    public void OnCloseMenu(InputAction.CallbackContext context) {
//        if (context.performed && gameMenu.activeInHierarchy) {
//            ResumeNextFrame();
//        }
    }

    public void OnCancel(InputAction.CallbackContext context) {
        if (gameMenu.activeInHierarchy && context.performed) {
            ResumeNextFrame();
        }
    }

    public void OnPoint(InputAction.CallbackContext context) {
    }

    public void OnNavigate(InputAction.CallbackContext context) {}

    public void OnSubmit(InputAction.CallbackContext context) {}
}
