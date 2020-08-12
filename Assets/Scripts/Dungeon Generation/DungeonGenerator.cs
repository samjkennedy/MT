using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    
    public int dungeonSize = 5;

    public List<Door> unconnectedDoors;
    public Room startingRoomPrefab;

    private List<Room> terminalRooms = new List<Room>(); //TODO: later swap them out for special rooms?

    public void Start() {
        Room startingRoom = Instantiate(startingRoomPrefab, Vector3.zero, Quaternion.identity, transform);
        CameraController.instance.SetRoom(startingRoom);
        unconnectedDoors = startingRoom.doors;

        int roomsAdded = 0;
        while (unconnectedDoors.Count > 0 && roomsAdded + unconnectedDoors.Count < dungeonSize) {
            //Get a random door
            Door door = unconnectedDoors[Random.Range(0, unconnectedDoors.Count)];
            //Get a suitable room to spawn from that door
            Room roomToSpawn = GetRoomToSpawn(door.direction);
            //Calculate its position relative to the two doors
            Door doorToConnect = roomToSpawn.doors.Find(d => d.direction == OppositeTo(door.direction)); 
            if (doorToConnect == null) {
                Debug.Log("You got a room without a valid door!");
                return;
            }
            Vector3 spawnPoint = door.transform.position - doorToConnect.transform.position;
            //does it intersect an existing room?
            //spawn it in at the calculated position
            Room spawnedRoom = Instantiate(roomToSpawn, spawnPoint, Quaternion.identity, transform);
            //does it form a connection with an existing room?
            //connect up any doors that are now connected (remove from @unconnectedDoors)
            unconnectedDoors.Remove(door);
            //add any new unconnected doors to the list
            foreach (Door spawnedDoor in spawnedRoom.doors) {
                if (spawnedDoor.transform.localPosition != doorToConnect.transform.position) {
                    unconnectedDoors.Add(spawnedDoor);
                }
            }
            roomsAdded++;
        }

        foreach (Door door in unconnectedDoors) {
            //Get a terminal 
            Room roomToSpawn = GetTerminalRoomToSpawn(door.direction);
            //Spawn in right pos
            Door doorToConnect = roomToSpawn.doors.Find(d => d.direction == OppositeTo(door.direction)); 
            if (doorToConnect == null) {
                Debug.Log("You got a room without a valid door!");
                return;
            }
            Vector3 spawnPoint = door.transform.position - doorToConnect.transform.position;
            Room spawnedRoom = Instantiate(roomToSpawn, spawnPoint, Quaternion.identity, transform);
            //Add to terminalRooms
            terminalRooms.Add(spawnedRoom);
            //Don't modify collection - no need
        }
    }

    private Room GetRoomToSpawn(Direction direction) {

        switch (direction) {
            case Direction.ABOVE:
                return RoomController.instance.bottomRooms[Random.Range(0, RoomController.instance.bottomRooms.Length)];
            case Direction.BELOW:
                return RoomController.instance.topRooms[Random.Range(0, RoomController.instance.topRooms.Length)];
            case Direction.RIGHT:
                return RoomController.instance.leftRooms[Random.Range(0, RoomController.instance.leftRooms.Length)];
            case Direction.LEFT:
                return RoomController.instance.rightRooms[Random.Range(0, RoomController.instance.rightRooms.Length)];
            default:
                Debug.Log("Unknown direction " + direction);
                return null;
        }
    }

    private Room GetTerminalRoomToSpawn(Direction direction) {

        switch (direction) {
            case Direction.ABOVE:
                return RoomController.instance.terminalBottomRooms[Random.Range(0, RoomController.instance.terminalBottomRooms.Length)];
            case Direction.BELOW:
                return RoomController.instance.terminalTopRooms[Random.Range(0, RoomController.instance.terminalTopRooms.Length)];
            case Direction.RIGHT:
                return RoomController.instance.terminalLeftRooms[Random.Range(0, RoomController.instance.terminalLeftRooms.Length)];
            case Direction.LEFT:
                return RoomController.instance.terminalRightRooms[Random.Range(0, RoomController.instance.terminalRightRooms.Length)];
            default:
                Debug.Log("Unknown direction " + direction);
                return null;
        }
    }

    private Direction OppositeTo(Direction direction) {
        
        switch (direction) {
            case Direction.ABOVE:
                return Direction.BELOW;
            case Direction.BELOW:
                return Direction.ABOVE;
            case Direction.RIGHT:
                return Direction.LEFT;
            case Direction.LEFT:
                return Direction.RIGHT;
            default:
                Debug.Log("Unknown direction " + direction);
                return Direction.NONE;
        }
    }

}
