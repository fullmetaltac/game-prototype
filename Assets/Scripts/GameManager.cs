using System;
using UnityEngine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public Tuple<int, int> CurrentRoomIndex { get; set; }

    private Bed bed;
    private Floor floor;
    private RoomSize roomSize;

    private Wall wallTop;
    private Wall wallLeft;
    private Wall wallRight;
    private Wall wallBottom;

    private Cage cageTop;
    private Cage cageLeft;
    private Cage cageRight;
    private Cage cageBottom;

    private Door doorTop;
    private Door doorLeft;
    private Door doorRight;
    private Door doorBottom;

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

        doorTop.Render(roomSize, WallLocation.TOP);
        doorLeft.Render(roomSize, WallLocation.LEFT);
        doorRight.Render(roomSize, WallLocation.RIGHT);
        doorBottom.Render(roomSize, WallLocation.BOTTOM);

        wallTop = this.AddComponent<Wall>();
        wallLeft = this.AddComponent<Wall>();
        wallRight = this.AddComponent<Wall>();
        wallBottom = this.AddComponent<Wall>();

        wallTop.Render(roomSize, WallLocation.TOP, WallType.NO_CAM);
        wallLeft.Render(roomSize, WallLocation.LEFT, WallType.CAM);
        wallRight.Render(roomSize, WallLocation.RIGHT, WallType.NO_CAM);
        wallBottom.Render(roomSize, WallLocation.BOTTOM, WallType.CAM);

        cageTop = this.AddComponent<Cage>();
        cageLeft = this.AddComponent<Cage>();
        cageRight = this.AddComponent<Cage>();
        cageBottom = this.AddComponent<Cage>();

        cageTop.Render(roomSize, WallLocation.TOP);
        cageLeft.Render(roomSize, WallLocation.LEFT);
        cageRight.Render(roomSize, WallLocation.RIGHT);
        cageBottom.Render(roomSize, WallLocation.BOTTOM);
    }
}
