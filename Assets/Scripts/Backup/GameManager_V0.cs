using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager_V0 : MonoBehaviour
{   
    public Room room;
    public WallLocation frontWall;
    public Queue<Tuple<int, int>> roomHistory;

    public List<ColorState> keys = new();
    
    public static GameManager_V0 instance;

    private void Awake()
    {
        instance = this;
        roomHistory = new();
        ManagerMap.InitMap();
        keys.Add(ColorState.GRAY);
        frontWall = WallLocation.BOTTOM;
        room = new Room(ManagerMap.center);
        room.Render();
        PlayerController.instance.playerModel.transform.position = room.center;
        //debug
        ManagerMap.RenderMap();
        keys.Add(ColorState.AQUA);
        keys.Add(ColorState.VIOLET);
        keys.Add(ColorState.ORANGE);
    }


    public void RenderNextRoom(WallLocation doorType)
    {
        Room newRoom;
        var neighbors = ManagerMap.DefineNeighbors(room.Index);
        
        frontWall++;
        roomHistory.Enqueue(room.Index);
        if (roomHistory.Count > 1)
            roomHistory.Dequeue();

        switch (doorType)
        {
            case WallLocation.TOP:
                newRoom = new Room(neighbors.topRoom);
                break;
            case WallLocation.LEFT:
                newRoom = new Room(neighbors.leftRoom);
                break;
            case WallLocation.RIGHT:
                newRoom = new Room(neighbors.rightRoom);
                break;
            default:
                newRoom = new Room(neighbors.bottomRoom);
                break;
        }
        newRoom.Render();
        StartCoroutine(room.DeRender());
        room = newRoom;

        if ((int)frontWall == Enum.GetValues(typeof(WallLocation)).Length)
            frontWall = 0;
    }   
}
