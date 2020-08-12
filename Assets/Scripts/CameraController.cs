using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    //Bounds - does not allow L shaped rooms (or any non-quad) <- TODO one day, should be an ez fix no?
    private Room currentRoom;
    
    public float roomChangeSpeed;
    public bool changingScenes;

    public Transform transformToFollow;

    float halfHeight;
    float halfWidth;

    void Awake() {

        instance = this;
    }

    void Start() {

        transformToFollow = GameObject.Find("Player").transform;
        halfHeight = Camera.main.orthographicSize;
        halfWidth = Camera.main.aspect * halfHeight;
    }

    void Update() {

        if (currentRoom == null) {
            return;
        }

        float x = currentRoom.transform.position.x;
        float y = currentRoom.transform.position.y;

        Vector3 targetPosition = new Vector3(
            Mathf.Clamp(transformToFollow.position.x, currentRoom.topLeft.x + halfWidth + x, currentRoom.bottomRight.x - halfWidth + x),
            Mathf.Clamp(transformToFollow.position.y, currentRoom.bottomRight.y + halfHeight + y, currentRoom.topLeft.y - halfHeight + y),
            transform.position.z
        );

        if (changingScenes) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * roomChangeSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                changingScenes = false;
            }
        } else {
            transform.position = targetPosition;
        }
    }

    public void SetRoom(Room room) {
        Debug.Log("Room is now " + room);
        changingScenes = true;
        currentRoom = room;
    }

    //Not actually needed below here
    void UpdatePosition() {

        if (currentRoom == null) {
            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();
    }

    Vector3 GetCameraTargetPosition() {
        
        if (currentRoom == null) {
            return Vector3.zero;
        }

        Vector3 targetPos = currentRoom.GetCenter();
        targetPos.z = transform.position.z;

        return targetPos;
    }
}
