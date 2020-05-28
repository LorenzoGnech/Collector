using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;

    public int Height;
    public int X;
    public int Y;
    public bool locked = true;
    private bool updatedDoors = false;

    public Room(int x, int y){
        X = x;
        Y = y;
    }

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;
    public List<Door> doors = new List<Door>();
    private Transform sideWalls; 
    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instance == null){
            Debug.Log("You pressed play in the wrong scene");
            return;
        }
        Door[] ds = GetComponentsInChildren<Door>();
        foreach(Door d in ds){
            doors.Add(d);
            switch(d.doorType){
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
            }
        }
        foreach(Transform child in transform){
            if(child.name == "SideWall"){
                sideWalls = child;
            }
        }
        RoomController.instance.RegisterRoom(this);
    }

    private void FixWalls(string name){
        foreach(Transform child in sideWalls){  
            if(child.name == name){
                foreach(Transform child2 in child){
                    if(child2.tag == "HalfWall"){
                        child2.gameObject.SetActive(false);
                    } else if(child2.tag == "Wall"){
                        child2.gameObject.SetActive(true);
                    }
                }
            }
        }                 
    }

    public void RemoveUnconnectedDoors(){
        foreach(Door d in doors){
            switch(d.doorType){
                case Door.DoorType.right:
                    if(GetRight() == null){
                        d.gameObject.SetActive(false);
                        //Debug.Log("Rimosso destra " + X + " - " + Y);
                        FixWalls("RightWall");
                    }
                    break;
                case Door.DoorType.left:
                    if(GetLeft() == null){
                        d.gameObject.SetActive(false);
                        FixWalls("LeftWall");
                        //Debug.Log("Rimosso sinistra " + X + " - " + Y);
                    }
                    break;
                case Door.DoorType.top:
                    if(GetTop() == null){
                        d.gameObject.SetActive(false);
                        //Debug.Log("Rimosso sopra " + X + " - " + Y);
                        FixWalls("TopWall");
                    }
                    break;
                case Door.DoorType.bottom:
                    if(GetBottom() == null){
                        d.gameObject.SetActive(false);
                        //Debug.Log("Rimosso sotto " + X + " - " + Y);
                        FixWalls("BottomWall");
                    }
                    break;
            }
        }
    }
    
    public Room GetRight(){
        if(RoomController.instance.DoesRoomExist(X+1, Y)){
            return RoomController.instance.FindRoom(X+1, Y);
        } else{
            return null;
        }
    }
    public Room GetLeft(){
        if(RoomController.instance.DoesRoomExist(X-1, Y)){
            return RoomController.instance.FindRoom(X-1, Y);
        } else{
            return null;
        }
    }
    public Room GetTop(){
        if(RoomController.instance.DoesRoomExist(X, Y+1)){
            return RoomController.instance.FindRoom(X, Y+1);
        } else{
            return null;
        }
    }
    public Room GetBottom(){
        if(RoomController.instance.DoesRoomExist(X, Y-1)){
            return RoomController.instance.FindRoom(X, Y-1);
        } else{
            return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(name.Contains("End") && !updatedDoors){
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCenter(){
        return new Vector3(X*Width, Y*Height);
    }

     void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }

    public void UnlockDoors(){
        locked = false;
        Debug.Log("Sblocco porte " + X + " " + Y);
        foreach(Door door in doors){
            door.Unlock();
        }
    }

    public void LockDoors(){
        locked = true;
        Debug.Log("Blocco porte " + X + " " + Y);
        foreach(Door door in doors){
            door.Lock();
        }
    }
}
