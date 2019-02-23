using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public enum CameraMode { Fixed, FollowPlayer, FollowPlayerRadius, FreeCam}

    [Header("Camera Mode")]
    [Tooltip("Does the camera stay still, follow the player, or follow within a radius?")]
    public CameraMode mode;
    
    [Header("Positioning")]
    [Tooltip("Used so the camera knows where it came from")]
    public Vector3 start;
    [Tooltip("Tells the camera where to go")]
    public Vector3 destination;

    public Vector3 pastCameraPosition;

    [Tooltip("How far the player can move from the view center before the camera will follow it (Applies only to the FollowPlayerRadius camera mode)")]
    public float followRadius = 3;

    [Tooltip("How fast the camera should \"catch up\" with the player when in follow radius mode.")]
    public float followSpeed = 10;

    private GameObject player;
    private float timeToReachTarget = 5.0f;
    private float t;

    // these only apply to the FollowPlayerRadius game mode:
    [Header("FollowPlayerRadius")]
    [Tooltip("Stores the coordinates representing the center of the camera view in world space (2d).")]
    private Vector2 screenCenterVector;
    [Tooltip("Stores the vector between the center of the camera view in world space to the player's position (2d).")]
    private Vector2 screenToPlayerVector;
    // Stores the z(into the screen) distance between the camera and player (calculated when the game starts from (camera z - player z) coordinates)
    private float CameraToPlayerZDistance;

    [Header("FreeCam")]
    public float freeMoveSpeed = 0.2f;
    public Vector2 DistFromPlayer;
    public Vector2 MaxDistFromPlayer = new Vector2(10,10);


    void Start() {
        pastCameraPosition = start;
        player = GameObject.Find("Player");
        start = destination = transform.position;
        CameraToPlayerZDistance = transform.position.z - player.transform.position.z;
    }

    void Update()
    {
        //check if camera mode should change
        CheckCameraMode();


        //Camera is a set a point and does not travel with player
        if (mode == CameraMode.Fixed)
        {
            //remove this camera being parented to the child. Disables direct following
            if (this.transform.parent == player.transform)
            {
                this.transform.parent = null;
            }
            LerpToPosition(this.transform.position, destination);

            //Camera is set to follow player
        }
        else if (mode == CameraMode.FollowPlayer)
        {

            //So unity only makes it happen once for better performance
            if (this.transform.parent != player.transform)
            {
                this.transform.parent = player.transform;
            }

            //Camera follows player with a radius buffer
        }
        else if (mode == CameraMode.FollowPlayerRadius)
        {

            //do normal stuff
            //get the center of the screen (mainCamera) in the unity world units
            screenCenterVector = Camera.main.ScreenToWorldPoint(new Vector2((Screen.width / 2), (Screen.height / 2)));
            screenToPlayerVector = (Vector2)player.transform.position - screenCenterVector; //subtracts the center of the screen coordinates (2d) from the player's coordinates;
            if (screenToPlayerVector.magnitude > followRadius) // if the player is too far from the center of the view:
            {
                //screenToPlayerVector.z = (transform.position.z - player.transform.position.z) - CameraToPlayerZDistance;
                //move the camera such that the player is within the radius focusAreaSize of the view center again:
                transform.Translate((screenToPlayerVector - screenToPlayerVector.normalized * followRadius) * Time.deltaTime * followSpeed, Space.World);
            }
            if ((transform.position.z - player.transform.position.z) != CameraToPlayerZDistance) {//warning is ok, because we keep updating the camera z distance each frame to be closer to the correct one;
                transform.Translate(new Vector3(0, 0, CameraToPlayerZDistance - (transform.position.z - player.transform.position.z)) * Time.deltaTime * followSpeed, Space.World);
            }
        }
        //player can control camera with arrow keys
        else if (mode == CameraMode.FreeCam)
        {
            //move left and right
            if (Input.GetKey(KeyCode.RightArrow) && DistFromPlayer.x < MaxDistFromPlayer.x)
            {
                this.transform.position = new Vector3(this.transform.position.x + freeMoveSpeed, this.transform.position.y, this.transform.position.z);
                DistFromPlayer.x += freeMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && DistFromPlayer.x > -MaxDistFromPlayer.x)
            {
                this.transform.position = new Vector3(this.transform.position.x - freeMoveSpeed, this.transform.position.y, this.transform.position.z);
                DistFromPlayer.x -= freeMoveSpeed;
            }


            if (Input.GetKey(KeyCode.UpArrow) && DistFromPlayer.y < MaxDistFromPlayer.y)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + freeMoveSpeed, this.transform.position.z);
                DistFromPlayer.y += freeMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && DistFromPlayer.y > -MaxDistFromPlayer.y)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - freeMoveSpeed, this.transform.position.z);
                DistFromPlayer.y -= freeMoveSpeed;
            }
        }
    }


    //NOT IN USE RIGHT NOW
    // Makes the camera Lerp from it's current position to another and stops character from moving
    private void LerpToPosition(Vector3 start, Vector3 end){
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(start, end, t);
        print("Lerp");
        //Disable the player movement when the camera is shifting
        if (Vector3.Distance(transform.position, end) > 1)
        {
            GameObject.Find("Player").GetComponent<Player>().canMove = false;
            //Stop the player from moving when you are transitioning with the camera 
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            player.GetComponent<Player>().canMove = true;
            //Give player the contraints on all rotations + Z axis position
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //Check for conditions to change camera mode
    public void CheckCameraMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = CameraMode.Fixed;
            DistFromPlayer = Vector2.zero;
            player.GetComponent<Player>().canMove = true;
        }
        else if ( Input.GetKeyDown(KeyCode.Alpha2))
        {
            mode = CameraMode.FollowPlayer;
            DistFromPlayer = Vector2.zero;
            player.GetComponent<Player>().canMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mode = CameraMode.FollowPlayerRadius;
            DistFromPlayer = Vector2.zero;
            player.GetComponent<Player>().canMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            mode = CameraMode.FreeCam;
            player.GetComponent<Player>().canMove = false;
        }
    }

    public void resetPosition(Vector3 destination, float time) {
        t = 0;
        start = transform.position;
        timeToReachTarget = time;
        destination = start;
    }

    //Use to tell the camera where to move to in fixed mode
    public void SetDestination(Vector3 destination, float time) {
        t = 0;
        start = transform.position;
        timeToReachTarget = time;
        this.destination = destination;
    }
}
