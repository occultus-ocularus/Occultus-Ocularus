using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A component that emits lasers.
public class LaserEmitter : MonoBehaviour {

	[Tooltip("The object to be instantiated as a laser.")]
	public GameObject laser;
	[Tooltip("The maximum number of reflections/refractions for the laser.")]
	public int maxReflections = 10;
	[Tooltip("How long an infinite laser (a laser shot into the empty sky) should be drawn.")]
	public float infiniteRenderSize = 100;
	[Tooltip("How far to scan for laser hits.")]
	public float maxDistance = Mathf.Infinity;

	private GameObject[] lasers;

	// Use this for initialization
	void Start () {
		lasers = new GameObject[maxReflections];
		for(int i = 0; i < lasers.Length; i ++) {
			// Add the lazers in backwards order, so that they render correctly.
			lasers[lasers.Length - i - 1] = GameObject.Instantiate(laser, gameObject.transform);
		}
	}
	
	// Sets the laser objects up so they appear to be bouncing around.
	void Update () {
		// The origin and direction of the ray.
		Vector2 o = transform.position, d = transform.right;
		
		// Whether or not each ray should be visible.
		bool visible = true;

		foreach(GameObject laser in lasers) {
			// If the laser object should be visible (the laser is "still going")
			if(visible) {
				// Make it visible
				laser.SetActive(true);
				Util.SetLayerRecursively(laser, gameObject.layer);
				// Find the intersection between the ray and the layer that the emitter resides in.
				ContactFilter2D cf = new ContactFilter2D();

				// Only check on the same layer.
				cf.SetLayerMask(1 << gameObject.layer);
				// Don't check for triggers.
				cf.useTriggers = false;

				// Only get the first hit.
				RaycastHit2D[] results = new RaycastHit2D[1];
				int count = Physics2D.Raycast(o, d, cf, results, maxDistance);

				// If the ray hit something.
				if(count > 0) {
					RaycastHit2D hit = results[0];
					// Make the laser object face the direction of the ray.
					laser.transform.position = (o + hit.point) * 0.5f;
					Vector3 localScale = laser.transform.localScale;
					Vector3 scale = new Vector3(
						Vector2.Distance(o, hit.point),
						localScale.y,
						localScale.z);
					float angle = Vector2.SignedAngle(laser.transform.right, d);
					laser.transform.Rotate(Vector3.forward, angle);
					laser.transform.localScale = scale;

					// Determine whether a laser redirection is available in the hit object. (Reflection or refraction)
					LaserAffector affector = hit.collider.gameObject.GetComponent<LaserAffector>();
					// If so, redirect the laser.
					if(affector != null) {
						Ray2D newRay = affector.RedirectLaser(new Ray2D(o, d), hit);
						if(newRay.direction.magnitude == 0)
						{
							visible = false;
							continue;
						}
						o = newRay.origin;
						d = newRay.direction;
                        //}
					}
					// Otherwise, set the following laser objects to invisible.
					else
						visible = false;
				}
				// If nothing was hit, i.e. the lazer is shot into the empty sky.
				else {
					// Draw it with length infiniteRenderSize.
					laser.transform.position = (o + d * infiniteRenderSize  * 0.5f);
					Vector3 localScale = laser.transform.localScale;
					Vector3 scale = new Vector3(
						infiniteRenderSize,
						localScale.y,
						localScale.z);
					float angle = Vector2.SignedAngle(laser.transform.right, d);
					laser.transform.Rotate(Vector3.forward, angle);
					laser.transform.localScale = scale;

					// Don't draw the following lasers.
					visible = false;
				}
			}
			else 
				laser.SetActive(false);
		}
	}
}
