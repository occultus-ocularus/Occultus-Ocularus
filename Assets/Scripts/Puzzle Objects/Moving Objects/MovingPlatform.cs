using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * Script to move platforms in any of these three ways:
 *   1. A continuous back and forth pattern without stopping
 *   2. A continuous back and forth pattern that can be started and
 *      stopped at any time with the StartMoving() and StopMoving() functions
 *   3. Extended and retracted with the Extend() and Retract() functions
 * 
 * The distance and speed of movement are configured with the moveDistance and
 * moveSpeed fields, and the orientation of movement can be set to horizontal
 * or vertical with the movementDirection field.
 */

public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Distance platform moves, can be positive or negative")]
    public float distance;
    [Tooltip("The platform's speed in m/s")]
    public float speed;
    public enum MvDir { horizontal, vertical };
    [Tooltip("Make platform move side-to-side or up-and-down")]
    public MvDir direction;
    [Tooltip("If set to true, platform will start moving on its own. If false, platform will only move upon StartMoving(), Extend(), Retract(), or ExtendOrRetract().")]
    public bool moveOnStart;
    [Tooltip("If set to true, door will start as open and close when extended instead of opening when extended")]
    public bool startWithDoorOpen;
    [Tooltip("If moving continuously, will pause at each end for this many seconds")]
    public float pauseTime;

    public UnityEvent DoorTraversedL;
    public UnityEvent DoorTraversedR;

    private bool move;
    private bool extendAndRetract;
    private bool extend;
    private float back;
    private float front;
    private bool inverted;
    private float displacement;
    private bool forwards;
    private Vector3 originalPosition;
    private BoxCollider2D doorOpening;

    private float timePaused;
    private bool paused;
    private bool canPause;

    float platformPosition;
    GameObject collidingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        move = moveOnStart;
        displacement = speed / 60;
        originalPosition = this.transform.position;
        if (distance < 0)
            inverted = true;

        // Set front and back bounds for movement
        if (!inverted) {
            if (direction == MvDir.horizontal) {
                back = originalPosition.x;
                front = originalPosition.x + distance;
            }
            else if (direction == MvDir.vertical) {
                back = originalPosition.y;
                front = originalPosition.y + distance;
            }
            extend = false;
        }
        else if (inverted) {
            if (direction == MvDir.horizontal) {
                back = originalPosition.x + distance;
                front = originalPosition.x;
            }
            else if (direction == MvDir.vertical) {
                back = originalPosition.y + distance;
                front = originalPosition.y;
            }
            extend = true;
        }

        doorOpening = gameObject.AddComponent<BoxCollider2D>();
        doorOpening.isTrigger = true;
        doorOpening.size = new Vector2(0.5f, 1);
    }

    void FixedUpdate()
    {
        if (paused)
            PauseMoving();

        if (move && Mathf.Abs(distance) > 0) {
            if (direction == MvDir.horizontal) {
                // If at bound, turn around or stop if in extendAndRetract mode
                if (transform.position.x > front - displacement + .001f) {
                    transform.position = new Vector3(front, transform.position.y, transform.position.z);
                    if (pauseTime > 0 && canPause)
                        PauseMoving();
                    forwards = false;
                    if (extendAndRetract && extend)
                        move = false;
                }
                else if (transform.position.x < back + displacement - .001f) {
                    transform.position = new Vector3(back, transform.position.y, transform.position.z);
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

                // Move right or left
                if (move) {
                    if (!extendAndRetract)
                        canPause = true;
                    if (!inverted) {
                        if (forwards && (!extendAndRetract || extend))
                            transform.position = new Vector3(transform.position.x + displacement, transform.position.y, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x - displacement, transform.position.y, transform.position.z);
                    }
                    else {
                        if (!forwards && (!extendAndRetract || !extend))
                            transform.position = new Vector3(transform.position.x - displacement, transform.position.y, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x + displacement, transform.position.y, transform.position.z);
                    }
                    if (collidingPlayer != null) {
                        collidingPlayer.transform.Translate(new Vector3(transform.position.x - platformPosition, 0, 0));
                        platformPosition = transform.position.x;
                    }
                }
            }
            else if (direction == MvDir.vertical) {
                // If at bound, turn around or stop if in extendAndRetract mode
                if (transform.position.y > front - displacement + .001f) {
                    transform.position = new Vector3(transform.position.x, front, transform.position.z);
                    if (pauseTime > 0 && canPause)
                        PauseMoving();
                    forwards = false;
                    if (extendAndRetract && extend)
                        move = false;
                }
                else if (transform.position.y < back + displacement - .001f) {
                    transform.position = new Vector3(transform.position.x, back, transform.position.z);
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

                // Move up or down
                if (move) {
                    if (!extendAndRetract)
                        canPause = true;
                    if (!inverted) {
                        if (forwards && (!extendAndRetract || extend))
                            transform.position = new Vector3(transform.position.x, transform.position.y + displacement, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y - displacement, transform.position.z);
                    }
                    else {
                        if (!forwards && (!extendAndRetract || !extend))
                            transform.position = new Vector3(transform.position.x, transform.position.y - displacement, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y + displacement, transform.position.z);
                    }
                    if (collidingPlayer != null) {
                        collidingPlayer.transform.Translate(new Vector3(0, transform.position.y - platformPosition, 0));
                        platformPosition = transform.position.y;
                    }
                }
            }
        }
        if (!startWithDoorOpen)
            doorOpening.offset = new Vector2((originalPosition.x - transform.position.x) / distance, (originalPosition.y - transform.position.y) / distance);
        else {
            if (direction == MvDir.horizontal)
                doorOpening.offset = new Vector2((originalPosition.x + distance - transform.position.x) / distance, (originalPosition.y - transform.position.y) / distance);
            if (direction == MvDir.vertical)
                doorOpening.offset = new Vector2((originalPosition.x - transform.position.x) / distance, (originalPosition.y + distance - transform.position.y) / distance);
        }
            
    }

    private void PauseMoving()
    {
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

    // Starts platform moving back and forth, pausing at each end if pauseTime is greater than zero
    public void StartMoving()
    {
        extendAndRetract = false;
        move = true;
    }

    // Immediately stops the platform in place
    public void StopMoving()
    {
        extendAndRetract = false;
        move = false;
    }

    // Starts platform moving towards the far end of its path, where it then stops
    public void Extend()
    {
        extendAndRetract = true;
        move = true;
        paused = false;
        canPause = false;
        if (!inverted)
            extend = true;
        else
            extend = false;
    }

    // Starts platform moving towards the beginning of its path, where it then stops
    public void Retract()
    {
        extendAndRetract = true;
        move = true;
        paused = false;
        canPause = false;
        if (!inverted)
            extend = false;
        else
            extend = true;
    }

    // Starts platform moving towards the opposite end of the path from where it was last moving towards, then stops
    public void ExtendOrRetract()
    {
        extendAndRetract = true;
        move = true;
        paused = false;
        canPause = false;
        extend = !extend;
    }

    // Called by player class when they jump or walk onto platform: Makes player stick to platform when they're standing on it.
    public void StickPlayer(GameObject player)
    {
        collidingPlayer = player;
        if (direction == MvDir.vertical) {
            platformPosition = transform.position.y;
        }
        else if (direction == MvDir.horizontal) {
            platformPosition = transform.position.x;
        }
    }

    // Called by player class when they jump or walk off of platform: Unsticks player from platform
    public void UnstickPlayer()
    {
        collidingPlayer = null;
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform.position.x - transform.position.x > 0)
            DoorTraversedL.Invoke();
        else
            DoorTraversedR.Invoke();

    }
}
