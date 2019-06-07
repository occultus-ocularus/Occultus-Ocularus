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

public class PauseMenu : MonoBehaviour, IUIActions {
    private static PauseMenu instance;
    private bool paused = false;
    public static bool isPaused {
        get {
//            Debug.Log(""+instance+" "+instance.paused);
            return instance != null && instance.paused;
        }
    }

    private PauseMenu currentCallbackInstance = null;
    private void SetInputCallbacks() {
        if (currentCallbackInstance != this) {
            currentCallbackInstance = this;
            playerInput.UI.SetCallbacks(this);
        }
    }
    private void ClearInputCallbacks() {
        if (currentCallbackInstance == this) {
            currentCallbackInstance = null;
            playerInput.UI.SetCallbacks(null);
        }   
    }
    public void Awake() {
        instance = this;
        SetInputCallbacks();
    }
    public void OnDestroy() {
        ClearInputCallbacks();
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
    public GameObject gameMenu;
    public GameObject uiSystem;
    private Selectable currentSelection;
    public Selectable initialSelectable;
    public bool onlyShowControlsWhenMenuOpen = false;

    public void Pause() {
        instance = this;
        paused = true;
//        Debug.Log("paused: "+isPaused);
        gameMenu.SetActive(true);
        if (onlyShowControlsWhenMenuOpen)
            uiSystem.SetActive(true);
        
//        dialogSystem.SetActive(false);
        currentSelection = initialSelectable;
        currentSelection.Select();
        Time.timeScale = 0;
    }
    public void Resume() {
        instance = this;
        paused = false;
//        Debug.Log("resumed: "+isPaused);
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

    public void OnNavigate(InputAction.CallbackContext context) {
        if (gameMenu.activeInHierarchy && context.performed)
        {
            if (currentSelection == null) {
                currentSelection = initialSelectable;
                currentSelection.Select();
                return;
            }
            var input = context.ReadValue<Vector2>();
            var s = currentSelection.FindSelectable(new Vector3(input.x, input.y, 0.0f));
            if (s) {
                currentSelection = s;
            }
            currentSelection.Select();
        }
    }

    public void OnSubmit(InputAction.CallbackContext context) {
        if (context.performed)
//            Debug.Log("Submit? "+gameMenu.activeInHierarchy+" "+currentSelection);
      
        if (gameMenu.activeInHierarchy && context.performed && currentSelection) {
            var button = (Button) currentSelection;
//            Debug.Log("Submit! "+button+" "+button.IsActive());
            if (button) { button.OnSubmit(null); }
        }
    }
}
