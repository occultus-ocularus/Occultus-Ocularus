using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, IPlayerActions {    
    [SerializeField] public PlayerInputMapping playerInput;
    
    [Header("Movement")]
    [Tooltip("On/Off for player movement input")] public bool canMove;

    // Things that can further disable player movement
    private bool dialogOpen = false;
    
    [Tooltip("Freeze all movement IE no gravity")] public bool frozen;
    [Tooltip("Whether to allow the player to fly freely")] public bool antiGrav = false;
    public float maxWalkSpeed = 1;
    public float walkForce = 20;
    
    public enum JumpMode { VelocityBased, GravityBased }
    [Header("Jumping")]
    [Tooltip("Is the jump using a velocity function or gravity?")]
    public JumpMode jumpMode;
    public GameObject JumpDustPrefab;

    // Velocity-Based Jump variables
    public float maxJumpTime = 2.0f;
    public float jump_force = 1200;
    public float jump_offset = 0.0f;
    public float jump_shift = -0.1f;
    public float jump_forced_decel = -50f;
    private bool allowedToJump = false;
    private float jumpTime = 0.0f;

    // Gravity-Based Jump Variables
    [Header("Gravity Jump Mode (Old)")]
    [Tooltip("The initial force to kick the player off the ground.")]
    public float jumpForce = 250;
    [Tooltip("How long in fixedUpdate() iterations to allow the player to keep holding the jump button to go higher (by counteracting gravity)")]
    public int jumpExtensionTime = 15;
    public bool wallJumpingEnabled = false;
    public bool ledgeRecoveryEnabled = false;

    [Header("Audio")]
    public AudioSource backgroundAudio;
    public AudioSource footsteps;

    public AudioClip initialFootstepSound;
    public AudioClip[] footstepsSound = new AudioClip[7];
    public AudioClip jumpSound;
    public AudioClip landSound;


    private bool allowResetting;
    private bool allowFlying;

    private Vector3 startPoint;
    private SpriteRenderer spriterender;
    private CapsuleCollider2D playerCollider;
    private BoxCollider2D playerBottomTrigger;

    private float h; //stores the horizontal axis input value
    //private float prevh; //stores the previous horizontal axis reading
    private bool jump = false;
    private bool prevJump = false;
    private bool jumpStart;

    [HideInInspector]
    public Rigidbody2D body;

    // For determining whether the player is touching the ground
    [HideInInspector]
    public bool touchingGround;
    // TouchingWall will be -1 when the player is contacting a surface to her right and 1 when the wall is to her left (0 when not touching any walls)
    private int touchingWall = 0;
    // For implementing the lerp when you stop moving
    private float stopLerpTime;
    //[HideInInspector] public int flipDirectionTimer;

    private int jumpTimer;
    private int walljumpTimer;
    private bool hangingOnLedge = true;
    private int triggerCount;
    private List<Collider2D> overlappingTriggers = new List<Collider2D>();

    private string currentScene;
    private Animator anim;

    private void SetInputCallbacks() {
        playerInput.Enable();
        playerInput.Player.SetCallbacks(this);
    }
    public void Awake() {
        SetInputCallbacks();
    }
    public bool playerCanMove {
        get { return canMove && !dialogOpen && !PauseMenu.isPaused; }
    }
    // Use this for initialization
    void Start() {
        if (Application.isEditor) {
            allowResetting = true;
            allowFlying = true;
        }
        startPoint = this.transform.position;
        body = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
        playerCollider = GetComponents<CapsuleCollider2D>()[0];
        playerBottomTrigger = GetComponents<BoxCollider2D>()[0];
        // Debug.Log(playerBottomTrigger.isTrigger);
        anim = GetComponent<Animator>();
        currentScene = SceneManager.GetActiveScene().name;
    }
    void Update() {
        if (this.transform.position.y < -50) { ResetPlayer(); print("RESPAWN"); }

        if (playerCanMove)
        {
            // Flips sprite depending on direction of movement
            h = movement.x;
            if (h > 0)
                spriterender.flipX = true;
            else if (h < 0)
                spriterender.flipX = false;
        }
        // Sets animation variables
        anim.SetFloat("speed", Mathf.Abs(body.velocity.x));
        anim.SetBool("grounded", touchingGround);
        anim.SetFloat("vertical velocity", body.velocity.y);
    }
    public void EnterUIOrDialog() {
        PlayerInputModel.instance.enterUI();
//        playerInput.Player.Disable();
//        playerInput.Interaction.Disable();
//        playerInput.Camera.Disable();
//        playerInput.UI.Enable();
//        playerInput.Dialog.Enable();
        dialogOpen = true;
    }
    public void ExitUIOrDialog() {
        PlayerInputModel.instance.exitUI();
//        playerInput.Player.Enable();
//        playerInput.Interaction.Enable();
//        playerInput.Camera.Enable();
//        playerInput.UI.Disable();
//        playerInput.Dialog.Disable();
        dialogOpen = false;
    }

    #region FootstepAudio
    void PlaySoftFootstep() {
        // The soft footstep sound is for grass/sand/dirt etc., but currently
        // everything is urban only, so this sound  is not used for now 

        /*footsteps.clip =
            Resources.Load<AudioClip>("Audio/SFX/Characters/Soft Footsteps/SoftFootsteps" + Random.Range(1, 4));
        footsteps.pitch = Random.Range(0.7f, 1.0f);
        footsteps.volume = Random.Range(0.1f, 0.2f);
        footsteps.Play();*/
    }

    void PlayHardFootstep() {
        footsteps.clip = footstepsSound[Random.Range(0, 6)];
        footsteps.pitch = Random.Range(0.7f, 1.0f);
        footsteps.volume = Random.Range(0.3f, 0.4f);
        footsteps.Play();
    }

    void PlayInitialFootstep() {
        footsteps.clip = initialFootstepSound;
        footsteps.pitch = Random.Range(0.7f, 1.0f);
        footsteps.volume = Random.Range(0.3f, 0.4f);
        footsteps.Play();
    }

    void PlayJumpSound() {
        footsteps.clip = jumpSound;
        footsteps.volume = 0.6f;
        footsteps.Play();
    }

    void PlayLandSound() {
        footsteps.clip = landSound;
        footsteps.volume = 0.5f;
        footsteps.Play();
    }

    void StopFootstep() {
        if (footsteps.clip != null && !footsteps.clip.name.Equals("jump"))
            footsteps.Stop();
    }
    #endregion
    #region InputCallbacks

    public Vector2 movement { get; set; } = Vector2.zero;
    
    public void OnMove(InputAction.CallbackContext context) {
        movement = playerCanMove ? context.ReadValue<Vector2>() : Vector2.zero;
    }
    public void OnResetPlayer(InputAction.CallbackContext context) {        
        // Reset position
        if (allowResetting && context.performed) {
            ResetPlayer();
        }

        // Quick hack for level restarting without using UI onscreen button
        else {
            LevelTransition[] lta = FindObjectsOfType<LevelTransition>();
            foreach (LevelTransition lt in lta) {
                if (lt.gameObject.name.Equals("RestartButton")) {
                    lt.FadeLoadScene();
                }
            }
            FindObjectOfType<PlayStats>().RestartGame();
        }

    }
    public void OnToggleFlying(InputAction.CallbackContext context) {
        // Shift between flying and grounded modes
        if (allowFlying && playerCanMove && context.performed) {
            if (antiGrav) {
                this.GetComponent<Renderer>().material.color = Color.white;
                antiGrav = false;
                body.gravityScale = 1;
            } else {
                this.GetComponent<Renderer>().material.color = Color.red;
                antiGrav = true;
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
        }
    }
    public void OnJump(InputAction.CallbackContext context) {
        if (context.performed && playerCanMove) {
            // Signal jump pressed
            jump = jumpStart = true;
            
            // If not in antigrav and the player just pressed the jump key:
            if (!antiGrav && jumpMode == JumpMode.VelocityBased && touchingGround)
            {
                allowedToJump = true;
                jumpTime = 0.0f;
                // Show Jump Dust Effect:
                GameObject jumpDust = Instantiate(JumpDustPrefab);
                jumpDust.transform.position = transform.position;
                jumpDust.GetComponentInChildren<Renderer>().sortingLayerName = spriterender.sortingLayerName;
            }
            anim.SetBool("startJump", true);
        } else {
            jump = jumpStart = false;
            anim.SetBool("startJump", false);
        }
    }
    #endregion
    
    void FixedUpdate() {
        if (playerCanMove) {
            // If you are allowed free flight
            if (antiGrav)
            {
//                Vector2 movement = PlayerInputModel.instance.movement;
                transform.position += (Vector3.up * movement.y * maxWalkSpeed * 5 +
                                      Vector3.right * movement.x * maxWalkSpeed * 5) 
                                      * Time.deltaTime;
                playerCollider.isTrigger = true;
            } else {
                playerCollider.isTrigger = false; // so the player isn't no-clipping after exiting anti-grav mode

                // -- Jumping Logic: --
                if (jumpMode == JumpMode.VelocityBased) VelocityBasedJump();
                else if (jumpMode == JumpMode.GravityBased) GravityBasedJump();

                prevJump = jump;

                // -- Walking Logic: --
                // prevh = h;
                h = movement.x;

                if (System.Math.Abs(h) < 0.01 && touchingGround) { //when ther'es little-to-no sideways input && we're on the ground bring the player to a stop
                    // increse the stopLerpTime, 
                    stopLerpTime += 3.5f * Time.deltaTime;
                    // slow down the x velocity by an value between the current x velocity and 0, determined by how far along stopLerpTime is, 
                    body.velocity =
                        new Vector2(Mathf.Lerp(body.velocity.x, 0, stopLerpTime),
                            body.velocity.y); //new Vector2(0, 0);//
                } else {
                    // when we start moving, reset stopLerpTime
                    stopLerpTime = 0;
                    if (h * body.velocity.x < maxWalkSpeed) {
                        //if we just did a wall jump (wallJumpTimer > 0), decrese the effectiveness of the players horizontal control, so they can't keep planting themselves back on the wall.
                        if (walljumpTimer != 0)
                            body.AddForce(Vector2.right * h * Mathf.Max(0, walkForce / (walljumpTimer / 2)));
                        else body.AddForce(Vector2.right * h * walkForce);
                    }

                    if (Mathf.Abs(body.velocity.x) > maxWalkSpeed)
                        body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxWalkSpeed, body.velocity.y);
                }
            }
        }else{
			// Copied and pasted from the "if touching ground". Needed to stop player from moving when freeCam is activated
			stopLerpTime += 3.5f * Time.deltaTime;
                    // slow down the x velocity by an value between the current x velocity and 0, determined by how far along stopLerpTime is, 
                    body.velocity =
                        new Vector2(Mathf.Lerp(body.velocity.x, 0, stopLerpTime),
                            body.velocity.y);
		}
    }

    void OnTriggerEnter2D(Collider2D otherCol) {
        triggerCount++;
        overlappingTriggers.Add(otherCol);
        WallGroundCheck(otherCol);

        if (otherCol.gameObject.tag == "Platform")
            otherCol.gameObject.GetComponent<MovingPlatform>().StickPlayer(gameObject);
    }

    void OnTriggerExit2D(Collider2D otherCol) {
        triggerCount--;
        overlappingTriggers.Remove(overlappingTriggers.Find(x => x.Equals(otherCol)));
        WallGroundCheck(otherCol);

        if (otherCol.gameObject.tag == "Platform")
            otherCol.gameObject.GetComponent<MovingPlatform>().UnstickPlayer();
    }

    void WallGroundCheck(Collider2D otherCol) {
        touchingGround = false;
        touchingWall = 0;
        hangingOnLedge = false;
        foreach (Collider2D objCollider in overlappingTriggers)
        {
            if (!objCollider.isTrigger)
            {
                if (playerBottomTrigger.IsTouching(objCollider))
                {
                    touchingGround = true;
                    touchingWall = 0;
                    hangingOnLedge = false;
                }
                //else if (playerLeftTrigger.IsTouching(objCollider))
                //{
                //    touchingWall = 1;
                //    hangingOnLedge = !playerTopTrigger.IsTouching(otherCol);
                //}
                //else if (playerRightTrigger.IsTouching(objCollider))
                //{
                //    touchingWall = -1;
                //    hangingOnLedge = !playerTopTrigger.IsTouching(otherCol);
                //}
            }
        }
    }

    private void ResetPlayer() {
        this.transform.position = startPoint;
        this.GetComponent<PlayerLayerSwitcher>().SwitchPlayerLayer(LayerMask.NameToLayer("Foreground"));
    }

    private void VelocityBasedJump() {
        if (jump && jumpTime <= maxJumpTime && allowedToJump)
        {
            //float coef = 0.0f;
            //float force = 0.0f;

            float desiredSpeed = 2 / (0.5f + Mathf.Pow(5, 20 * (jumpTime + jump_shift))) + jump_offset;
            //float desiredSpeed = 1 / Mathf.Pow(jumpTime-1,10)-0.5f;
            desiredSpeed *= jump_force;
            float currentSpeed = body.velocity.y;
            float dif = desiredSpeed - currentSpeed;
            dif -= Physics.gravity.y;

            body.AddForce(dif * body.mass * Vector3.up * Time.deltaTime);
            /*
            //coef = 2 / (1 + Mathf.Pow(50, (jumpTime + 1)));
            //coef = Mathf.Sqrt(coef);
            coef = 1 / Mathf.Pow(jumpTime+1, 2) + 0.3f;
            coef*=body.mass;
            if (jumpTime <= 0.3f)
            {
                coef = 1;
                force = jumpForce;
            }
            body.AddForce(force * coef * Vector3.up);
            */
            jumpTime += Time.deltaTime;
        }
        else if (!touchingGround)
        {
            allowedToJump = false;
            if (body.velocity.y >= 0)
            {
                body.AddForce(jump_forced_decel * body.mass * Vector3.up);
            }
        }
    }

    private void GravityBasedJump() {
        if (touchingGround)
        {
            walljumpTimer = 0;
            // if the jump was just started
            if (jumpStart)
            {
                jumpTimer = jumpExtensionTime;
                body.AddForce(jumpForce * Vector3.up);
            }
        }
        else
        {
            if (jump && jumpTimer != 0)
            {
                body.AddForce(-Physics2D.gravity -
                              (1 / jumpTimer) *
                              Vector2.up); // counteract gravity while the user is holding the jump button & jump timer hasn't gotten to zero.
                jumpTimer--;
            }
            else
            {
                jumpTimer = 0;
            }

            if (wallJumpingEnabled)
            {
                if (jumpStart && touchingWall != 0)
                {
                    walljumpTimer = 60;
                    jumpTimer = jumpExtensionTime;
                    if (!hangingOnLedge) body.velocity = new Vector2(touchingWall * 4, 4);
                }

                if (walljumpTimer != 0) walljumpTimer--;
            }

            if (ledgeRecoveryEnabled && jumpStart && hangingOnLedge && body.velocity.y < 1)
            {
                body.velocity = new Vector2(-touchingWall, 4);
            }
        }
    }
}