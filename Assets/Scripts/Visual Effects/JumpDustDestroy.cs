using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDustDestroy : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps && !ps.IsAlive()) {
            Destroy(gameObject);
        }
    }
}
