using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    // string currentFloorName = "First Floor";
    // RoomInfo currentLoadRoomData;
    public Room currentRoom;

    // Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    // public List<Room> loadedRooms = new List<Room>();
    // bool isLoadingRoom = false;

    //My shit

    public Room[] leftRooms;
    public Room[] rightRooms;
    public Room[] topRooms;
    public Room[] bottomRooms;
    public Room[] terminalLeftRooms;
    public Room[] terminalRightRooms;
    public Room[] terminalTopRooms;
    public Room[] terminalBottomRooms;

    void Awake()
    {
        instance = this;
    }

    // void Update() {
    //     //UpdateRoomQueue();
    // }

    // private void UpdateRoomQueue() {
    //     if (isLoadingRoom) {
    //         return;
    //     }
    //     if (loadRoomQueue.Count == 0) {
    //         return;
    //     }

    //     currentLoadRoomData = loadRoomQueue.Dequeue();
    //     isLoadingRoom = true;

    //     StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    // }

    // public void LoadRoom(string name, int x, int y) {
    //     if (RoomExistsAt(x, y)) {
    //         return;
    //     }
    //     RoomInfo newRoomData = new RoomInfo();
    //     newRoomData.name = name;
    //     newRoomData.x = x;
    //     newRoomData.y = y;

    //     loadRoomQueue.Enqueue(newRoomData);
    // }

    // IEnumerator LoadRoomRoutine(RoomInfo info) {
    //     string roomName = currentFloorName + " " + info.name;

    //     AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

    //     while (!loadRoom.isDone) {
    //         yield return null;
    //     }
    // }

    // public void RegisterRoom(Room room) {
    //     room.transform.position = new Vector3(
    //         currentLoadRoomData.x * room.width,
    //         currentLoadRoomData.y * room.height,
    //         0
    //     ) + room.GetCenter();

    //     room.x = currentLoadRoomData.x;
    //     room.y = currentLoadRoomData.y;
    //     room.name = currentFloorName + "-" + currentLoadRoomData.name + " " + room.x + ", " + room.y;

    //     room.transform.parent = transform;

    //     isLoadingRoom = false;

    //     if (loadedRooms.Count == 0) {
    //         CameraController.instance.SetRoom(room);
    //     }

    //     loadedRooms.Add(room);
    // }

    // public bool RoomExistsAt(int x, int y) {
    //     return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    // }

    public void OnPlayerEnterRoom(Room room) {
        CameraController.instance.SetRoom(room);
        currentRoom = room;
    }

    // struct RoomInfo {
    //     public string name;
    //     public int x;
    //     public int y;

    //     public int length; //1 = 16 tiles
    //     public int height; //1 = 16 tiles
    // }
}
