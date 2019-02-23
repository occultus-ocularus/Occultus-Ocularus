using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserReceiver : MonoBehaviour, LaserAffector
{
    // private SwitchableDoor door;//private switchDoor door;
    private bool isHit = false;
    private bool isSwitched;
    public UnityEvent Activate;
    public UnityEvent Deactivate;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHit)
        {
            if(!isSwitched) {
                Deactivate.Invoke();
                isSwitched = true;
            }
        }
        isHit = false;
    }

    public Ray2D RedirectLaser(Ray2D inRay, RaycastHit2D hit)
    {
        isHit = true;
        if(isSwitched)
        {
            Activate.Invoke();
            isSwitched = false;
        }
         
        Vector2 o, d;
        o = hit.point * 0f;
        d = hit.point * 0f;
        return new Ray2D(o, d);
    }
}
