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
    public bool shaking;

    public float sizeDivider;

    public Transform transformToFollow;

    float halfHeight;
    float halfWidth;

    float smoothTime = 0.3f;
    float zoomVelocity = 0.0f;

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

        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, Mathf.Clamp(Mathf.Abs(currentRoom.bottomRight.y - currentRoom.topLeft.y) / sizeDivider, 3.75f, 10f), ref zoomVelocity, smoothTime);
        halfHeight = Camera.main.orthographicSize;
        halfWidth = Camera.main.aspect * halfHeight;

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
        } else if (!shaking) {
            transform.position = targetPosition;
        }
    }

    public void SetRoom(Room room) {
        Debug.Log("Room is now " + room);
        changingScenes = true;
        currentRoom = room;
    }

    public void Shake(float duration, float magnitude) {
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    IEnumerator ShakeRoutine(float duration, float magnitude) {
        shaking = true;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = currentRoom.transform.position.x;
            float y = currentRoom.transform.position.y;

            Vector3 targetPosition = new Vector3(
                Mathf.Clamp(transformToFollow.position.x, currentRoom.topLeft.x + halfWidth + x, currentRoom.bottomRight.x - halfWidth + x),
                Mathf.Clamp(transformToFollow.position.y, currentRoom.bottomRight.y + halfHeight + y, currentRoom.topLeft.y - halfHeight + y),
                transform.position.z
            );
            float dX = Random.Range(-1f, 1f) * magnitude;
            float dY = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(targetPosition.x + dX, targetPosition.y + dY, transform.position.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }

        shaking = false;
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
