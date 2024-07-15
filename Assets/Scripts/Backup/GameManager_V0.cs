using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager_V0 : MonoBehaviour
{   
    public Room room;
    public WallType frontWall;
    public Queue<Tuple<int, int>> roomHistory;

    public List<ColorState> keys = new();
    
    public static GameManager_V0 instance;

    private void Awake()
    {
        instance = this;
        roomHistory = new();
        MapManager.InitMap();
        keys.Add(ColorState.GRAY);
        frontWall = WallType.BOTTOM;
        room = new Room(MapManager.center);
        room.Render();
        PlayerController.instance.playerModel.transform.position = room.center;
        //debug
        MapManager.RenderMap();
        keys.Add(ColorState.AQUA);
        keys.Add(ColorState.VIOLET);
        keys.Add(ColorState.ORANGE);
    }


    public void RenderNextRoom(DoorType doorType)
    {
        Room newRoom;
        var neighbors = MapManager.DefineNeighbors(room.Index);
        
        frontWall++;
        roomHistory.Enqueue(room.Index);
        if (roomHistory.Count > 1)
            roomHistory.Dequeue();

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
