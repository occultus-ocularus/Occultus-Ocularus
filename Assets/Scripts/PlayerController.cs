using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    [Tooltip("On/Off for player movement input")] public bool canMove;
    [Tooltip("Freeze all movement IE no gravity")] public bool frozen;
    [Tooltip("Whether to allow the player to fly freely")] public bool antiGrav = false;
    public float maxWalkSpeed = 1;
    public float walkForce = 20;
    public float jumpForce = 20;
    [Tooltip("How long in fixedUpdate() iterations to allow the player to keep holding the jump button to go higher")]
    public int jumpExtensionTime = 20;

    [Header("Audio")]
    public AudioSource backgroundAudio;
    public AudioSource footsteps;

    private Rigidbody2D body;
    private Vector3 startPoint;
    private SpriteRenderer spriterender;

    private bool jump = false;
    private bool prevJump = false;
    // For determining whether the player is touching the ground
    private ContactPoint2D[] contactPoints = new ContactPoint2D[5];
    private int contactCount = 1;
    [HideInInspector]
    public bool touchingGround = false;
    // TouchingWall will be -1 when the player is contacting a surface to her right and 1 when the wall is to her left (0 when not touching any walls)
    private int touchingWall = 0;
    // For implementing the lerp when you stop moving
    private float stopLerpTime;
    private float jumpVelocity;
    private int jumpTimer;
    private int walljumpTimer;
    private bool hangingOnLedge = true;


    // Use this for initialization
    void Start() {
        startPoint = this.transform.position;
        body = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (this.transform.position.y < -50) { this.transform.position = startPoint; print("RESPAWN"); }


        // Flips sprite depending on direction of movement
        if (canMove)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                spriterender.flipX = true;
                transform.eulerAngles = new Vector3(0, 0, 1); //slight angle allows ledge detection to work.
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                spriterender.flipX = false;
                transform.eulerAngles = new Vector3(0, 0, -1);
            }
        }

        InputHandler();
        AudioHandler();
    }

    // Function to handle button or mouse events to avoid cluttering update
    void InputHandler() {


        // Reset position
        if (Input.GetKeyDown(KeyCode.R)) {
            this.transform.position = Vector3.zero;
        }

        // Jump, set variable, force is applied in in FixedUpdate, (recommended by Unity docs).
        jump = Input.GetButton("Jump");
        // If you can move freely, switch camera modes
        if (jump && antiGrav) {
            CameraScript cScript = Camera.main.GetComponent<CameraScript>();
            if (cScript.mode == CameraScript.CameraMode.Fixed) {
                cScript.mode = CameraScript.CameraMode.FollowPlayer;
            }
            else if (cScript.mode == CameraScript.CameraMode.FollowPlayer) {
                cScript.mode = CameraScript.CameraMode.Fixed;
                cScript.SetDestination(cScript.pastCameraPosition, 2.0f);
            }
        }

        // Shift between flying and grounded modes
        if (Input.GetKeyDown(KeyCode.RightShift)) {
            if (antiGrav) {
                this.GetComponent<Renderer>().material.color = Color.white;
                antiGrav = false;
                body.gravityScale = 1;
            }
            else {
                this.GetComponent<Renderer>().material.color = Color.red;
                antiGrav = true;
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
        }
    }


    void AudioHandler() {
        for (int i = 0; i < contactCount; i++) {
            // Only make sound if on the ground
            if (Vector2.Dot(contactPoints[i].normal, Vector2.up) > 0.5) {
                // If horizontally moving
                if (Mathf.Abs(body.velocity.x) > 0.1) {
                    if (!footsteps.isPlaying) {
                        footsteps.clip = Resources.Load<AudioClip>("Audio/Footsteps/SoftFootsteps" + Random.Range(1, 4));
                        footsteps.pitch = Random.Range(0.7f, 1.0f);
                        footsteps.volume = Random.Range(0.1f, 0.2f);
                        footsteps.Play();
                    }
                }
            }
        }

    }

    void FixedUpdate() {
        if (canMove) {
            //Detect if you're touching the ground or walls right now
            contactCount = GetComponent<Collider2D>().GetContacts(contactPoints);
            touchingGround = false; // reset variables each time in case we left the ground.
            touchingWall = 0;  // reset variables each time in case we left the wall.
            for (int i = 0; i < contactCount; i++) {
                // if were not already touching the ground, check if the contact normal arrow in the up direction is > threshold, 
                if (!touchingGround) touchingGround = Vector2.Dot(contactPoints[i].normal, Vector2.up) > 0.1;
                if (touchingWall == 0) {
                    // If we aren't already touching a wall, check if the contact normal arrow in the +Right and -Right driections is > threshold, and set the sign of touchingwall accordingly. 
                    if (Vector2.Dot(contactPoints[i].normal, Vector2.right) > 0.5) touchingWall = 1;
                    else if (Vector2.Dot(contactPoints[i].normal, -Vector2.right) > 0.5) touchingWall = -1;

                    if (touchingWall != 0) {
                        // If we are touching a wall, check if the contact point is not a corner that is its distance from the players center is less than the height (hardcoded),
                        float contactPointYOffset = contactPoints[i].point.y - transform.position.y;
                        // (since the player is tilted slightly, this will only happen when the wall ends before the top of the player)
                        hangingOnLedge = contactPointYOffset > -0.4 && contactPointYOffset < 0.4;
                    }
                }
            }
            // If you are allowed free flight
            if (antiGrav) {
                this.transform.position = this.transform.position + Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * maxWalkSpeed;
                this.transform.position = this.transform.position + Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * maxWalkSpeed;
                this.GetComponent<BoxCollider2D>().isTrigger = true;
            }
            // When u walkin
            else {
                float h = Input.GetAxis("Horizontal");
                if (System.Math.Abs(h) < 0.01 && touchingGround) { //when ther'es little-to-no sideways input && we're on the ground bring the player to a stop
                    // increse the stopLerpTime, 
                    stopLerpTime += 3.5f * Time.deltaTime;
                    // slow down the x velocity by an value between the current x velocity and 0, determined by how far along stopLerpTime is, 
                    body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0, stopLerpTime), body.velocity.y);//new Vector2(0, 0);//
                }
                else {
                    // when we start moving, reset stopLerpTime
                    stopLerpTime = 0;
                    if (h * body.velocity.x < maxWalkSpeed) {
                        //if we just did a wall jump (wallJumpTimer > 0), decrese the effectiveness of the players horizontal control, so they can't keep planting themselves back on the wall.
                        if (walljumpTimer != 0) body.AddForce(Vector2.right * h * Mathf.Max(0, walkForce / (walljumpTimer / 2)));
                        else body.AddForce(Vector2.right * h * walkForce);
                    }
                    if (Mathf.Abs(body.velocity.x) > maxWalkSpeed) body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxWalkSpeed, body.velocity.y);
                }

                bool jumpStart = !prevJump && jump;
                if (jumpStart) Debug.Log(Time.frameCount);
                if (touchingGround) {
                    walljumpTimer = 0;
                    // if the jump was just started
                    if (jumpStart) {
                        jumpTimer = jumpExtensionTime;
                        body.AddForce(jumpForce * Vector3.up);
                    }
                }
                else {
                    if (jump && jumpTimer != 0) {
                        body.AddForce(-Physics2D.gravity - (1 / jumpTimer) * Vector2.up);
                        jumpTimer--;
                    }
                    if (jumpStart && touchingWall != 0) {
                        walljumpTimer = 60;
                        jumpTimer = jumpExtensionTime;
                        // body.AddForce(Vector2.up * Mathf.Max(0, jumpForce - body.velocity.y * 15));
                        //body.AddForce(Vector2.right * (120 * touchingWall - (h * walkForce)));
                        if (hangingOnLedge) body.velocity = new Vector2(-touchingWall, 4);
                        else body.velocity = new Vector2(touchingWall * 4, 4);
                    }
                    if (walljumpTimer != 0) walljumpTimer--;
                }

                this.GetComponent<BoxCollider2D>().isTrigger = false;
                prevJump = jump;
            }
        }
    }
}