using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public float displayTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, displayTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
