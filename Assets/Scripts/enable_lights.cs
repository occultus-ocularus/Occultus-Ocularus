using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enable_lights : MonoBehaviour
{
    private Tilemap foregroundDec;
    private Color[] lightEffectColors = { new Color(0.584f, 0.422f, 0.422f),
                                         new Color(0.632f, 0.480f, 0.480f),
                                         new Color(0.688f, 0.542f, 0.542f),
                                         new Color(0.792f, 0.684f, 0.684f),
                                         new Color(0.858f, 0.773f, 0.773f)};


   void Start() {
        foregroundDec = GetComponent<Tilemap>();
        foregroundDec.color = lightEffectColors[4];
    }

    public void Interact() {
        this.gameObject.SetActive(true);
    }

    public void SetLightLevel(int level) {
        foregroundDec.color = lightEffectColors[level];
    }
}
