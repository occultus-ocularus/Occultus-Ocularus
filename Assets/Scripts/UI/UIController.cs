using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Input;

public class UIController : MonoBehaviour, IUIActions {  
      
    public PlayerInputMapping uiInput;
    public LevelTransition levelTransition;
    public PlayStats playStats;
    public MainMenuController mainMenuController;

    private string currentScene;

    public void OnCancel(InputAction.CallbackContext context) {
        if (currentScene.Equals("End Menu")) {
            mainMenuController.QuitGame();
        }
    }

    public void OnClick(InputAction.CallbackContext context) {

    }

    public void OnNavigate(InputAction.CallbackContext context) {

    }

    public void OnOpenMenu(InputAction.CallbackContext context) {
        if (currentScene.Equals("End Menu")) {
            mainMenuController.QuitGame();
        }
    }

    public void OnPoint(InputAction.CallbackContext context) {

    }

    public void OnSubmit(InputAction.CallbackContext context) {
        if (currentScene.Equals("End Menu")) {
            levelTransition.FadeLoadScene();
            playStats.RestartGame();
        }
        else {
            levelTransition.FadeLoadScene();
            playStats.CheckpointReached("Main Menu");
        }

    }

    void Awake() {
        uiInput.Enable();
        uiInput.UI.SetCallbacks(this);
    }

    // Use this for initialization
    void Start() {
        currentScene = SceneManager.GetActiveScene().name;
    }
}