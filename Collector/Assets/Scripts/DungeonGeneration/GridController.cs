using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [System.Serializable]
    public struct Grid{
        public int columns, rows;
        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;
    public GameObject gridTile;
    public List<Vector2> availablePoints = new List<Vector2>();
    ObjectRoomSpawner objectRoomSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake(){
        objectRoomSpawner = GetComponentInParent<ObjectRoomSpawner>();
        room = GetComponentInParent<Room>();
        grid.columns = room.Width - 2;
        grid.rows = room.Height -1;
        GenerateGrid();
    }

    public void GenerateGrid(){
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;
        for(int y=0; y<grid.rows; y++){
            for(int x=0; x<grid.columns; x++){
                Debug.Log("Entro nel ciclo!");
                GameObject go = Instantiate(gridTile, transform);
                go.transform.position = new Vector2(x - (grid.columns - grid.horizontalOffset), y - (grid.rows - grid.verticalOffset));
                go.name = "X = " + x + " - Y = " + y;
                availablePoints.Add(go.transform.position);
            }
        }
        objectRoomSpawner.InitialiseObjectSpawning();
    }
}
