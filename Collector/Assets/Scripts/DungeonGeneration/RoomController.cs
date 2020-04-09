using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo{
    public string name;
    public int x;
    public int y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    string currentWorldName = "Basement";
    RoomInfo currentLoadRoomData;
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    public List<Room> loadedRooms = new List<Room>();
    bool isLoadingRoom = false;
    bool spawnedBoosRoom = false;
    bool deletedDoors = false;
    bool updatedRooms = false;
    Room currRoom;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake(){
        instance = this;
    }

    public void LoadRoom(string name, int x, int y){
        if(DoesRoomExist(x, y)){
            return;
        }
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.x = x;
        newRoomData.y = y;
        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info){
        string roomName = currentWorldName + info.name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false){
            yield return null;
        }
    }

    public void RegisterRoom(Room room){
        if(!DoesRoomExist(currentLoadRoomData.x, currentLoadRoomData.y)){
        room.transform.position = new Vector3(
            currentLoadRoomData.x * room.Width,
            currentLoadRoomData.y * room.Height,
            0
            );

            room.X = currentLoadRoomData.x;
            room.Y = currentLoadRoomData.y;
            room.name = currentWorldName + " - " + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;
            if(loadedRooms.Count == 0){
                CameraController.instance.currentRoom = room;
            }
            loadedRooms.Add(room);
        } else{
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public void RemoveDoors(){
        foreach(Room r in loadedRooms){
            r.RemoveUnconnectedDoors();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoomQueue();
        if(!deletedDoors && loadRoomQueue.Count == 0 && !(loadedRooms.Count == 0)){
            UpdateRooms();
            RemoveDoors();
            deletedDoors = true;
        }
    }

    void UpdateRoomQueue(){
        if(isLoadingRoom){
            return;
        }
        if(loadRoomQueue.Count == 0){
            if(!spawnedBoosRoom){
                StartCoroutine(SpawnBoosRoom());
            } 
            return;
        }
        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBoosRoom(){
        spawnedBoosRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQueue.Count == 0){
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public bool DoesRoomExist(int x, int y){
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }
    public Room FindRoom(int x, int y){
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName(){
        string[] possibleRooms = new string[]{
            "Empty", 
            "Basic1"
        };
        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room){
        CameraController.instance.currentRoom = room;
        currRoom = room;
        UpdateRooms();
    }

    private void UpdateRooms(){
        foreach(Room room in loadedRooms){
            if(currRoom != room){
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies != null){
                    foreach(EnemyController enemy in enemies){
                        enemy.notInRoom = true;
                    }
                }
            } else{
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies.Length > 0){
                    foreach(EnemyController enemy in enemies){
                        enemy.notInRoom = false;
                    }
                }
            }
        }
    }
}
