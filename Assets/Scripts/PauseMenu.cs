using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;
using UnityEngine.UI;

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
    public GameObject uiSystem;
    public GameObject dialogSystem;
    private Selectable currentSelection;
    public Selectable initialSelectable;
    

    public Button[] menuButtons;
    private int activeMenuButton = 0;
    
    private string currentScene;

    public void Pause() {
//        currentSelection = Selectable.allSelectablesArray[0];
        currentSelection = null;
        gameMenu.SetActive(true);
        uiSystem.SetActive(true);
        dialogSystem.SetActive(false);
        Time.timeScale = 0;
    }
    public void Resume() {
        gameMenu.SetActive(false);
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
                currentSelection.Select();
            }
        }
    }

    public void OnSubmit(InputAction.CallbackContext context) {
        if (gameMenu.activeInHierarchy && context.performed && currentSelection) {
            var button = (Button) currentSelection;
            if (button) { button.OnSubmit(null); }
        }
    }
}
