using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType{
        left, right, top, bottom
    };
    public DoorType doorType;
    // Start is called before the first frame update
    void Start(){
        this.transform.gameObject.AddComponent<BoxCollider2D>();
        Vector2 S = this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds.size;
        int widthParent = GetComponentInParent<Room>().Width;
        int heightParent = GetComponentInParent<Room>().Height;
        this.transform.gameObject.GetComponent<BoxCollider2D>().size = S;
        switch(doorType){
            case DoorType.left:
                this.transform.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (-(widthParent/2), 0);
                break;
            case DoorType.right:
                this.transform.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (widthParent/2, 0);
                break;
            case DoorType.top:
                this.transform.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (heightParent/2, 0);
                break;
            case DoorType.bottom:
                this.transform.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (-(heightParent/2), 0);
                break;
        }
        this.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lock(){
        this.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public void Unlock(){
        this.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

}
