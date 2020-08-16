using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    
    public int minDungeonSize;
    public int maxDungeonSize;

    public List<Opening> unconnectedOpenings;
    public Room startingRoomPrefab;

    public float buildSpeed;

    private List<Room> terminalRooms = new List<Room>(); //TODO: later swap them out for special rooms?

    public void Start() {
        Room startingRoom = Instantiate(startingRoomPrefab, Vector3.zero, Quaternion.identity, transform);
        CameraController.instance.SetRoom(startingRoom);
        unconnectedOpenings = startingRoom.openings;

        StartCoroutine(BuildLevel());
    }

    IEnumerator BuildLevel() {
        int dungeonSize = Random.Range(minDungeonSize, maxDungeonSize);
        int roomsAdded = 0;
        int loops = 0;
        while (unconnectedOpenings.Count > 0 && roomsAdded + unconnectedOpenings.Count < dungeonSize) {
            loops++;
            Debug.Log("Loop " + loops);
            //Get a random Opening
            Opening opening = unconnectedOpenings[Random.Range(0, unconnectedOpenings.Count)];
            //Get a suitable room to spawn from that Opening
            Room roomToSpawn = GetRoomToSpawn(opening.direction);
            //Calculate its position relative to the two Openings
            Opening openingToConnect = roomToSpawn.openings.Find(o => o.direction == OppositeTo(opening.direction)); 
            if (openingToConnect == null) {
                Debug.Log("You got a room without a valid Opening!");
                continue;
            }
            Vector3 spawnPoint = opening.transform.position - openingToConnect.transform.position;
            //does it intersect an existing room?
            //spawn it in at the calculated position
            Room spawnedRoom = Instantiate(roomToSpawn, spawnPoint, Quaternion.identity, transform);

            yield return new WaitForSeconds(buildSpeed);
            
            if (spawnedRoom.isColliding) {
                Destroy(spawnedRoom.gameObject);
                yield return new WaitForSeconds(buildSpeed);
                continue;
            }
            //does it form a connection with an existing room?
            //connect up any Openings that are now connected (remove from @unconnectedOpenings)
            unconnectedOpenings.Remove(opening);
            //add any new unconnected Openings to the list
            foreach (Opening spawnedOpening in spawnedRoom.openings) {
                if (spawnedOpening.transform.localPosition != openingToConnect.transform.position) {
                    unconnectedOpenings.Add(spawnedOpening);
                }
            }
            roomsAdded++;
        }

        foreach (Opening opening in unconnectedOpenings) {
            //Get a terminal 
            Room roomToSpawn = GetTerminalRoomToSpawn(opening.direction);
            //Spawn in right pos
            Opening openingToConnect = roomToSpawn.openings.Find(o => o.direction == OppositeTo(opening.direction)); 
            if (openingToConnect == null) {
                Debug.Log("You got a room without a valid Opening!");
                continue;
            }
            Vector3 spawnPoint = opening.transform.position - openingToConnect.transform.position;
            Room spawnedRoom = Instantiate(roomToSpawn, spawnPoint, Quaternion.identity, transform);
            yield return new WaitForSeconds(buildSpeed);
            if (spawnedRoom.isColliding) {
                Destroy(spawnedRoom.gameObject);
                yield return new WaitForSeconds(buildSpeed);
                continue;
            }
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
