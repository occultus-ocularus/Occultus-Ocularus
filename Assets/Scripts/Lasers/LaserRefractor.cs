using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Refracs lasers.
public class LaserRefractor : MonoBehaviour, LaserAffector {

    public LaserEmitter emitter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// "Refracts" the laser. Not true refraction.
	public Ray2D RedirectLaser(Ray2D inRay, RaycastHit2D hit) {
		Vector2 o, d;
        o = hit.point - (Vector2)hit.normal; //* (transform.localScale.x + 0.01f);
		d = -hit.normal;

        emitter.enabled = true;
        return new Ray2D(o, d);
	}
}
