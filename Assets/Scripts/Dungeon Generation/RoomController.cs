using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentFloorName = "First Floor";
    RoomInfo currentLoadRoomData;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();
    bool isLoadingRoom = false;

    void Awake()
    {
        instance = this;
    }

    // public bool DoesRoomExist(int x, int y) {
    //     return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    // }

    struct RoomInfo {
        public string name;
        public int x;
        public int y;

        public int length; //1 = 16 tiles
        public int height; //1 = 16 tiles
    }
}
