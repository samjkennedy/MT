using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{

    public string name;
    //In tiles
    public int width;
    public int height;

    //could get interesting with multiTile rooms but we'll burn that bridge when we come to it
    public int x;
    public int y;

    public List<Door> doors;

    public Vector3 topLeft;
    public Vector3 bottomRight;

    public Vector3 center;

    void Start() {
        if (RoomController.instance == null) {
            Debug.Log("Room controller instance was null");
            return;
        }

        //RoomController.instance.RegisterRoom(this);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    public Vector3 GetCenter() {
        return center;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}
