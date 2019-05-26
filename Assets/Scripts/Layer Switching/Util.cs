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
    public static void SetSortingLayerRecursively(GameObject obj, int newSortingLayer) {
        try {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            objRenderer.sortingLayerID = newSortingLayer;
        }catch {
            //probably this object didn't have a render component.
        }
        foreach (Transform child in obj.transform) {
            SetSortingLayerRecursively(child.gameObject, newSortingLayer);
        }
    }
}
