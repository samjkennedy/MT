using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform transformToFollow;
    //Bounds
    public Transform topLeft;
    public Transform bottomRight;

    float halfHeight;
    float halfWidth;

    void Start() {
        transformToFollow = GameObject.Find("Player").transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = Camera.main.aspect * halfHeight;
    }

    void Update() {
        transform.position = new Vector3(
            Mathf.Clamp(transformToFollow.position.x, topLeft.position.x + halfWidth, bottomRight.position.x - halfWidth),
            Mathf.Clamp(transformToFollow.position.y, bottomRight.position.y + halfHeight, topLeft.position.y - halfHeight),
            transform.position.z
        );
    }
}
