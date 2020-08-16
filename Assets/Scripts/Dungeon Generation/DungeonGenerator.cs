using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    
    public int minDungeonSize;
    public int maxDungeonSize;

    //Grid space
    public int maxWidth;
    public int maxHeight;

    private int tilesInGridX = 16;
    private int tilesInGridY = 10;

    public List<Opening> unconnectedOpenings;
    public Room startingRoomPrefab;

    public float buildSpeed;

    private RoomData[,] grid;

    private List<Room> terminalRooms = new List<Room>(); //TODO: later swap them out for special rooms?

    public void Start() {
        grid = new RoomData[maxWidth, maxHeight];

        int x = maxWidth / 2;
        int y = maxHeight / 2;

        Vector3 spawnPoint = new Vector3(tilesInGridX * x, tilesInGridY * y, 0);
        Room startingRoom = Instantiate(startingRoomPrefab, spawnPoint, Quaternion.identity, transform);

        RoomData roomData = new RoomData();
        roomData.position = startingRoom.transform.position;
        roomData.width = startingRoom.width / tilesInGridX;
        roomData.height = startingRoom.height / tilesInGridY;

        List<OpeningData> openingData = new List<OpeningData>();
        foreach (Opening opening in startingRoom.openings) {
            OpeningData openingDatum = new OpeningData();
            openingDatum.x = x;
            openingDatum.y = y;
            openingDatum.direction = opening.direction;
            openingData.Add(openingDatum);
        }
        roomData.openingData = openingData;

        grid[x, y] = roomData;

        // Debug.Log(x + ", " +y);
        // Debug.Log(grid[x, y].width + ", " + grid[x, y].width);

        // foreach (OpeningData openingDatum in grid[x, y].openingData) {
        //     Debug.Log(openingDatum.x + ", " + openingDatum.y);
        //     Debug.Log(openingDatum.direction);
        // }

        CameraController.instance.SetRoom(startingRoom);
        unconnectedOpenings = startingRoom.openings;

        //StartCoroutine(BuildLevel());
        StartCoroutine(BuildLevelWithGrid(roomData));
    }

    public Queue<OpeningData> openings;

    IEnumerator BuildLevelWithGrid(RoomData spawnRoom) {
        int dungeonSize = Random.Range(minDungeonSize, maxDungeonSize);
        Debug.Log(dungeonSize);

        openings = new Queue<OpeningData>();
        foreach (OpeningData openingDatum in spawnRoom.openingData) {
            openings.Enqueue(openingDatum);
        }
        
        int roomsAdded = 0;
        while (openings.Count > 0 && roomsAdded + openings.Count < dungeonSize) {
            Debug.Log(openings.Count);
            OpeningData opening = openings.Dequeue();

            Debug.Log(opening.direction);

            Room roomToSpawn = GetRoomToSpawn(opening.direction);
            //Guarantee a 1x1 room for now, because non 1x1 rooms are hard
            while(roomToSpawn.width / tilesInGridX > 1 || roomToSpawn.height / tilesInGridY > 1) {
                roomToSpawn = GetRoomToSpawn(opening.direction);
            }

            Debug.Log(roomToSpawn);

            Opening openingToConnect = roomToSpawn.openings.Find(o => o.direction == OppositeTo(opening.direction)); 
            if (openingToConnect == null) {
                Debug.Log("You got a room without a valid Opening!");
                continue;
            }

            Vector3 vecForDir = GetVecForDir(opening.direction);
            Vector3 spawnPoint = Vector3.Scale(new Vector3(tilesInGridX, tilesInGridY, 0), vecForDir + new Vector3(opening.x, opening.y, 0));
            Room spawnedRoom = Instantiate(roomToSpawn, spawnPoint, Quaternion.identity, transform);

            RoomData roomData = new RoomData();
            roomData.position = spawnPoint;
            roomData.width = spawnedRoom.width / tilesInGridX;
            roomData.height = spawnedRoom.height / tilesInGridY;

            List<OpeningData> openingData = new List<OpeningData>();

            foreach (Opening spawnedOpening in spawnedRoom.openings) {
                    OpeningData openingDatum = new OpeningData();
                    openingDatum.x = opening.x + (int)vecForDir.x;
                    openingDatum.y = opening.y + (int)vecForDir.y;
                    openingDatum.direction = spawnedOpening.direction;

                    openingData.Add(openingDatum);
                if (spawnedOpening.transform.localPosition != openingToConnect.transform.position) {
                    openings.Enqueue(openingDatum);
                }
            }
            roomData.openingData = openingData;
            grid[opening.x + (int)vecForDir.x, opening.y + (int)vecForDir.y] = roomData;

            roomsAdded++;

            yield return new WaitForSeconds(buildSpeed);
        }
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

    private Vector3 GetVecForDir(Direction direction) {

        switch (direction) {
            case Direction.ABOVE:
                return new Vector3(0, 1, 0);
            case Direction.BELOW:
                return new Vector3(0, -1, 0);
            case Direction.RIGHT:
                return new Vector3(1, 0, 0);
            case Direction.LEFT:
                return new Vector3(-1, 0, 0);
            default:
                Debug.Log("Unknown direction " + direction);
                return Vector3.zero;
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

    public struct RoomData {
        //In game world
        public Vector3 position;
        //In grid units
        public int width;
        public int height;

        public List<OpeningData> openingData;
    }

    public struct OpeningData {
        public int x;
        public int y;
        public Direction direction;
    }

}
