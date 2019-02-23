using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This interaction rotates its object by a set amount of degrees.
 */
public class InteractionRotate : Interaction
{
    [Tooltip("How much to rotate the object by.")]
    public float degrees = 45;
    [Tooltip("After this amount of seconds, the object will have rotated 'degrees' degrees. This value must be positive.")]
    public float rotateTime = 0.5f;

    // Set to infinity so the object doesn't start out rotating.
    private float t = Mathf.Infinity;
    private bool rotating;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
        if(rotating)
        {
            GameObject target = targetObject != null ? targetObject : gameObject;
            if(rotateTime == 0)
            {
                // Rotate instantly if rotateTime == 0
                target.transform.Rotate(new Vector3(0, 0, degrees));

                // Stop rotating.
                rotating = false;
            }
            else
            {
                float nextTimeStep = t + Time.fixedDeltaTime;
                // If the next time step is less than rotateTime.
                if(nextTimeStep < rotateTime) {
                    // Rotate
                    target.transform.Rotate(new Vector3(0, 0, degrees * Time.fixedDeltaTime / rotateTime));
                    t += Time.fixedDeltaTime;

                    /*
                    * If the next time step is greater than rotateTime, and the next time
                    * step is greater than rotateTime.
                    */
                } else if(t < rotateTime && nextTimeStep >= rotateTime) {
                    // Rotate so the object will be pointed exactly in the correct direction.
                    target.transform.Rotate(new Vector3(0, 0, degrees * (rotateTime - t) / rotateTime));
                    t += Time.fixedDeltaTime;

                    // Stop rotating
                    rotating = false;
                }
            }
        }
    }

    public override void Interact()
    {
        // Start animation again if the animation has been played already.
        if(rotating == false)
        {
            rotating = true;
            t = 0;
        }
    }
}
