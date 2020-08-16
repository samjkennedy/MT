using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (BoxCollider2D))]
[RequireComponent (typeof (Rigidbody2D))]
public class Room : MonoBehaviour
{

    public string name;
    //In tiles
    public int width;
    public int height;

    public bool isColliding;
    public bool isComplete;

    //could get interesting with multiTile rooms but we'll burn that bridge when we come to it
    public int x;
    public int y;

    public List<Opening> openings;
    public List<Door> doors;
    public List<Enemy> killableEnemiesInRoom;

    public Vector3 topLeft;
    public Vector3 bottomRight;

    public Vector3 center;

    void Awake() {
        foreach (Transform child in transform) {
            if (child.tag == "Enemy") {
                child.gameObject.SetActive(false);
            }
        }
    }

    void Start() {
        topLeft = new Vector3(-width/2, height/2, 0);
        bottomRight = new Vector3(width/2, -height/2, 0);
    }

    void Update() {
        isComplete = true;
        killableEnemiesInRoom.Clear();
        foreach (Transform child in transform) {
            if (child.tag == "Enemy") {
                if (child.gameObject.GetComponent<Enemy>() != null && child.gameObject.GetComponent<Enemy>().IsKillable()) {
                    killableEnemiesInRoom.Add(child.GetComponent<Enemy>());
                }
            }
        }
        if (killableEnemiesInRoom.Count == 0) {
            StartCoroutine(LockDoorsIn(false, 0.25f));
        }
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
            
            killableEnemiesInRoom.Clear();
            isComplete = true;
            foreach (Transform child in this.transform) {
                if (child.tag == "Enemy") {
                    child.gameObject.SetActive(true);
                    isComplete = false;
                }
            }
            if (!isComplete) {
                StartCoroutine(LockDoorsIn(true, 0.5f));
            }
        }
        if (other.gameObject.GetComponent<Room>() != null) {
            isColliding = true;
        }
    }

    IEnumerator LockDoorsIn(bool locked, float time) {
        yield return new WaitForSeconds(time);

        foreach (Door door in doors) {
            door.sr.enabled = locked;
            door.collider.enabled = locked;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            foreach (Transform child in this.transform) {
                if (child.tag == "Enemy") {
                    child.gameObject.SetActive(false);
                }
            }
        }
        if (other.gameObject.GetComponent<Room>() != null) {
            isColliding = true;
        }
    }
}
