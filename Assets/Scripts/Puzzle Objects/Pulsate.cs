using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsate : MonoBehaviour
{
    public float pulseTime = 1.0f;
    public float min = 0.95f;
    public float max = 1.2f;
    public bool pulsating = false;

    private SpriteRenderer rend;
    private Color originalColor;

    private float lerp;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<SpriteRenderer>();
        originalColor = rend.material.color;
    }

    // Update is called once per frame
    void Update() {
        if (pulsating) {
            lerp = Mathf.PingPong(Time.time, pulseTime) / pulseTime;
            // print(lerp);
            rend.material.color = Color.Lerp(originalColor * min, originalColor * max, lerp);
        } else if (!rend.material.color.Equals(originalColor)) {
            rend.material.color = ((originalColor - rend.material.color) / 3)+rend.material.color;
        }
    }

    public void StartPulsing () {
        pulsating = true;
    }
    public void StopPulsing() {
        pulsating = false;
    }

}
