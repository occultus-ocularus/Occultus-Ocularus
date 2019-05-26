using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This interaction will enable or disable the given GameObject.
 */
public class InteractionToggle : Interaction
{

    public enum Mode {
        Toggle,
        AlwaysDisable,
        AlwaysEnable
    }

    [Tooltip("Whether the object should act as a toggle or not.")]
    public Mode mode = Mode.Toggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if(targetObject != null)
        {
            switch(mode)
            {
                case Mode.Toggle:
                    targetObject.SetActive(!targetObject.activeSelf);
                    break;
                case Mode.AlwaysDisable:
                    targetObject.SetActive(false);
                    break;
                case Mode.AlwaysEnable:
                    targetObject.SetActive(true);
                    break;
            }
        }
        else
        {
            switch(mode)
            {
                case Mode.Toggle:
                    gameObject.SetActive(!gameObject.activeSelf);
                    break;
                case Mode.AlwaysDisable:
                    gameObject.SetActive(false);
                    break;
                case Mode.AlwaysEnable:
                    gameObject.SetActive(true);
                    break;
            }
        }
    }
}
