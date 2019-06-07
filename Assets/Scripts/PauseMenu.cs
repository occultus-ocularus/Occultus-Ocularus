using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

public class PauseMenu : MonoBehaviour, IUIActions
{    
    // Start is called before the first frame update
    public void Awake()
    {
        uiInput.Enable();
        uiInput.UI.SetCallbacks(this);
    }
    public void Start()
    {
        
    }

    // Update is called once per frame

    public void Update()
    {
        
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
    public LevelTransition levelTransition;
    public PlayStats playStats;
    public MainMenuController mainMenuController;
    public GameObject gameMenu;

    private string currentScene;

    public void Pause() {
        gameMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume() {
        gameMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void ToggleMenu() {
        if (gameMenu.activeInHierarchy) {
            Resume();
        } else {
            Pause();
        }
    }
    
    public void OnClick(InputAction.CallbackContext context) {
        throw new System.NotImplementedException();
    }

    public void OnOpenMenu(InputAction.CallbackContext context) {
        if (context.performed) {
            ToggleMenu();
        }
    }

    public void OnCancel(InputAction.CallbackContext context) {
        throw new System.NotImplementedException();
    }

    public void OnPoint(InputAction.CallbackContext context) {
        
    }

    public void OnNavigate(InputAction.CallbackContext context) {
        throw new System.NotImplementedException();
    }

    public void OnSubmit(InputAction.CallbackContext context) {

    }
}
