using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Movement")]
    [Tooltip("On/Off for player movement input")] public bool canMove;
    [Tooltip("Freeze all movement IE no gravity")] public bool frozen;
    [Tooltip("Whether to allow the player to fly freely")] public bool antiGrav = false;
    public float maxWalkSpeed = 1;
    public float walkForce = 20;
    public float maxJumpTime = 2.0f;
    public float jump_force = 1200;
    public float jump_offset=0.0f;
    public float jump_shift = -0.1f;
    public float jump_forced_decel = -50f;

    [Header("Audio")]
    public AudioSource backgroundAudio;
    public AudioSource footsteps;

    private Rigidbody2D body;
    private Vector3 startPoint;
    private SpriteRenderer spriterender;

    private bool jump = false;
    private bool allowedToJump = false;
    private float jumpTime = 0.0f;
    private bool onGround = false;


    // For determining whether the player is touching the ground
    private ContactPoint2D[] contactPoints = new ContactPoint2D[5];

    // Use this for initialization
    void Start() {
        startPoint = this.transform.position;  
        body = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if(this.transform.position.y < -50) { this.transform.position = startPoint; print("RESPAWN");}


        // Flips sprite depending on direction of movement
        if (Input.GetAxis("Horizontal") < 0)
            spriterender.flipX = true;
        else if (Input.GetAxis("Horizontal") > 0)
            spriterender.flipX = false;

        InputHandler();
        AudioHandler();
    }

    // Function to handle button or mouse events to avoid cluttering update
    void InputHandler() {


        // Reset position
        if (Input.GetKeyDown(KeyCode.R)) {
            this.transform.position = Vector3.zero;
        }

        // "Jump"
        jump = false;
        if (Input.GetButton("Jump"))
        {
            jump = true;
        }
        if (Input.GetButtonDown("Jump")) {
            // If you can move freely, switch camera modes
            if (antiGrav) {
                CameraScript cScript = Camera.main.GetComponent<CameraScript>();
                if (cScript.mode == CameraScript.CameraMode.Fixed) {
                    cScript.mode = CameraScript.CameraMode.FollowPlayer;
                } else if (cScript.mode == CameraScript.CameraMode.FollowPlayer) {
                    cScript.mode = CameraScript.CameraMode.Fixed;
                    cScript.SetDestination(cScript.pastCameraPosition, 2.0f);
                }
                // If you can't then jump, apply force in FixedUpdate, recommended by Unity docs.
            } else {
                // Only jump if touching the ground
                int count = GetComponent<Collider2D>().GetContacts(contactPoints);
                for(int i = 0; i < count; i ++) {
                    if (Vector2.Dot(contactPoints[i].normal, Vector2.up) > 0.5)
                        allowedToJump = true;
                        jumpTime = 0.0f;
                    }
            }
        }
        onGround = false;
        int count2 = GetComponent<Collider2D>().GetContacts(contactPoints);
        for (int i = 0; i < count2; i++)
        {
            if (Vector2.Dot(contactPoints[i].normal, Vector2.up) > 0.5 && jumpTime > 0) onGround = true;
        }

        // Shift between flying and grounded modes
        if (Input.GetKeyDown(KeyCode.RightShift)) {
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


    void AudioHandler(){
        int count = GetComponent<Collider2D>().GetContacts(contactPoints);
        for (int i = 0; i < count; i++)
        {
            // Only make sound if on the ground
            if (Vector2.Dot(contactPoints[i].normal, Vector2.up) > 0.5)
            {
                // If horizontally moving
                if (Mathf.Abs(body.velocity.x) > 0.1)
                {
                    if (!footsteps.isPlaying)
                    {
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

            // If you are allowed free flight
            if (antiGrav) {
                this.transform.position = this.transform.position + Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * maxWalkSpeed;
                this.transform.position = this.transform.position + Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * maxWalkSpeed;
                this.GetComponent<BoxCollider2D>().isTrigger = true;
            }
            // When u walkin
            else {
                float h = Input.GetAxis("Horizontal");
                if (h * body.velocity.x < maxWalkSpeed)
                    body.AddForce(Vector2.right * h * walkForce);
                if (Mathf.Abs(body.velocity.x) > maxWalkSpeed)
                    body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxWalkSpeed, body.velocity.y);

                if (jump && jumpTime <= maxJumpTime && allowedToJump)
                {
                    //float coef = 0.0f;
                    //float force = 0.0f;

                    float desiredSpeed = 2 / (0.5f + Mathf.Pow(5, 20 * (jumpTime + jump_shift))) + jump_offset;
                    //float desiredSpeed = 1 / Mathf.Pow(jumpTime-1,10)-0.5f;
                    desiredSpeed *= jump_force;
                    float currentSpeed = this.GetComponent<Rigidbody2D>().velocity.y;
                    float dif = desiredSpeed - currentSpeed;
                    dif -= Physics.gravity.y;

                    body.AddForce(dif * this.GetComponent<Rigidbody2D>().mass * Vector3.up * Time.deltaTime);
                    /*
                    //coef = 2 / (1 + Mathf.Pow(50, (jumpTime + 1)));
                    //coef = Mathf.Sqrt(coef);
                    coef = 1 / Mathf.Pow(jumpTime+1, 2) + 0.3f;
                    coef*=this.GetComponent<Rigidbody2D>().mass;
                    if (jumpTime <= 0.3f)
                    {
                        coef = 1;
                        force = jumpForce;
                    }
                    body.AddForce(force * coef * Vector3.up);
                    */
                    jumpTime += Time.deltaTime;
                }
                else if (!onGround){
                    allowedToJump = false;
                    if (this.GetComponent<Rigidbody2D>().velocity.y>= 0){
                        body.AddForce(jump_forced_decel * this.GetComponent<Rigidbody2D>().mass * Vector3.up);
                    }
                }
                this.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }
}
