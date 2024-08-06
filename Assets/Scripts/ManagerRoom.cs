using System;
using UnityEngine;
using Unity.VisualScripting;

public class ManagerRoom : MonoBehaviour
{
    public Tuple<int, int> roomIndex;

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

    private Furniture furnitureTopLeft;
    private Furniture furnitureTopRight;
    private Furniture furnitureBottomLeft;
    private Furniture furnitureBottomRight;

    private void Start()
    {
        roomIndex = ManagerGame.roomIndex;
        LoadAll();
        RenderAll();
    }


    private void LoadAll()
    {
        roomSize = this.AddComponent<RoomSize>();
        floor = this.AddComponent<Floor>();
        bed = this.AddComponent<Bed>();
        doorTop = this.AddComponent<Door>();
        doorLeft = this.AddComponent<Door>();
        doorRight = this.AddComponent<Door>();
        doorBottom = this.AddComponent<Door>();
        wallTop = this.AddComponent<Wall>();
        wallLeft = this.AddComponent<Wall>();
        wallRight = this.AddComponent<Wall>();
        wallBottom = this.AddComponent<Wall>();
        cageTop = this.AddComponent<Cage>();
        cageLeft = this.AddComponent<Cage>();
        cageRight = this.AddComponent<Cage>();
        cageBottom = this.AddComponent<Cage>();
        furnitureTopLeft = this.AddComponent<Furniture>();
        furnitureTopRight = this.AddComponent<Furniture>();
        furnitureBottomLeft = this.AddComponent<Furniture>();
        furnitureBottomRight = this.AddComponent<Furniture>();

    }

    private void RenderAll()
    {
        roomSize.Calculate(roomIndex);
        floor.Render();
        bed.Render();
        doorTop.Render(roomSize, WallLocation.TOP);
        doorLeft.Render(roomSize, WallLocation.LEFT);
        doorRight.Render(roomSize, WallLocation.RIGHT);
        doorBottom.Render(roomSize, WallLocation.BOTTOM);
        wallTop.Render(roomSize, WallLocation.TOP);
        wallLeft.Render(roomSize, WallLocation.LEFT);
        wallRight.Render(roomSize, WallLocation.RIGHT);
        wallBottom.Render(roomSize, WallLocation.BOTTOM);
        cageTop.Render(roomSize, WallLocation.TOP);
        cageLeft.Render(roomSize, WallLocation.LEFT);
        cageRight.Render(roomSize, WallLocation.RIGHT);
        cageBottom.Render(roomSize, WallLocation.BOTTOM);
        furnitureTopLeft.Render(roomSize, FurnitureLocation.TOP_LEFT);
        furnitureTopRight.Render(roomSize, FurnitureLocation.TOP_RIGHT);
        furnitureBottomLeft.Render(roomSize, FurnitureLocation.BOTTOM_LEFT);
        furnitureBottomRight.Render(roomSize, FurnitureLocation.BOTTOM_RIGHT);
    }
        

    public void DeRenderAll()
    {
        floor.DeRender();
        bed.DeRender();
        doorTop.DeRender();
        doorLeft.DeRender();
        doorRight.DeRender();
        doorBottom.DeRender();
        wallTop.DeRender();
        wallLeft.DeRender();
        wallRight.DeRender();
        wallBottom.DeRender();
        cageTop.DeRender();
        cageLeft.DeRender();
        cageRight.DeRender();
        cageBottom.DeRender();
        furnitureTopLeft.DeRender();
        furnitureTopRight.DeRender();
        furnitureBottomLeft.DeRender();
        furnitureBottomRight.DeRender();
    }

}
