using System;
using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
    public Vector3 center;
    public static float xDim, zDim;

    public Grid grid;
    public Tuple<int, int> Index { get; set; }

    private string key_name = "Key[{0},{1}]";
    private string room_name = "Room[{0},{1}]";

    GameObject key;
    GameObject alarm;
    GameObject room;
    GameObject floor;
    GameObject doorTop;
    GameObject doorLeft;
    GameObject doorRight;
    GameObject doorBottom;
    GameObject wallTop;
    GameObject wallLeft;
    GameObject wallRigh;
    GameObject wallBottom;


    private void Awake()
    {
        var objectPos = transform.position;
        Vector3Int gridPosition = grid.WorldToCell(objectPos);

        transform.position = grid.CellToWorld(gridPosition);
    }
    public Room(Tuple<int, int> index)
    {
        this.Index = index;
    }
    public void Render()
    {
        int x = Index.Item1;
        int z = Index.Item2;

        InstatiateAll();
        PositionAll();
    }
    public IEnumerator DeRender()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(key);
        Destroy(room);
    }
    public void CloseDoors()
    {
        if (doorTop.GetComponent<ColorStateApplier>().sourceColor == ColorState.BLACK)
            doorTop.GetComponent<BoxCollider>().isTrigger = false;
        if (doorLeft.GetComponent<ColorStateApplier>().sourceColor == ColorState.BLACK)
            doorLeft.GetComponent<BoxCollider>().isTrigger = false;
        if (doorRight.GetComponent<ColorStateApplier>().sourceColor == ColorState.BLACK)
            doorRight.GetComponent<BoxCollider>().isTrigger = false;
        if (doorBottom.GetComponent<ColorStateApplier>().sourceColor == ColorState.BLACK)
            doorBottom.GetComponent<BoxCollider>().isTrigger = false;
    }
    private void InstatiateAll()
    {
        room = Instantiate(Resources.Load<GameObject>("room"));
        if (IsKeyPresent() && !IsAlarmPresent())
            key = Instantiate(Resources.Load<GameObject>("key"));
        if (IsAlarmPresent())
            alarm = Instantiate(Resources.Load<GameObject>("alarm"));
        floor = room.transform.Find("floor").gameObject;
        doorTop = room.transform.Find("doorTop").gameObject;
        doorLeft = room.transform.Find("doorLeft").gameObject;
        doorRight = room.transform.Find("doorRight").gameObject;
        doorBottom = room.transform.Find("doorBottom").gameObject;
        wallTop = room.transform.Find("wall_top").gameObject;
        wallLeft = room.transform.Find("wall_left").gameObject;
        wallRigh = room.transform.Find("wall_right").gameObject;
        wallBottom = room.transform.Find("wall_bot").gameObject;
    }
    private void PositionAll()
    {
        PositionFloor(floor);
        if (IsKeyPresent() && !IsAlarmPresent())
            PositionKey(key);
        if (IsAlarmPresent())
            PositionAlarm(alarm);
        PositionateDoor(doorTop, DoorType.TOP);
        PositionateDoor(doorLeft, DoorType.LEFT);
        PositionateDoor(doorRight, DoorType.RIGHT);
        PositionateDoor(doorBottom, DoorType.BOTTOM);
        PositionateWall(wallTop, WallType.TOP);
        PositionateWall(wallLeft, WallType.LEFT);
        PositionateWall(wallRigh, WallType.RIGHT);
        PositionateWall(wallBottom, WallType.BOTTOM);
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

        var floorColor = MapManager.rooms[Index.Item1, Index.Item2].Split(':')[0];
        var color = MapManager.SrtToColor(MapManager.rooms[x, z]);
        floor.AddComponent<ColorStateApplier>();
        floor.GetComponent<ColorStateApplier>().sourceColor = color;
    }
    
    private bool IsKeyPresent()
    {
        return !MapManager.rooms[Index.Item1, Index.Item2].Contains("NO_KEY");
    }

    private bool IsAlarmPresent()
    {
        return MapManager.rooms[Index.Item1, Index.Item2].Contains("ALARM");
    }
    private void PositionKey(GameObject key)
    {
        key.transform.position = floor.transform.position +  new Vector3(0, .6f, 0);
        var keyColor = MapManager.rooms[Index.Item1, Index.Item2].Split(':')[1];
        var color = MapManager.SrtToColor(keyColor);
        key.AddComponent<ColorStateApplier>();
        key.GetComponent<ColorStateApplier>().sourceColor = color;
    }
    private void PositionAlarm(GameObject alarm)
    {
        alarm.transform.position = floor.transform.position + new Vector3(0, .6f, 0);
        var alarmColor = MapManager.rooms[Index.Item1, Index.Item2].Split(':')[1];
        var color = MapManager.SrtToColor(alarmColor);
        alarm.AddComponent<ColorStateApplier>();
        alarm.GetComponent<ColorStateApplier>().sourceColor = color;
    }
    private void PositionateDoor(GameObject door, DoorType type)
    {
        int x = Index.Item1;
        int z = Index.Item2;
        var neighbor = Index;
        var color = ColorState.GRAY;

        var renderer = door.GetComponent<Renderer>();
        var door_xDim = renderer.bounds.extents.x;
        var door_yDim = renderer.bounds.extents.y;
        var door_zDim = renderer.bounds.extents.z;


        switch (type)
        {
            case DoorType.TOP:
                door.transform.position = center + new Vector3(0, door_yDim, zDim - door_zDim);
                neighbor = MapManager.GetTopNeighbor(Index);
                break;
            case DoorType.LEFT:
                door.transform.position = center + new Vector3(-xDim + door_xDim, door_yDim, 0);
                neighbor = MapManager.GetLeftNeighbor(Index);
                break;
            case DoorType.RIGHT:
                door.transform.position = center + new Vector3(xDim - door_xDim, door_yDim, 0);
                neighbor = MapManager.GetRightNeighbor(Index);
                break;
            case DoorType.BOTTOM:
                door.transform.position = center + new Vector3(0, door_yDim, -zDim + door_zDim);
                neighbor = MapManager.GetBottomNeighbor(Index);
                break;
        }

        if (neighbor != null)
        {
            color = MapManager.SrtToColor(MapManager.rooms[neighbor.Item1, neighbor.Item2]);
            if (GameManager_V0.instance.roomHistory.Contains(neighbor))
                color = ColorState.BLACK;
        }
        else
            door.GetComponent<BoxCollider>().isTrigger = false;

        if (!GameManager_V0.instance.keys.Contains(color))
            door.GetComponent<BoxCollider>().isTrigger = false;
        
        if (color == ColorState.BLACK)
            door.GetComponent<BoxCollider>().isTrigger = true;


        door.AddComponent<ColorStateApplier>();
        door.GetComponent<ColorStateApplier>().sourceColor = color;
    }
    private void PositionateWall(GameObject wall, WallType type)
    {
        var wallLen = Enum.GetValues(typeof(WallType)).Length;
        var frontWall = (int)GameManager_V0.instance.frontWall % wallLen;
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
