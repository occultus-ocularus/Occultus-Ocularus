﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour, IUIActions {

    private bool initializedInputCallbacks = false;
    public void Awake() {
        if (!initializedInputCallbacks) {
            initializedInputCallbacks = true;
            uiInput.UI.SetCallbacks(this);
        }
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
    public PlayerInputMapping uiInput;
    public GameObject gameMenu;
    public GameObject uiSystem;
    public GameObject dialogSystem;
    public PlayerController player;
    private Selectable currentSelection;
    public Selectable initialSelectable;
    public bool onlyShowControlsWhenMenuOpen = false;

    public void Pause() {
        player.OnMenuOpened();
        gameMenu.SetActive(true);
        if (onlyShowControlsWhenMenuOpen)
            uiSystem.SetActive(true);
        dialogSystem.SetActive(false);
        currentSelection = initialSelectable;
        currentSelection.Select();
        Time.timeScale = 0;
    }
    public void Resume() {
        player.OnMenuClosed();
        gameMenu.SetActive(false);
        if (onlyShowControlsWhenMenuOpen)
            uiSystem.SetActive(false);
        dialogSystem.SetActive(true);
        Time.timeScale = 1;
    }
    public void ToggleMenu() {
        if (gameMenu.activeInHierarchy) {
            Resume();
        } else {
            Pause();
        }
    }
    
    public void OnClick(InputAction.CallbackContext context) {}

    public void OnOpenMenu(InputAction.CallbackContext context) {
        if (context.performed) {
            ToggleMenu();
        }
    }

    public void OnCloseMenu(InputAction.CallbackContext context) {
//        if (context.performed && gameMenu.activeInHierarchy) {
//            Resume();
//        }
    }

    public void OnCancel(InputAction.CallbackContext context) {
        if (gameMenu.activeInHierarchy && context.performed) {
            Resume();
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
        if (gameMenu.activeInHierarchy && context.performed && currentSelection) {
            var button = (Button) currentSelection;
            if (button) { button.OnSubmit(null); }
        }
    }
}
