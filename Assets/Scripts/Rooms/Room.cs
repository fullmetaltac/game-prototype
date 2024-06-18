using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public Vector3 center;
    private GameObject room;
    public static float xDim, zDim;
    public Tuple<int, int> Index { get; set; }


    public Room(Tuple<int, int> index)
    {
        this.Index = index;
    }

    public IEnumerator DeRender()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(room);
    }

    public void Render()
    {
        int x = Index.Item1;
        int z = Index.Item2;

        room = Instantiate(Resources.Load<GameObject>("room"));
        room.name = string.Format(MapManager.box_name, x, z);

        var floor = room.transform.Find("floor").gameObject;
        var doorTop = room.transform.Find("doorTop").gameObject;
        var doorLeft = room.transform.Find("doorLeft").gameObject;
        var doorRight = room.transform.Find("doorRight").gameObject;
        var doorBottom = room.transform.Find("doorBottom").gameObject;

        PositionFloor(floor);
        PositionateDoor(doorTop, DoorType.TOP);
        PositionateDoor(doorLeft, DoorType.LEFT);
        PositionateDoor(doorRight, DoorType.RIGHT);
        PositionateDoor(doorBottom, DoorType.BOTTOM);

        var wallTop = room.transform.Find("wall_top").gameObject;
        var wallLeft = room.transform.Find("wall_left").gameObject;
        var wallRigh = room.transform.Find("wall_right").gameObject;
        var wallBottom = room.transform.Find("wall_bot").gameObject;

        PositionateWall(wallTop, WallType.TOP);
        PositionateWall(wallLeft, WallType.LEFT);
        PositionateWall(wallRigh, WallType.RIGHT);
        PositionateWall(wallBottom, WallType.BOTTOM);


        var color = MapManager.CharToColor(MapManager.rooms[x, z]);
        floor.AddComponent<ColorStateApplier>();
        floor.GetComponent<ColorStateApplier>().sourceColor = color;
    }

    private void PositionFloor(GameObject floor)
    {
        int x = Index.Item1;
        int z = Index.Item2;

        var renderer = floor.GetComponent<Renderer>();
        xDim = renderer.bounds.extents.x;
        zDim = renderer.bounds.extents.z;

        float center_x = x * xDim * 2 - MapManager.center.Item1 * xDim * 2;
        float center_z = z * zDim * 2 - MapManager.center.Item2 * zDim * 2;

        center = new Vector3(center_x, 0, center_z);
        floor.transform.position = center;
    }

    private void PositionateDoor(GameObject door, DoorType type)
    {
        var renderer = door.GetComponent<Renderer>();
        var door_xDim = renderer.bounds.extents.x;
        var door_yDim = renderer.bounds.extents.y;
        var door_zDim = renderer.bounds.extents.z;

        switch (type)
        {
            case DoorType.TOP:
                door.transform.position = center + new Vector3(0, door_yDim, zDim - door_zDim);
                break;
            case DoorType.LEFT:
                door.transform.position = center + new Vector3(-xDim + door_xDim, door_yDim, 0);
                break;
            case DoorType.RIGHT:
                door.transform.position = center + new Vector3(xDim - door_xDim, door_yDim, 0);
                break;
            case DoorType.BOTTOM:
                door.transform.position = center + new Vector3(0, door_yDim, -zDim + door_zDim);
                break;
        }
    }

    private void PositionateWall(GameObject wall, WallType type)
    {
        var wallLen = Enum.GetValues(typeof(WallType)).Length;
        var frontWall = (int)GameManager.instance.frontWall % wallLen;
        var nextWall = (frontWall + 1) % wallLen;

        var wallBig = wall.transform.Find("wall_big").gameObject;
        var wallSmall = wall.transform.Find("wall_small").gameObject;

        var rendererBig = wallBig.GetComponent<Renderer>();
        var rendererSmall = wallSmall.GetComponent<Renderer>();

        var wallBig_xDim = rendererBig.bounds.extents.x;
        var wallBig_yDim = rendererBig.bounds.extents.y;
        var wallBig_zDim = rendererBig.bounds.extents.z;

        var wallSmall_xDim = rendererSmall.bounds.extents.x;
        var wallSmall_yDim = rendererSmall.bounds.extents.y;
        var wallSmall_zDim = rendererSmall.bounds.extents.z;

        var positionBig = center;
        var positionSmall = center;

        switch (type)
        {
            case WallType.TOP:
                positionBig += new Vector3(0, wallBig_yDim, zDim - wallBig_zDim);
                positionSmall += new Vector3(0, wallSmall_yDim, zDim - wallSmall_zDim);
                break;
            case WallType.LEFT:
                positionBig += new Vector3(-xDim + wallBig_xDim, wallBig_yDim, 0);
                positionSmall += new Vector3(-xDim + wallSmall_xDim, wallSmall_yDim, 0);
                break;
            case WallType.RIGHT:
                positionBig += new Vector3(xDim - wallBig_xDim, wallBig_yDim, 0);
                positionSmall += new Vector3(xDim - wallSmall_xDim, wallSmall_yDim, 0);
                break;
            case WallType.BOTTOM:
                positionBig += new Vector3(0, wallBig_yDim, -zDim + wallBig_zDim);
                positionSmall += new Vector3(0, wallSmall_yDim, -zDim + wallSmall_zDim);
                break;
        }
        


        if ((int)type == frontWall || (int)type == nextWall)
        {
            wallBig.SetActive(false);
            wallSmall.SetActive(true);
        }
        else
        {
            wallBig.SetActive(true);
            wallSmall.SetActive(false);
        }

        wall.transform.position = positionBig;
        wallBig.transform.position = positionBig;
        wallSmall.transform.position = positionSmall;
    }
}
