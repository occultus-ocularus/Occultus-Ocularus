using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Contains basic utility functions.
 */
public class Util
{
    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
    
        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
