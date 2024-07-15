using System;
using UnityEngine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public Tuple<int, int> CurrentRoomIndex { get; set; }

    private Bed bed;
    private Floor floor;
    private Door doorTop;
    private Door doorLeft;
    private Door doorRight;
    private Door doorBottom;
    private RoomSize roomSize;
    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        MapManager.InitMap();
        CurrentRoomIndex = MapManager.center;

        roomSize = this.AddComponent<RoomSize>();
        roomSize.Calculate(CurrentRoomIndex);

        floor = this.AddComponent<Floor>();
        floor.Render(roomSize);

        bed = this.AddComponent<Bed>();
        bed.Render(roomSize);

        doorTop = this.AddComponent<Door>();
        doorLeft = this.AddComponent<Door>();
        doorRight = this.AddComponent<Door>();
        doorBottom = this.AddComponent<Door>();

        doorTop.Render(roomSize, DoorType.TOP);
        doorLeft.Render(roomSize, DoorType.LEFT);
        doorRight.Render(roomSize, DoorType.RIGHT);
        doorBottom.Render(roomSize, DoorType.BOTTOM);
    }
}
