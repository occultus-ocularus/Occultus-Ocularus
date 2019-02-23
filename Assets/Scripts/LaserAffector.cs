using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Affects how a laser is refracted or reflected on a surface.
public interface LaserAffector {

    // A function that will redirect the laser when the object is hit.
    Ray2D RedirectLaser(Ray2D inRay, RaycastHit2D hit);
}
