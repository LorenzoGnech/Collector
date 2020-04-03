using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private void Start(){
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms){
        RoomController.instance.LoadRoom("Start", 0, 0);
        foreach(Vector2Int roomLocation in rooms){
            RoomController.instance.LoadRoom(RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
        }
        /*
        foreach(Vector2Int roomLocation in rooms){
            Debug.Log("Sono nella room " + roomLocation.x + " " + roomLocation.y);
            Room stanza = RoomController.instance.FindRoom((int)roomLocation.x, (int)roomLocation.y);
            Debug.Log(stanza);
            if(!(stanza == null)){
                stanza.RemoveUnconnectedDoors();
            }
        }
        */
    }
}
