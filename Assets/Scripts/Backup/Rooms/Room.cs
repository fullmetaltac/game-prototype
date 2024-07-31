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
        PositionateDoor(doorTop, WallLocation.TOP);
        PositionateDoor(doorLeft, WallLocation.LEFT);
        PositionateDoor(doorRight, WallLocation.RIGHT);
        PositionateDoor(doorBottom, WallLocation.BOTTOM);
        PositionateWall(wallTop, WallLocation.TOP);
        PositionateWall(wallLeft, WallLocation.LEFT);
        PositionateWall(wallRigh, WallLocation.RIGHT);
        PositionateWall(wallBottom, WallLocation.BOTTOM);
    }
    private void PositionFloor(GameObject floor)
    {
        int x = Index.Item1;
        int z = Index.Item2;

        var renderer = floor.GetComponent<Renderer>();
        xDim = renderer.bounds.extents.x;
        zDim = renderer.bounds.extents.z;

        float center_x = x * xDim * 2 - ManagerMap.center.Item1 * xDim * 2;
        float center_z = z * zDim * 2 - ManagerMap.center.Item2 * zDim * 2;

        center = new Vector3(center_x, 0, center_z);
        floor.transform.position = center;

        var floorColor = ManagerMap.rooms[Index.Item1, Index.Item2].Split(':')[0];
        var color = ManagerMap.SrtToColor(ManagerMap.rooms[x, z]);
        floor.AddComponent<ColorStateApplier>();
        floor.GetComponent<ColorStateApplier>().sourceColor = color;
    }
    
    private bool IsKeyPresent()
    {
        return !ManagerMap.rooms[Index.Item1, Index.Item2].Contains("NO_KEY");
    }

    private bool IsAlarmPresent()
    {
        return ManagerMap.rooms[Index.Item1, Index.Item2].Contains("ALARM");
    }
    private void PositionKey(GameObject key)
    {
        key.transform.position = floor.transform.position +  new Vector3(0, .6f, 0);
        var keyColor = ManagerMap.rooms[Index.Item1, Index.Item2].Split(':')[1];
        var color = ManagerMap.SrtToColor(keyColor);
        key.AddComponent<ColorStateApplier>();
        key.GetComponent<ColorStateApplier>().sourceColor = color;
    }
    private void PositionAlarm(GameObject alarm)
    {
        alarm.transform.position = floor.transform.position + new Vector3(0, .6f, 0);
        var alarmColor = ManagerMap.rooms[Index.Item1, Index.Item2].Split(':')[1];
        var color = ManagerMap.SrtToColor(alarmColor);
        alarm.AddComponent<ColorStateApplier>();
        alarm.GetComponent<ColorStateApplier>().sourceColor = color;
    }
    private void PositionateDoor(GameObject door, WallLocation type)
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
            case WallLocation.TOP:
                door.transform.position = center + new Vector3(0, door_yDim, zDim - door_zDim);
                neighbor = ManagerMap.GetTopNeighbor(Index);
                break;
            case WallLocation.LEFT:
                door.transform.position = center + new Vector3(-xDim + door_xDim, door_yDim, 0);
                neighbor = ManagerMap.GetLeftNeighbor(Index);
                break;
            case WallLocation.RIGHT:
                door.transform.position = center + new Vector3(xDim - door_xDim, door_yDim, 0);
                neighbor = ManagerMap.GetRightNeighbor(Index);
                break;
            case WallLocation.BOTTOM:
                door.transform.position = center + new Vector3(0, door_yDim, -zDim + door_zDim);
                neighbor = ManagerMap.GetBottomNeighbor(Index);
                break;
        }

        if (neighbor != null)
        {
            color = ManagerMap.SrtToColor(ManagerMap.rooms[neighbor.Item1, neighbor.Item2]);
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
    private void PositionateWall(GameObject wall, WallLocation type)
    {
        var wallLen = Enum.GetValues(typeof(WallLocation)).Length;
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
            case WallLocation.TOP:
                positionBig += new Vector3(0, wallBig_yDim, zDim - wallBig_zDim);
                positionSmall += new Vector3(0, wallSmall_yDim, zDim - wallSmall_zDim);
                break;
            case WallLocation.LEFT:
                positionBig += new Vector3(-xDim + wallBig_xDim, wallBig_yDim, 0);
                positionSmall += new Vector3(-xDim + wallSmall_xDim, wallSmall_yDim, 0);
                break;
            case WallLocation.RIGHT:
                positionBig += new Vector3(xDim - wallBig_xDim, wallBig_yDim, 0);
                positionSmall += new Vector3(xDim - wallSmall_xDim, wallSmall_yDim, 0);
                break;
            case WallLocation.BOTTOM:
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
