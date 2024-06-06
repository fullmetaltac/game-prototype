using System;
using System.Collections;
using UnityEngine;
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


        var color = MapManager.CharToColor(MapManager.rooms[x, z]);
        floor.AddComponent<ColorStateApplier>();
        floor.GetComponent<ColorStateApplier>().sourceColor = color;
    }

    private void PositionFloor(GameObject floor)
    {
        int x = Index.Item1;
        int z = Index.Item2;

        var renderer = floor.GetComponent<Renderer>();
        xDim = renderer.bounds.extents.x * 2;
        zDim = renderer.bounds.extents.z * 2;

        float center_x = x * xDim - MapManager.center.Item1 * xDim;
        float center_z = z * zDim - MapManager.center.Item2 * zDim;

        center = new Vector3(center_x, 0, center_z);
        floor.transform.position = center;
    }

    private void PositionateDoor(GameObject door, DoorType type)
    {
        var renderer = door.GetComponent<Renderer>();
        var room_yDim = renderer.bounds.extents.y * 2;

        switch (type)
        {
            case DoorType.TOP:
                door.transform.position = center + new Vector3(0, room_yDim / 2, zDim / 2);
                break;
            case DoorType.LEFT:
                door.transform.position = center + new Vector3(-xDim / 2, room_yDim / 2, 0);
                break;
            case DoorType.RIGHT:
                door.transform.position = center + new Vector3(xDim / 2, room_yDim / 2, 0);
                break;
            case DoorType.BOTTOM:
                door.transform.position = center + new Vector3(0, room_yDim / 2, -zDim / 2);
                break;
        }
    }
}
