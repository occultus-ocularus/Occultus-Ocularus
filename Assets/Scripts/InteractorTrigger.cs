using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

/*
 * This interactor causes an interaction when something enters or exits its trigger.
 */
public class InteractorTrigger : MonoBehaviour
{
    [Tooltip("Objects with this tag will cause interactions. If tag is empty, all objects will cause interactions.")] 
    public string objectTag = "Player";
    [Tooltip("Whether or not only objects residing in the same layer as this object will cause interactions.")]
    public bool sameLayerOnly = true;
    [Tooltip("This is the event that is triggered when something enters the trigger.")]
    public UnityEvent onEnter;
    [Tooltip("This is the event that is triggered when something exits the trigger.")]
    public UnityEvent onExit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If no tag is specified or the object's tag matches the specified tag.
        if(String.IsNullOrEmpty(tag) || other.tag == objectTag) {
            // If sameLayerOnly is false or the object is in the same layer.
            if(!sameLayerOnly || other.gameObject.layer == gameObject.layer) {
                if(onEnter == null)
                    return;
                onEnter.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If no tag is specified or the object's tag matches the specified tag.
        if(String.IsNullOrEmpty(tag) || other.tag == objectTag) {
            // If sameLayerOnly is false or the object is in the same layer.
            if(!sameLayerOnly || other.gameObject.layer == gameObject.layer) {
                if(onExit == null)
                    return;
                onExit.Invoke();
            }
        }
    }
}
