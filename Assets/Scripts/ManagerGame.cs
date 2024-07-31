using System;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;

public class ManagerGame : MonoBehaviour
{
    public WallLocation wallToHide;

    public static ManagerGame instance;
    public static Tuple<int, int> roomIndex { get; set; }

    private ManagerRoom room;
    private Queue<Tuple<int, int>> roomHistory;

    private void Awake()
    {
        instance = this;
        roomHistory = new();
        
        ManagerMap.InitMap();
        roomIndex = ManagerMap.center;
        room = this.AddComponent<ManagerRoom>();

        wallToHide = WallLocation.BOTTOM;

        // DEBUG
        // MapManager.RenderMap();
    }

    public void RenderNextRoom(WallLocation doorLocation)
    {
        ManagerRoom newRoom;
        var neighbors = ManagerMap.DefineNeighbors(roomIndex);

        wallToHide++;
        roomHistory.Enqueue(roomIndex);
        if (roomHistory.Count > 1)
            roomHistory.Dequeue();

        switch (doorLocation)
        {
            case WallLocation.TOP:
                roomIndex = neighbors.topRoom;
                break;
            case WallLocation.LEFT:
                roomIndex = neighbors.leftRoom;
                break;
            case WallLocation.RIGHT:
                roomIndex = neighbors.rightRoom;
                break;
            default:
                roomIndex = neighbors.bottomRoom;
                break;
        }
        newRoom = this.AddComponent<ManagerRoom>();
        StartCoroutine(room.DeRenderAll());
        room = newRoom;

        if ((int)wallToHide == Enum.GetValues(typeof(WallLocation)).Length)
            wallToHide = 0;
    }
}
