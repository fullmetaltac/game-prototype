using System;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;

public class ManagerGame : MonoBehaviour
{
    public WallLocation wallToHide;
    public HashSet<ManagerRoom> roomHistory;

    public static ManagerGame instance;
    public static Tuple<int, int> roomIndex { get; set; }

    private ManagerRoom room;

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
        roomHistory.Add(room);

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
        room = newRoom;

        if ((int)wallToHide == Enum.GetValues(typeof(WallLocation)).Length)
            wallToHide = 0;
    }

    public void DeRenderRoom()
    {
        roomHistory.ToList().ForEach(r => { 
            if (r.roomIndex != roomIndex)
            {
                r.DeRenderAll();
            } 
        });
    }
}
