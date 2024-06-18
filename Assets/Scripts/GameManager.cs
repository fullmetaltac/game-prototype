using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public Room room;
    public WallType frontWall;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        MapManager.InitMap();
        frontWall = WallType.BOTTOM;
        room = new Room(MapManager.center);
        room.Render();
        PlayerController.instance.playerModel.transform.position = room.center;
    }

    public void RenderNextRoom(DoorType doorType)
    {
        Room newRoom;
        var neighbors = MapManager.DefineNeighbors(room.Index);
       
        frontWall++;

        switch (doorType)
        {
            case DoorType.TOP:
                newRoom = new Room(neighbors.topRoom);
                break;
            case DoorType.LEFT:
                newRoom = new Room(neighbors.leftRoom);
                break;
            case DoorType.RIGHT:
                newRoom = new Room(neighbors.rightRoom);
                break;
            default:
                newRoom = new Room(neighbors.bottomRoom);
                break;
        }
        newRoom.Render();
        StartCoroutine(room.DeRender());
        room = newRoom;

        if ((int)frontWall == Enum.GetValues(typeof(WallType)).Length)
            frontWall = 0;
    }   
}
