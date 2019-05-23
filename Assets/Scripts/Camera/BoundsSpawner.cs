using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsSpawner : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
	// Attach CameraCollisionBoundsPrefab
	public GameObject prefab;
	public float thickness = 0.25f;
	
	// This script will simply instantiate instances of the Prefab around itself when the game starts.
	// I believe you *must* divide localScale by 2 to use them as coordinates.
    void Start()
    {	
        //Create Right bounds
		GameObject Right = Instantiate(prefab, transform.position + new Vector3(transform.localScale.x / 2, 0,0), Quaternion.identity);
		Right.transform.localScale = new Vector3(thickness, transform.localScale.y, 1);
		
		//Create Top bounds
		GameObject Top = Instantiate(prefab, transform.position + new Vector3(0, transform.localScale.y / 2,0), Quaternion.identity);
		Top.transform.localScale = new Vector3(transform.localScale.x, thickness, 1);
		
		//Create Left bounds
		GameObject Left = Instantiate(prefab, transform.position - new Vector3(transform.localScale.x / 2, 0,0), Quaternion.identity);
		Left.transform.localScale = new Vector3(thickness, transform.localScale.y, 1);
		
		//Create Bottom bounds
		GameObject Bottom = Instantiate(prefab, transform.position - new Vector3(0, transform.localScale.y / 2,0), Quaternion.identity);
		Bottom.transform.localScale = new Vector3(transform.localScale.x, thickness, 1);
    }
}
