using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserReceiver : MonoBehaviour, ILaserAffector
{
    public UnityEvent Activate;
    public UnityEvent Deactivate;

    private bool isHit;
    private bool activated;

    UnityAction action;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit && activated)
        {
            Deactivate.Invoke();
            activated = false;
        }
        isHit = false;
    }

    public Ray2D RedirectLaser(Ray2D inRay, RaycastHit2D hit)
    {
        isHit = true;
        if(!activated)
        {
            Activate.Invoke();
            activated = true;
        }

        Vector2 o = new Vector2(0, 0);
        return new Ray2D(o, o);
    }
}
