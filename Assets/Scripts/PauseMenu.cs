using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

public class PauseMenu : MonoBehaviour, IUIActions
{
    //GameObject gameObject;
    [SerializeField] public PlayerInputMapping playerInput;
    // Start is called before the first frame update
    public void Awake()
    {
        playerInput.Enable();
        playerInput.UI.SetCallbacks(this);
    }
    public void Start()
    {
        
    }

    // Update is called once per frame

    public void Update()
    {
        
    }
    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
    void OnMove(InputAction.CallbackContext context){}
    void OnJump(InputAction.CallbackContext context){}
    void OnResetPlayer(InputAction.CallbackContext context){}
    void OnToggleFlying(InputAction.CallbackContext context){}
    public PlayerInputMapping uiInput;
    public LevelTransition levelTransition;
    public PlayStats playStats;
    public MainMenuController mainMenuController;

    private string currentScene;

    public void OnCancel(InputAction.CallbackContext context) {

    }

    public void OnClick(InputAction.CallbackContext context) {

    }

    public void OnNavigate(InputAction.CallbackContext context) {

    }

    public void OnOpenMenu(InputAction.CallbackContext context) {
        //if (currentScene.Equals("End Menu")) {
        //    mainMenuController.QuitGame();
        //}
        this.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnPoint(InputAction.CallbackContext context) {

    }

    public void OnSubmit(InputAction.CallbackContext context) {

    }
}
