using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script can be attached to the player. If we create a trigger box around the player, 
 * this script will rotate any object tagged 'rotateTag' by 'degrees' once the player's 
 * trigger box enters the object and the key 'keyCode' is pressed.
 */

public class PrismInteraction : MonoBehaviour {

    [Tooltip("Object with this tag will be rotated")] 
    public string rotateTag = "Prism";
        
    [Tooltip("How much to rotate the object by")]
    public int degrees = 45;

    [Tooltip("Which key is used for rotation")] [SerializeField] 
    public KeyCode keyCode = KeyCode.E;

    // All of the objects that the player is currently "colliding" with (e.i. the objects to possibly rotate)
    private List<GameObject> currentlyColliding = new List<GameObject>();

    void Update() {
        // If key pressed, in range, and colliding object is not null...
        if (Input.GetKeyDown(keyCode))
        {
            // If the thing with which you are colliding with is tagged correctly, rotate it
            for (int i = 0; i < currentlyColliding.Count; i++)
            {
                if (currentlyColliding[i].tag == rotateTag)
                {
                    currentlyColliding[i].transform.Rotate(new Vector3(0, 0, degrees));
                }
            }  
        }
    }

    /*
     * I tried doing this with OnTriggerStay2D, but it was buggy so I decided to use this method instead.
     * It is much cleaner and smoothe with OnEnter and OnStay with a boolean.
     */

    // Rotates anything tagged 'tagToRotate' when E is pressed
    private void OnTriggerEnter2D(Collider2D collision) {
        currentlyColliding.Add(collision.transform.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        currentlyColliding.Remove(collision.transform.gameObject);
    }
}
