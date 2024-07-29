using System;
using UnityEngine;
using Unity.VisualScripting;

public class ManagerGame : MonoBehaviour
{
    public static ManagerGame instance;
    public static Tuple<int, int> CurrentRoomIndex { get; set; }

    private ManagerRoom room;

    private void Awake()
    {
        instance = this;
        
        ManagerMap.InitMap();
        CurrentRoomIndex = ManagerMap.center;
        room = this.AddComponent<ManagerRoom>();

        // DEBUG
        // MapManager.RenderMap();
    }
}
