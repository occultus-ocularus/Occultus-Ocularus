using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVersionNumber : MonoBehaviour
{

    void Start()
    {
        this.GetComponent<Text>().text = "ver " + Application.version;
    }

}
