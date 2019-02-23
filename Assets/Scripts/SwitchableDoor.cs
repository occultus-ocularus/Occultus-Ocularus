using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableDoor : MonoBehaviour
{
    [SerializeField]
    public bool isClosed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openDoor()
    {
        Debug.Log("open");
    }

    public void closeDoor()
    {
        Debug.Log("close");
    }

}
