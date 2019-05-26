using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Reflects lasers.
public class LaserReflector : MonoBehaviour, ILaserAffector {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Reflects the laser.
	public Ray2D RedirectLaser(Ray2D inRay, RaycastHit2D hit) {
		Vector2 o, d;
		o = hit.point - inRay.direction * 0.01f;
		d = Vector2.Reflect(inRay.direction, hit.normal);
		return new Ray2D(o, d);
	}
}
