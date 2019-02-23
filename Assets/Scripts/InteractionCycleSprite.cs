using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This interaction cycles through a set of given sprites, and sets them to the sprite of the target object.
 * This is useful for simple transitions, e.g., changing a laser emitter's appearence to look as though it is 
 * receiving light or not.
 */
public class InteractionCycleSprite : Interaction
{
    [Tooltip("What sprites to cycle through.")]
    public Sprite[] sprites = null;
    [Tooltip("The index of the current sprite.")]
    public int index = 0;
    public override void Interact()
    {
        // Use the target object if specified, otherwise use this object.
        GameObject target = targetObject != null ? targetObject : gameObject;

        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();

        // If the selected game object does not have a sprite renderer
        if(sr == null) {
            Debug.Log("WARNING! InteractionCycleSprite missing SpriteRenderer!");
        } else {
            // Go to the next sprite in the list, and increment it.
            if(sprites != null && sprites.Length > 0) {
                index ++;
                if(index >= sprites.Length)
                    index = 0;
                sr.sprite = sprites[index];
            }
        }
    }
}
