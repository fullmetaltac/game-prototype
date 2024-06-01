using System;
using UnityEngine;
public class Room : MonoBehaviour
{
    public Vector3 position;
    public Tuple<int, int> topRoom { get; set; }
    public Tuple<int, int> leftRoom { get; set; }
    public Tuple<int, int> rightRoom { get; set; }
    public Tuple<int, int> bottomRoom { get; set; }
    public Tuple<int, int> currentRoom { get; set; }

    public Room(Tuple<int, int> currentRoom)
    {
        this.currentRoom = currentRoom;
        MapManager.DefineNeighbors(this);
    }

    public void Render()
    {
        position = RenderRoom(currentRoom);
        if (topRoom != null)
            RenderRoom(topRoom);
        if (leftRoom != null)
            RenderRoom(leftRoom);
        if (rightRoom != null)
            RenderRoom(rightRoom);
        if (bottomRoom != null)
            RenderRoom(bottomRoom);
    }

    void RenderRoomv0(Tuple<int, int> indexes)
    {
        int i = indexes.Item1;
        int j = indexes.Item2;
        var color = MapManager.CharToColor(MapManager.rooms[i, j]);
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = string.Format(MapManager.box_name, i, j);
        box.transform.position = new Vector3(i, 0, j);
        box.AddComponent<ColorStateApplier>();
        box.GetComponent<ColorStateApplier>().sourceColor = color;
    }

    Vector3 RenderRoom(Tuple<int, int> indexes)
    {
        int x = indexes.Item1;
        int z = indexes.Item2;

        var floor = Instantiate(Resources.Load<GameObject>("floor"));
        floor.name = string.Format(MapManager.box_name, x, z);

        var renderer = floor.GetComponent<Renderer>();
        var xDim = renderer.bounds.extents.x * 2;
        var zDim = renderer.bounds.extents.z * 2;
        floor.transform.position = new Vector3(x * xDim , 0, z * zDim);

        var color = MapManager.CharToColor(MapManager.rooms[x, z]);
        floor.AddComponent<ColorStateApplier>();
        floor.GetComponent<ColorStateApplier>().sourceColor = color;

        return floor.transform.position;
    }

}
