using UnityEngine;
using UnityEngine.Events;

/*
 * Script to move elevators, doors, or other objects in any of these three ways:
 *   1. A continuous back and forth pattern without stopping
 *   2. A continuous back and forth pattern that can be started and
 *      stopped at any time with the StartMoving() and StopMoving() functions
 *   3. Extended and retracted with the Extend(), Retract(),
 *      and ExtendOrRetract() functions
 * 
 * The distance and speed of movement are configured with the distance and
 * speed fields, and the orientation of movement can be set to horizontal
 * or vertical with the direction field.
 * 
 * Also note that the "continuous" mode will pause at each end of its path
 * for the amount of time specified in pauseTime.
 */

public class MovingPlatform : MonoBehaviour {
    // public fields
    [Tooltip("Distance platform moves, can be positive or negative")]
    public float distance;
    [Tooltip("The platform's speed in m/s")]
    public float speed;
    public enum MvDir { horizontal, vertical };
    [Tooltip("Make platform move side-to-side or up-and-down")]
    public MvDir direction;
    [Tooltip("If set to true, platform will start moving on its own. If " +
        "false, platform will only move upon StartMoving(), Extend(), " +
        "Retract(), or ExtendOrRetract().")]
    public bool moveOnStart;
    [Tooltip("If set to true, door will start as open and close when "
        + "extended instead of opening when extended")]
    public bool startWithDoorOpen;
    [Tooltip("If set to true, door will call extend and retract on all of " +
        "its children instead of on itself.")]
    public bool activateChildrenInstead;
    [Tooltip("If moving continuously, will pause at each end for this many " +
        "seconds")]
    public float pauseTime;

    // UnityEvents for door traversal
    public UnityEvent DoorTraversedL;
    public UnityEvent DoorTraversedR;

    // whether currently moving
    private bool move;
    // current mvmt. mode:    T = extend/retract       F = continuously move
    private bool extendAndRetract;
    // curr. mvmt. direction: T = towards back         F = towards front   
    private bool forwards;
    // desired mvmt. dir.:    T = away from orig. pos. F = towards orig. pos.           
    private bool extend;

    // T = horizontal axis of mvmt;       F = vertical axis of mvmt
    private bool horizontal;
    // T = back is the original position; F = front is the original position
    private bool inverted;
    // lower position value along axis of mvmt. (y-position if vertical, etc.)
    private float back;
    // higher pos. value along axis of movement
    private float front;
    // distance to move per fixed update cycle            
    private float displacement;
    // original position vector
    private Vector3 origPos;
    // current position vector (inside FixedUpdate, may be incorrect elsewhere)
    private Vector3 currPos;

    // collider to tell if the player walked through the door
    private BoxCollider2D doorOpening;

    // current amount of time paused at front/back
    private float timePaused;
    // whether currently paused          
    private bool paused;
    // whether allowed to pause
    private bool canPause;

    // curr. position value along axis of movement
    float platformPosition; 
    // used for sticking/unsticking the player from a platform
    GameObject collidingPlayer;

    void Start() {
        // Set up collider that checks the door opening
        doorOpening = gameObject.AddComponent<BoxCollider2D>();
        doorOpening.isTrigger = true;
        doorOpening.size = new Vector2(0.5f, 1);

        // Disable FixedUpdate if activateChildrenInstead is true
        if (activateChildrenInstead) return;

        move = moveOnStart;
        displacement = speed / 60;
        origPos = transform.position;

        if (distance < 0)
            inverted = true;

        if (direction == MvDir.horizontal) {
            horizontal = true;
        }

        // Set front and back bounds for movement
        if (!inverted) {
            back = horizontal ? origPos.x : origPos.y;
            front = (horizontal ? origPos.x : origPos.y) + distance;
            extend = false;
        }
        else if (inverted) {
            back = (horizontal ? origPos.x : origPos.y) + distance;
            front = horizontal ? origPos.x : origPos.y;
            extend = true;
        }
    }

    void FixedUpdate() {
        currPos = transform.position;
        if (activateChildrenInstead) return;
        if (paused)
            PauseMoving();

        if (move && Mathf.Abs(distance) > 0) {
            // If at bound, turn around or stop if in extendAndRetract mode
            if ((horizontal ? currPos.x : currPos.y)
                 > (front - displacement + .001f)) {
                currPos =
                    new Vector3(
                        horizontal ? front : currPos.x,
                        horizontal ? currPos.y : front,
                        currPos.z
                        );
                if (pauseTime > 0 && canPause)
                    PauseMoving();
                forwards = false;
                if (extendAndRetract && extend)
                    move = false;
            }
            else if ((horizontal ? currPos.x : currPos.y)
                      < (back + displacement - .001f)) {
                currPos =
                    new Vector3(
                        horizontal ? back : currPos.x,
                        horizontal ? currPos.y : back,
                        currPos.z
                        );
                if (pauseTime > 0 && canPause)
                    PauseMoving();
                forwards = true;
                if (extendAndRetract && !extend)
                    move = false;
            }

            // If going wrong direction, turn around
            if (extendAndRetract) {
                if (!inverted && !forwards && extend)
                    forwards = true;
                else if (inverted && forwards && !extend)
                    forwards = false;
            }

            // Move platform (and attached player, if applicable)
            if (move) {
                if (!extendAndRetract)
                    canPause = true;

                // Set platform's transform to be at new position
                if (!inverted ?
                     forwards && (!extendAndRetract || extend) :
                     !(!forwards && (!extendAndRetract || !extend))) {
                    currPos =
                        new Vector3(
                            currPos.x + (horizontal ? displacement : 0),
                            currPos.y + (horizontal ? 0 : displacement),
                            currPos.z
                            );
                }
                else {
                    currPos =
                        new Vector3(
                            currPos.x - (horizontal ? displacement : 0),
                            currPos.y - (horizontal ? 0 : displacement),
                            currPos.z
                            );
                }

                // Move attached player
                if (collidingPlayer != null) {
                    collidingPlayer.transform.Translate(
                        new Vector3(
                            horizontal ? currPos.x - platformPosition : 0,
                            horizontal ? 0 : currPos.y - platformPosition,
                            0
                            )
                        );
                    platformPosition =
                        horizontal ? currPos.x : currPos.y;
                }
            }
        }
        PositionDoorTrigger();
        transform.position = currPos;
    }

    // Pauses platform for pauseTime seconds
    private void PauseMoving() {
        if (timePaused < pauseTime) {
            paused = true;
            extendAndRetract = true;
            move = false;
            timePaused += Time.deltaTime;
        }
        else {
            paused = false;
            extendAndRetract = false;
            timePaused = 0;
            move = true;
            canPause = false;
        }

    }

    // Positions door opening trigger at correct offset
    private void PositionDoorTrigger() {
        if (!startWithDoorOpen)
            doorOpening.offset =
                new Vector2(origPos.x - currPos.x, origPos.y - currPos.y);
        else {
            doorOpening.offset =
                new Vector2(
                    horizontal ? origPos.x + distance - currPos.x : 0,
                    horizontal ? 0 : origPos.y + distance - currPos.y
                    );
        }
    }

    // Starts platform moving back and forth, pausing
    // at each end if pauseTime is greater than zero
    public void StartMoving() {
        extendAndRetract = false;
        move = true;
    }

    // Immediately stops the platform in place
    public void StopMoving() {
        extendAndRetract = false;
        move = false;
    }

    // Starts platform moving towards the far
    // end of its path, where it then stops
    public void Extend() {
        if (activateChildrenInstead) {

            MovingPlatform[] doors =
                gameObject.GetComponentsInChildren<MovingPlatform>();
            foreach (MovingPlatform childPlatform in doors) {
                if (childPlatform.gameObject != gameObject) {
                    Debug.Log("Extend1");
                    childPlatform.Extend();
                }
            }
        }
        else {
            extendAndRetract = true;
            move = true;
            paused = false;
            canPause = false;
            if (!inverted)
                extend = true;
            else
                extend = false;
        }
    }

    // Starts platform moving towards the
    // beginning of its path, where it then stops
    public void Retract() {
        if (activateChildrenInstead) {

            MovingPlatform[] doors =
                gameObject.GetComponentsInChildren<MovingPlatform>();
            foreach (MovingPlatform childPlatform in doors) {
                if (childPlatform.gameObject != gameObject) {
                    childPlatform.Retract();
                }
            }
        }
        else {
            extendAndRetract = true;
            move = true;
            paused = false;
            canPause = false;
            if (!inverted)
                extend = false;
            else
                extend = true;
        }
    }

    // Starts platform moving towards the opposite end of the
    // path from where it was last moving towards, then stops
    public void ExtendOrRetract() {
        if (activateChildrenInstead) {

            MovingPlatform[] doors =
                gameObject.GetComponentsInChildren<MovingPlatform>();
            foreach (MovingPlatform childPlatform in doors) {
                if (childPlatform.gameObject != gameObject) {
                    childPlatform.ExtendOrRetract();
                }
            }
        }
        else {
            extendAndRetract = true;
            move = true;
            paused = false;
            canPause = false;
            extend = !extend;
        }
    }

    // Called by player class when they jump or walk onto platform:
    // Makes player stick to platform when they're standing on it
    public void StickPlayer(GameObject player) {
        collidingPlayer = player;
        platformPosition =
            horizontal ? transform.position.x : transform.position.y;
    }

    // Called by player class when they jump or walk off of platform:
    // Unsticks player from platform
    public void UnstickPlayer() {
        collidingPlayer = null;
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform.position.x - transform.position.x > 0)
            DoorTraversedL.Invoke();
        else
            DoorTraversedR.Invoke();

    }
}
