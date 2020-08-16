using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    public SpriteRenderer sr;
    public BoxCollider2D collider;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
    }
}
