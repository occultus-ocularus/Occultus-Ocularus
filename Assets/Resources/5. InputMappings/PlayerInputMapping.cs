// GENERATED AUTOMATICALLY FROM 'Assets/Resources/InputMappings/PlayerInputMapping.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class PlayerInputMapping : InputActionAssetReference
{
    public PlayerInputMapping()
    {
    }
    public PlayerInputMapping(InputActionAsset asset)
        : base(asset)
    {
    }
    [NonSerialized] private bool m_Initialized;
    private void Initialize()
    {
        // Player
        m_Player = asset.GetActionMap("Player");
        m_Player_Move = m_Player.GetAction("Move");
        m_Player_Jump = m_Player.GetAction("Jump");
        m_Player_ResetPlayer = m_Player.GetAction("ResetPlayer");
        m_Player_ToggleFlying = m_Player.GetAction("ToggleFlying");
        // UI
        m_UI = asset.GetActionMap("UI");
        m_UI_Navigate = m_UI.GetAction("Navigate");
        m_UI_Submit = m_UI.GetAction("Submit");
        m_UI_Cancel = m_UI.GetAction("Cancel");
        m_UI_Point = m_UI.GetAction("Point");
        m_UI_Click = m_UI.GetAction("Click");
        m_UI_OpenMenu = m_UI.GetAction("OpenMenu");
        // Camera
        m_Camera = asset.GetActionMap("Camera");
        m_Camera_ToggleCamera = m_Camera.GetAction("ToggleCamera");
        m_Camera_SetCameraMode1 = m_Camera.GetAction("SetCameraMode1");
        m_Camera_SetCameraMode2 = m_Camera.GetAction("SetCameraMode2");
        m_Camera_SetCameraMode3 = m_Camera.GetAction("SetCameraMode3");
        m_Camera_SetCameraMode4 = m_Camera.GetAction("SetCameraMode4");
        // Interaction
        m_Interaction = asset.GetActionMap("Interaction");
        m_Interaction_Interact = m_Interaction.GetAction("Interact");
        // LayerSwitching
        m_LayerSwitching = asset.GetActionMap("LayerSwitching");
        m_LayerSwitching_ToggleLayerSwitching = m_LayerSwitching.GetAction("ToggleLayerSwitching");
        // Dialog
        m_Dialog = asset.GetActionMap("Dialog");
        m_Dialog_Next = m_Dialog.GetAction("Next");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        if (m_PlayerActionsCallbackInterface != null)
        {
            Player.SetCallbacks(null);
        }
        m_Player = null;
        m_Player_Move = null;
        m_Player_Jump = null;
        m_Player_ResetPlayer = null;
        m_Player_ToggleFlying = null;
        if (m_UIActionsCallbackInterface != null)
        {
            UI.SetCallbacks(null);
        }
        m_UI = null;
        m_UI_Navigate = null;
        m_UI_Submit = null;
        m_UI_Cancel = null;
        m_UI_Point = null;
        m_UI_Click = null;
        m_UI_OpenMenu = null;
        if (m_CameraActionsCallbackInterface != null)
        {
            Camera.SetCallbacks(null);
        }
        m_Camera = null;
        m_Camera_ToggleCamera = null;
        m_Camera_SetCameraMode1 = null;
        m_Camera_SetCameraMode2 = null;
        m_Camera_SetCameraMode3 = null;
        m_Camera_SetCameraMode4 = null;
        if (m_InteractionActionsCallbackInterface != null)
        {
            Interaction.SetCallbacks(null);
        }
        m_Interaction = null;
        m_Interaction_Interact = null;
        if (m_LayerSwitchingActionsCallbackInterface != null)
        {
            LayerSwitching.SetCallbacks(null);
        }
        m_LayerSwitching = null;
        m_LayerSwitching_ToggleLayerSwitching = null;
        if (m_DialogActionsCallbackInterface != null)
        {
            Dialog.SetCallbacks(null);
        }
        m_Dialog = null;
        m_Dialog_Next = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        var PlayerCallbacks = m_PlayerActionsCallbackInterface;
        var UICallbacks = m_UIActionsCallbackInterface;
        var CameraCallbacks = m_CameraActionsCallbackInterface;
        var InteractionCallbacks = m_InteractionActionsCallbackInterface;
        var LayerSwitchingCallbacks = m_LayerSwitchingActionsCallbackInterface;
        var DialogCallbacks = m_DialogActionsCallbackInterface;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
        Player.SetCallbacks(PlayerCallbacks);
        UI.SetCallbacks(UICallbacks);
        Camera.SetCallbacks(CameraCallbacks);
        Interaction.SetCallbacks(InteractionCallbacks);
        LayerSwitching.SetCallbacks(LayerSwitchingCallbacks);
        Dialog.SetCallbacks(DialogCallbacks);
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Player
    private InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private InputAction m_Player_Move;
    private InputAction m_Player_Jump;
    private InputAction m_Player_ResetPlayer;
    private InputAction m_Player_ToggleFlying;
    public struct PlayerActions
    {
        private PlayerInputMapping m_Wrapper;
        public PlayerActions(PlayerInputMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move { get { return m_Wrapper.m_Player_Move; } }
        public InputAction @Jump { get { return m_Wrapper.m_Player_Jump; } }
        public InputAction @ResetPlayer { get { return m_Wrapper.m_Player_ResetPlayer; } }
        public InputAction @ToggleFlying { get { return m_Wrapper.m_Player_ToggleFlying; } }
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Move.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                Jump.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                ResetPlayer.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetPlayer;
                ResetPlayer.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetPlayer;
                ResetPlayer.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetPlayer;
                ToggleFlying.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleFlying;
                ToggleFlying.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleFlying;
                ToggleFlying.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleFlying;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.cancelled += instance.OnMove;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.cancelled += instance.OnJump;
                ResetPlayer.started += instance.OnResetPlayer;
                ResetPlayer.performed += instance.OnResetPlayer;
                ResetPlayer.cancelled += instance.OnResetPlayer;
                ToggleFlying.started += instance.OnToggleFlying;
                ToggleFlying.performed += instance.OnToggleFlying;
                ToggleFlying.cancelled += instance.OnToggleFlying;
            }
        }
    }
    public PlayerActions @Player
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new PlayerActions(this);
        }
    }
    // UI
    private InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private InputAction m_UI_Navigate;
    private InputAction m_UI_Submit;
    private InputAction m_UI_Cancel;
    private InputAction m_UI_Point;
    private InputAction m_UI_Click;
    private InputAction m_UI_OpenMenu;
    public struct UIActions
    {
        private PlayerInputMapping m_Wrapper;
        public UIActions(PlayerInputMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate { get { return m_Wrapper.m_UI_Navigate; } }
        public InputAction @Submit { get { return m_Wrapper.m_UI_Submit; } }
        public InputAction @Cancel { get { return m_Wrapper.m_UI_Cancel; } }
        public InputAction @Point { get { return m_Wrapper.m_UI_Point; } }
        public InputAction @Click { get { return m_Wrapper.m_UI_Click; } }
        public InputAction @OpenMenu { get { return m_Wrapper.m_UI_OpenMenu; } }
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                Navigate.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                Submit.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                Cancel.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                Point.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                Click.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                OpenMenu.started -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
                OpenMenu.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
                OpenMenu.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                Navigate.started += instance.OnNavigate;
                Navigate.performed += instance.OnNavigate;
                Navigate.cancelled += instance.OnNavigate;
                Submit.started += instance.OnSubmit;
                Submit.performed += instance.OnSubmit;
                Submit.cancelled += instance.OnSubmit;
                Cancel.started += instance.OnCancel;
                Cancel.performed += instance.OnCancel;
                Cancel.cancelled += instance.OnCancel;
                Point.started += instance.OnPoint;
                Point.performed += instance.OnPoint;
                Point.cancelled += instance.OnPoint;
                Click.started += instance.OnClick;
                Click.performed += instance.OnClick;
                Click.cancelled += instance.OnClick;
                OpenMenu.started += instance.OnOpenMenu;
                OpenMenu.performed += instance.OnOpenMenu;
                OpenMenu.cancelled += instance.OnOpenMenu;
            }
        }
    }
    public UIActions @UI
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new UIActions(this);
        }
    }
    // Camera
    private InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private InputAction m_Camera_ToggleCamera;
    private InputAction m_Camera_SetCameraMode1;
    private InputAction m_Camera_SetCameraMode2;
    private InputAction m_Camera_SetCameraMode3;
    private InputAction m_Camera_SetCameraMode4;
    public struct CameraActions
    {
        private PlayerInputMapping m_Wrapper;
        public CameraActions(PlayerInputMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleCamera { get { return m_Wrapper.m_Camera_ToggleCamera; } }
        public InputAction @SetCameraMode1 { get { return m_Wrapper.m_Camera_SetCameraMode1; } }
        public InputAction @SetCameraMode2 { get { return m_Wrapper.m_Camera_SetCameraMode2; } }
        public InputAction @SetCameraMode3 { get { return m_Wrapper.m_Camera_SetCameraMode3; } }
        public InputAction @SetCameraMode4 { get { return m_Wrapper.m_Camera_SetCameraMode4; } }
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                ToggleCamera.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnToggleCamera;
                ToggleCamera.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnToggleCamera;
                ToggleCamera.cancelled -= m_Wrapper.m_CameraActionsCallbackInterface.OnToggleCamera;
                SetCameraMode1.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode1;
                SetCameraMode1.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode1;
                SetCameraMode1.cancelled -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode1;
                SetCameraMode2.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode2;
                SetCameraMode2.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode2;
                SetCameraMode2.cancelled -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode2;
                SetCameraMode3.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode3;
                SetCameraMode3.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode3;
                SetCameraMode3.cancelled -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode3;
                SetCameraMode4.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode4;
                SetCameraMode4.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode4;
                SetCameraMode4.cancelled -= m_Wrapper.m_CameraActionsCallbackInterface.OnSetCameraMode4;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                ToggleCamera.started += instance.OnToggleCamera;
                ToggleCamera.performed += instance.OnToggleCamera;
                ToggleCamera.cancelled += instance.OnToggleCamera;
                SetCameraMode1.started += instance.OnSetCameraMode1;
                SetCameraMode1.performed += instance.OnSetCameraMode1;
                SetCameraMode1.cancelled += instance.OnSetCameraMode1;
                SetCameraMode2.started += instance.OnSetCameraMode2;
                SetCameraMode2.performed += instance.OnSetCameraMode2;
                SetCameraMode2.cancelled += instance.OnSetCameraMode2;
                SetCameraMode3.started += instance.OnSetCameraMode3;
                SetCameraMode3.performed += instance.OnSetCameraMode3;
                SetCameraMode3.cancelled += instance.OnSetCameraMode3;
                SetCameraMode4.started += instance.OnSetCameraMode4;
                SetCameraMode4.performed += instance.OnSetCameraMode4;
                SetCameraMode4.cancelled += instance.OnSetCameraMode4;
            }
        }
    }
    public CameraActions @Camera
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new CameraActions(this);
        }
    }
    // Interaction
    private InputActionMap m_Interaction;
    private IInteractionActions m_InteractionActionsCallbackInterface;
    private InputAction m_Interaction_Interact;
    public struct InteractionActions
    {
        private PlayerInputMapping m_Wrapper;
        public InteractionActions(PlayerInputMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interact { get { return m_Wrapper.m_Interaction_Interact; } }
        public InputActionMap Get() { return m_Wrapper.m_Interaction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(InteractionActions set) { return set.Get(); }
        public void SetCallbacks(IInteractionActions instance)
        {
            if (m_Wrapper.m_InteractionActionsCallbackInterface != null)
            {
                Interact.started -= m_Wrapper.m_InteractionActionsCallbackInterface.OnInteract;
                Interact.performed -= m_Wrapper.m_InteractionActionsCallbackInterface.OnInteract;
                Interact.cancelled -= m_Wrapper.m_InteractionActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_InteractionActionsCallbackInterface = instance;
            if (instance != null)
            {
                Interact.started += instance.OnInteract;
                Interact.performed += instance.OnInteract;
                Interact.cancelled += instance.OnInteract;
            }
        }
    }
    public InteractionActions @Interaction
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new InteractionActions(this);
        }
    }
    // LayerSwitching
    private InputActionMap m_LayerSwitching;
    private ILayerSwitchingActions m_LayerSwitchingActionsCallbackInterface;
    private InputAction m_LayerSwitching_ToggleLayerSwitching;
    public struct LayerSwitchingActions
    {
        private PlayerInputMapping m_Wrapper;
        public LayerSwitchingActions(PlayerInputMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleLayerSwitching { get { return m_Wrapper.m_LayerSwitching_ToggleLayerSwitching; } }
        public InputActionMap Get() { return m_Wrapper.m_LayerSwitching; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(LayerSwitchingActions set) { return set.Get(); }
        public void SetCallbacks(ILayerSwitchingActions instance)
        {
            if (m_Wrapper.m_LayerSwitchingActionsCallbackInterface != null)
            {
                ToggleLayerSwitching.started -= m_Wrapper.m_LayerSwitchingActionsCallbackInterface.OnToggleLayerSwitching;
                ToggleLayerSwitching.performed -= m_Wrapper.m_LayerSwitchingActionsCallbackInterface.OnToggleLayerSwitching;
                ToggleLayerSwitching.cancelled -= m_Wrapper.m_LayerSwitchingActionsCallbackInterface.OnToggleLayerSwitching;
            }
            m_Wrapper.m_LayerSwitchingActionsCallbackInterface = instance;
            if (instance != null)
            {
                ToggleLayerSwitching.started += instance.OnToggleLayerSwitching;
                ToggleLayerSwitching.performed += instance.OnToggleLayerSwitching;
                ToggleLayerSwitching.cancelled += instance.OnToggleLayerSwitching;
            }
        }
    }
    public LayerSwitchingActions @LayerSwitching
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new LayerSwitchingActions(this);
        }
    }
    // Dialog
    private InputActionMap m_Dialog;
    private IDialogActions m_DialogActionsCallbackInterface;
    private InputAction m_Dialog_Next;
    public struct DialogActions
    {
        private PlayerInputMapping m_Wrapper;
        public DialogActions(PlayerInputMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @Next { get { return m_Wrapper.m_Dialog_Next; } }
        public InputActionMap Get() { return m_Wrapper.m_Dialog; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(DialogActions set) { return set.Get(); }
        public void SetCallbacks(IDialogActions instance)
        {
            if (m_Wrapper.m_DialogActionsCallbackInterface != null)
            {
                Next.started -= m_Wrapper.m_DialogActionsCallbackInterface.OnNext;
                Next.performed -= m_Wrapper.m_DialogActionsCallbackInterface.OnNext;
                Next.cancelled -= m_Wrapper.m_DialogActionsCallbackInterface.OnNext;
            }
            m_Wrapper.m_DialogActionsCallbackInterface = instance;
            if (instance != null)
            {
                Next.started += instance.OnNext;
                Next.performed += instance.OnNext;
                Next.cancelled += instance.OnNext;
            }
        }
    }
    public DialogActions @Dialog
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new DialogActions(this);
        }
    }
    private int m_SpaceToJumpSchemeIndex = -1;
    public InputControlScheme SpaceToJumpScheme
    {
        get

        {
            if (m_SpaceToJumpSchemeIndex == -1) m_SpaceToJumpSchemeIndex = asset.GetControlSchemeIndex("SpaceToJump");
            return asset.controlSchemes[m_SpaceToJumpSchemeIndex];
        }
    }
    private int m_SpaceToInteractSchemeIndex = -1;
    public InputControlScheme SpaceToInteractScheme
    {
        get

        {
            if (m_SpaceToInteractSchemeIndex == -1) m_SpaceToInteractSchemeIndex = asset.GetControlSchemeIndex("SpaceToInteract");
            return asset.controlSchemes[m_SpaceToInteractSchemeIndex];
        }
    }
}
public interface IPlayerActions
{
    void OnMove(InputAction.CallbackContext context);
    void OnJump(InputAction.CallbackContext context);
    void OnResetPlayer(InputAction.CallbackContext context);
    void OnToggleFlying(InputAction.CallbackContext context);
}
public interface IUIActions
{
    void OnNavigate(InputAction.CallbackContext context);
    void OnSubmit(InputAction.CallbackContext context);
    void OnCancel(InputAction.CallbackContext context);
    void OnPoint(InputAction.CallbackContext context);
    void OnClick(InputAction.CallbackContext context);
    void OnOpenMenu(InputAction.CallbackContext context);
}
public interface ICameraActions
{
    void OnToggleCamera(InputAction.CallbackContext context);
    void OnSetCameraMode1(InputAction.CallbackContext context);
    void OnSetCameraMode2(InputAction.CallbackContext context);
    void OnSetCameraMode3(InputAction.CallbackContext context);
    void OnSetCameraMode4(InputAction.CallbackContext context);
}
public interface IInteractionActions
{
    void OnInteract(InputAction.CallbackContext context);
}
public interface ILayerSwitchingActions
{
    void OnToggleLayerSwitching(InputAction.CallbackContext context);
}
public interface IDialogActions
{
    void OnNext(InputAction.CallbackContext context);
}
