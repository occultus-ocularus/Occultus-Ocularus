using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMove : MonoBehaviour {
    public Camera cam;
    [Header("Positioning")]
    [Tooltip("Where the camera originally was")]
    public Vector3 origin;
    [Tooltip("Where it should go to")] public Vector3 destination;

    public float moveTime = 2.0f;

    private CameraScript cScript;

    // Use this for initialization
    void Start() {
        if (cam == null) { cam = Camera.main; }
        cScript = cam.GetComponent<CameraScript>();
    }

    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {

            ///This is to check if you are on same y height as this object    
            //serves to allow checking in you approached from left or right
            if (Mathf.Abs(other.transform.position.x - transform.position.x) < this.GetComponent<BoxCollider2D>().size.x / 2) {
                //you have entered from the top
                if (other.transform.position.y > this.transform.position.y) {
                    cScript.pastCameraPosition = origin;
                    cScript.SetDestination(origin, moveTime);
                } else { //you entered from the bottom
                    cScript.pastCameraPosition = destination;
                    cScript.SetDestination(destination, moveTime);

                }
            } else if (Mathf.Abs(other.transform.position.y - transform.position.y) > this.GetComponent<BoxCollider2D>().size.y / 2) {
                //enter from the right
                if (other.transform.position.x > this.transform.position.x) {
                    cScript.pastCameraPosition = origin;
                    cScript.SetDestination(origin, moveTime);
                } else { //you entered from the left 
                    cScript.pastCameraPosition = destination;
                    cScript.SetDestination(destination, moveTime);
                }
            }
        }
    }
}
