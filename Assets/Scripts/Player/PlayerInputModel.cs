using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.Input;
using Vector2 = UnityEngine.Vector2;

public class PlayerInputModel {
    public static PlayerInputModel instance = new PlayerInputModel();
    
    #region InputSettings
    [SerializeField] public bool useLegacyKeyboardInput { get; set; } = false;
    public enum KeyboardInputMapping {
        UseSpaceToJump,
        UseSpaceToInteract,
    }
    [SerializeField]
    public KeyboardInputMapping keyboardInputMapping { get; set; } = KeyboardInputMapping.UseSpaceToJump;
    private bool useSpaceToJump => keyboardInputMapping == KeyboardInputMapping.UseSpaceToJump;
    private bool useSpaceToInteract => keyboardInputMapping == KeyboardInputMapping.UseSpaceToInteract;
    
    #endregion
    #region PropertiesToEnableAndDisableInputActions
    public enum InputMode {
        Game,
        UI
    }
    [SerializeField] public InputMode inputMode { get; set; } = InputMode.Game;
    [SerializeField] public bool movementDisabled { get; set; } = false;
    [SerializeField] public bool jumpDisabled { get; set; } = false;
    [SerializeField] public bool interactDisabled { get; set; } = false;
    [SerializeField] public bool cameraActionsDisabled { get; set; } = false;
    [SerializeField] public bool flyDisabled { get; set; } = false;


    void Reset(InputMode mode) {
        inputMode = mode;
        movementDisabled = jumpDisabled = interactDisabled = flyDisabled = cameraActionsDisabled = false;
    }
    public bool uiInputActive => inputMode == InputMode.UI;
    public bool gameInputActive => inputMode == InputMode.Game;
    public bool movementActive => gameInputActive && !movementDisabled;
    public bool jumpActive => gameInputActive && !jumpDisabled;
    public bool interactActive => gameInputActive && !interactDisabled;
    public bool flyActive => gameInputActive && !flyDisabled;
    public bool cameraActionsActive => gameInputActive && !cameraActionsDisabled;
    
    #endregion

    public void DebugLogInput() {
        Debug.Log($"movement: {movement} (gamepad: {gamepadInputVector}), jumpPressed: {jumpPressed}, interactPressed: {interactPressed}");
    }

    public void enterUI() {
        inputMode = InputMode.UI;
    }
    public void exitUI() {
        inputMode = InputMode.Game;
    }
    
    public Vector2 movement => movementActive ? inputVector : Vector2.zero;
    public Vector2 navigation => uiInputActive ? inputVector : Vector2.zero;
    public bool jumpPressed => jumpActive && isJumpPressed;
    public bool interactPressed => interactActive && isInteractPressed;
    public bool cameraTogglePressed => cameraActionsActive && isCameraTogglePressed;
    public bool flyPressed => flyActive && isFlyPressed;
    public bool uiAcceptPressed => uiInputActive && isAcceptPressed;
    public bool uiCancelPressed => uiInputActive && isCancelPressed;
    public bool uiMenuPressed => isMenuPressed;
    
    #region MovementAndNavigation  
        
    // movement + ui navigation: wasd / arrow keys / left stick / dpad
    private Vector2 inputVector => Vector2.ClampMagnitude(
        (useLegacyKeyboardInput ? legacyInputVector : newKeyboardInputVector) +
        gamepadInputVector,
        1.0f);
    
    private Vector2 legacyInputVector => new Vector2(
        Input.GetAxis("Horizontal"),
        Input.GetAxis("Vertical"));

    private Vector2 newKeyboardInputVector => newInputWASD + newInputKeyArrows;
    private Vector2 newInputWASD => new Vector2(
        Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue(),
        Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()
    );
    private Vector2 newInputKeyArrows => new Vector2(
        Keyboard.current.rightArrowKey.ReadValue() - Keyboard.current.leftArrowKey.ReadValue(),
        Keyboard.current.upArrowKey.ReadValue() - Keyboard.current.downArrowKey.ReadValue()
    );

    private Vector2 gamepadInputVector => Gamepad.current != null
        ? Gamepad.current.leftStick.ReadValue() + Gamepad.current.dpad.ReadValue()
        : Vector2.zero;
    
    #endregion 
    #region JumpInputBindings

    // jump button (dedicated)
    // space or uparrow (depending on input mapping), south gamepad button ("A")
    private bool isJumpPressed => keyboardJumpPressed || gamepadJumpPressed;

    private bool keyboardJumpPressed =>
        useLegacyKeyboardInput ? Input.GetButton("Jump") :
        useSpaceToJump ? (Keyboard.current.spaceKey.isPressed) :
        useSpaceToInteract ? (Keyboard.current.upArrowKey.isPressed) :
        false;
    private bool gamepadJumpPressed =>
        Gamepad.current != null && Gamepad.current.buttonSouth.isPressed;
    
    #endregion
    #region InteractInputBindings

    // triggers player interaction with all objects
    // e / enter or space depending on input binding. gamepad: west button ("X")
    private bool isInteractPressed => keyboardInteractPressed || gamepadInteractPressed;
    private bool keyboardInteractPressed =>
        useLegacyKeyboardInput ? Input.GetButton("Interact") :
        useSpaceToJump ? (Keyboard.current.eKey.isPressed || Keyboard.current.enterKey.isPressed) :
        useSpaceToInteract ? (Keyboard.current.spaceKey.isPressed) :
        false;
    private bool gamepadInteractPressed =>
        Gamepad.current != null && Gamepad.current.buttonWest.isPressed;
    
    #endregion
    #region CameraInputBindings

    // toggles camera (new input binding, distinct from directly setting the camera mode)
    // 't' key / right trigger. '1' effectively does the same thing
    private bool isCameraTogglePressed => keyboardCameraTogglePressed || gamepadCameraTogglePressed;
    private bool keyboardCameraTogglePressed => useLegacyKeyboardInput 
        ? Input.GetButton("ToggleCamera") 
        : Keyboard.current.tKey.isPressed;
    private bool gamepadCameraTogglePressed =>
        Gamepad.current != null && Gamepad.current.rightTrigger.isPressed;
    
    #endregion
    #region FlyInputBindings
    
    // toggles fly / noclip mode
    // right shift key. Gamepad: north button ("Y" / triangle)

    private bool isFlyPressed => keyboardFlyPressed || gamepadFlyPressed;
    private bool keyboardFlyPressed => useLegacyKeyboardInput
        ? Input.GetButton("ToggleFly")
        : Keyboard.current.rightShiftKey.isPressed;
    private bool gamepadFlyPressed =>
        Gamepad.current != null && Gamepad.current.buttonNorth.isPressed;

    #endregion
    #region UIAcceptBindings
    
    // button to accept (press buttons) in the UI, and advance / skip through dialog
    // set as 'space', 'enter'. Gamepad: south button ("A")

    private bool isAcceptPressed => keyboardAcceptPressed || gamepadAcceptPressed;
    private bool keyboardAcceptPressed => useLegacyKeyboardInput
        ? Input.GetKey("Space") || Input.GetKey("Enter")
        : Keyboard.current.spaceKey.isPressed || Keyboard.current.enterKey.isPressed;
    private bool gamepadAcceptPressed =>
        Gamepad.current != null && Gamepad.current.buttonSouth.isPressed;
    
    #endregion
    #region UICancelBindings
    
    // button to cancel / go back when navigating through the UI (currently unused)
    // keyboard: 'esc', gamepad: east button ("B")

    private bool isCancelPressed => keyboardCancelPressed || gamepadCancelPressed;

    private bool keyboardCancelPressed => useLegacyKeyboardInput
        ? Input.GetKey("Escape")
        : Keyboard.current.escapeKey.isPressed;
    private bool gamepadCancelPressed =>
        Gamepad.current != null && Gamepad.current.buttonEast.isPressed;

    #endregion
    #region UIMenuBindings
    
    // button to open (and toggle) the game menu
    // 'esc' (same as ui-cancel) on keyboard. gamepad: 'select'
    private bool isMenuPressed => keyboardMenuPressed || gamepadMenuPressed;

    private bool keyboardMenuPressed => useLegacyKeyboardInput
        ? Input.GetKey("Escape")
        : Keyboard.current.escapeKey.isPressed;
    private bool gamepadMenuPressed =>
        Gamepad.current != null && Gamepad.current.selectButton.isPressed;
    
    #endregion
}