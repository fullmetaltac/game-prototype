using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    public static int rows, cols;
    public static int size = 7;
    public static Tuple<int, int> center;
    public static string box_name = "Box[{0},{1}]";

    private static string room_data= "COLOR:KEY:TYPE";

    public static string[,] rooms;
    public static string[,] rooms_debug;
    static string start_sector;
    static string second_sector;
    static string third_sector;
    static string end_sector;
    static Dictionary<string, string> colorSectors = new();

    static List<Tuple<int, int>> borders = new();
    static List<Tuple<int, int>> bordersN1 = new();
    static List<Tuple<int, int>> borderTop = new();
    static List<Tuple<int, int>> borderLeft = new();
    static List<Tuple<int, int>> borderRight = new();
    static List<Tuple<int, int>> borderBottom = new();


    static List<Tuple<int, int>> diagonalPrimary = new();
    static List<Tuple<int, int>> diagonalPrimaryN1 = new();
    static List<Tuple<int, int>> diagonalSecondary = new();
    static List<Tuple<int, int>> diagonalSecondaryN1 = new();

    static List<Tuple<int, int>> keysTop = new();
    static List<Tuple<int, int>> keysLeft = new();
    static List<Tuple<int, int>> keysRight = new();
    static List<Tuple<int, int>> keysBottom = new();

    static List<Tuple<int, int>> roomsTop = new();
    static List<Tuple<int, int>> roomsLeft = new();
    static List<Tuple<int, int>> roomsRight = new();
    static List<Tuple<int, int>> roomsBottom = new();

    static List<Tuple<int, int>> innerSector = new();
    static List<Tuple<int, int>> outerSector = new();

    static Random random = new();

    public static void InitMap()
    {
        DefineColorSectors();
        GenerateRooms(size);
        DefineBorders();
        DefineDiagonals();
        DefineDiagonalN1();
        DefineReverseDiagonalN1();
        DefineKeysLocations();
        DefineSectorsOrder();
        DefineRingSectors();
    }

    static void DefineRingSectors()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int distanceX = Math.Abs(i - size / 2);
                int distanceY = Math.Abs(j - size / 2);
                if (distanceX <= size / 4 && distanceY <= size / 4)
                {
                    innerSector.Add(Tuple.Create(j, i));
                }
                else
                {
                    outerSector.Add(Tuple.Create(j, i));
                }
            }
        }
    }

    public static Tuple<int, int> GetLeftNeighbor(Tuple<int, int> roomIndex)
    {
        int i = roomIndex.Item1;
        int j = roomIndex.Item2;
        if (ContainsTuple(borderLeft, Tuple.Create(i, j)))
            return null;
        return Tuple.Create(i - 1, j);
    }

    public static Tuple<int, int> GetRightNeighbor(Tuple<int, int> roomIndex)
    {
        int i = roomIndex.Item1;
        int j = roomIndex.Item2;
        if (ContainsTuple(borderRight, Tuple.Create(i, j)))
            return null;
        return Tuple.Create(i + 1, j);
    }

    public static Tuple<int, int> GetTopNeighbor(Tuple<int, int> roomIndex)
    {
        int i = roomIndex.Item1;
        int j = roomIndex.Item2;
        if (ContainsTuple(borderTop, Tuple.Create(i, j)))
            return null;
        return Tuple.Create(i, j + 1);
    }

    public static Tuple<int, int> GetBottomNeighbor(Tuple<int, int> roomIndex)
    {
        int i = roomIndex.Item1;
        int j = roomIndex.Item2;
        if (ContainsTuple(borderBottom, Tuple.Create(i, j)))
            return null;
        return Tuple.Create(i, j - 1);
    }
    
    public static Neighbors DefineNeighbors(Tuple<int, int> roomIndex)
    {
        int i = roomIndex.Item1;
        int j = roomIndex.Item2;
        var neigbors = new Neighbors();

        // left-bottom corner
        if (i == 0 && j == 0)
        {
            neigbors.topRoom = Tuple.Create(i, j + 1);
            neigbors.leftRoom = null;
            neigbors.rightRoom = Tuple.Create(i + 1, j);
            neigbors.bottomRoom = null;
            return neigbors;
        }
        // left-top corner
        if (i == 0 && j == cols - 1)
        {
            neigbors.topRoom = null;
            neigbors.leftRoom = null;
            neigbors.rightRoom = Tuple.Create(i + 1, j);
            neigbors.bottomRoom = Tuple.Create(i, j - 1);
            return neigbors;
        }
        //right-bottom corner
        if (i == rows - 1 && j == 0)
        {
            neigbors.topRoom = Tuple.Create(i, j + 1);
            neigbors.leftRoom = Tuple.Create(i - 1, j);
            neigbors.rightRoom = null;
            neigbors.bottomRoom = null;
            return neigbors;
        }
        // right-top corner
        if (i == rows - 1 && j == cols - 1)
        {
            neigbors.topRoom = null;
            neigbors.leftRoom = Tuple.Create(i - 1, j);
            neigbors.rightRoom = null;
            neigbors.bottomRoom = Tuple.Create(i, j - 1);
            return neigbors;
        }
        if (ContainsTuple(borderTop, Tuple.Create(i, j)))
        {
            neigbors.topRoom = null;
            neigbors.leftRoom = Tuple.Create(i - 1, j);
            neigbors.rightRoom = Tuple.Create(i + 1, j);
            neigbors.bottomRoom = Tuple.Create(i, j - 1);
            return neigbors;
        }
        if (ContainsTuple(borderBottom, Tuple.Create(i, j)))
        {
            neigbors.topRoom = Tuple.Create(i, j + 1);
            neigbors.leftRoom = Tuple.Create(i - 1, j);
            neigbors.rightRoom = Tuple.Create(i + 1, j);
            neigbors.bottomRoom = null;
            return neigbors;
        }
        if (ContainsTuple(borderLeft, Tuple.Create(i, j)))
        {
            neigbors.topRoom = Tuple.Create(i, j + 1);
            neigbors.leftRoom = null;
            neigbors.rightRoom = Tuple.Create(i + 1, j);
            neigbors.bottomRoom = Tuple.Create(i, j - 1);
            return neigbors;
        }
        if (ContainsTuple(borderRight, Tuple.Create(i, j)))
        {
            neigbors.topRoom = Tuple.Create(i, j + 1);
            neigbors.leftRoom = Tuple.Create(i - 1, j);
            neigbors.rightRoom = null;
            neigbors.bottomRoom = Tuple.Create(i, j - 1);
            return neigbors;
        }
        neigbors.topRoom = Tuple.Create(i, j + 1);
        neigbors.leftRoom = Tuple.Create(i - 1, j);
        neigbors.rightRoom = Tuple.Create(i + 1, j);
        neigbors.bottomRoom = Tuple.Create(i, j - 1);
        return neigbors;
    }

    static void DefineDiagonalN1()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i > 0) // Check element above
                {
                    diagonalPrimaryN1.Add(Tuple.Create(i - 1, i));
                }
                if (i < rows - 1) // Check element below
                {
                    diagonalPrimaryN1.Add(Tuple.Create(i + 1, i));
                }
                if (i > 0 && j > 0) // Check upper-left neighbor (diagonally)
                {
                    diagonalPrimaryN1.Add(Tuple.Create(i - 1, i - 1));
                }
                if (i < rows - 1 && j < cols - 1) // Check lower-right neighbor (diagonally)
                {
                    diagonalPrimaryN1.Add(Tuple.Create(i + 1, i + 1));
                }
            }
        }
    }

    static void DefineReverseDiagonalN1()
    {
        int i = Math.Min(rows - 1, cols - 1);
        int j = Math.Max(rows - 1, cols - 1) - i;

        while (i >= 0 && j >= 0)
        {
            // Left neighbor
            if (j > 0)
            {
                diagonalSecondaryN1.Add(Tuple.Create(i, j - 1));
            }
            // Lower-left neighbor (diagonally)
            if (i < rows - 1 && j > 0)
            {
                diagonalSecondaryN1.Add(Tuple.Create(i + 1, j - 1));
            }
            // Right neighbor (within same row)
            if (j < cols - 1)
            {
                diagonalSecondaryN1.Add(Tuple.Create(i, j + 1));
            }
            // Upper-right neighbor (diagonally)
            if (i > 0 && j < cols - 1)
            {
                diagonalSecondaryN1.Add(Tuple.Create(i - 1, j + 1));
            }
            i--;
            j++;
        }
    }

    static void DefineKeysLocations()
    {
        List<Tuple<int, int>> areaTop = new();
        List<Tuple<int, int>> areaLeft = new();
        List<Tuple<int, int>> areaRight = new();
        List<Tuple<int, int>> areaBottom = new();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var idx = Tuple.Create(i, j);
                if (diagonalPrimaryN1.Contains(idx) || diagonalSecondaryN1.Contains(idx))
                {
                    continue;
                }

                int distanceX = Math.Abs(i - size / 2);
                int distanceY = Math.Abs(j - size / 2);

                if (rooms[i, j] == colorSectors.GetValueOrDefault("top") && distanceY > 1)
                {
                    areaTop.Add(Tuple.Create(i, j));
                }
                if (rooms[i, j] == colorSectors.GetValueOrDefault("bottom") && distanceY > 1)
                {
                    areaBottom.Add(Tuple.Create(i, j));
                }
                if (rooms[i, j] == colorSectors.GetValueOrDefault("right") && distanceX > 2)
                {
                    areaRight.Add(Tuple.Create(i, j));
                }
                if (rooms[i, j] == colorSectors.GetValueOrDefault("left") && distanceX > 2)
                {
                    areaLeft.Add(Tuple.Create(i, j));
                }
            }
        }

        keysTop.Add(areaTop[random.Next(areaTop.Count)]);
        keysLeft.Add(areaLeft[random.Next(areaLeft.Count)]);
        keysRight.Add(areaRight[random.Next(areaRight.Count)]);
        keysBottom.Add(areaBottom[random.Next(areaBottom.Count)]);
    }

    static void DefineBorders()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i == 0)
                {
                    borderLeft.Add(Tuple.Create(i, j));
                }
                else if (i == rows - 1)
                {
                    borderRight.Add(Tuple.Create(i, j));
                }
                else if (j == 0)
                {
                    borderBottom.Add(Tuple.Create(i, j));
                }
                else if (j == cols - 1)
                {
                    borderTop.Add(Tuple.Create(i, j));
                }
            }
        }
        borders = borderBottom
            .Concat(borderTop)
            .Concat(borderLeft)
            .Concat(borderRight)
            .ToList();
    }

    static void DefineDiagonals()
    {

        for (int i = 0; i < rows && i < cols; i++)
        {
            diagonalPrimary.Add(Tuple.Create(i, i));
        }

        for (int i = 0; i < rows; i++)
        {
            diagonalSecondary.Add(Tuple.Create(i, cols - i - 1));
        }
    }

    static void GenerateRooms(int size)
    {
        rooms = new string[size, size];
        rooms_debug = new string[size, size];
        rows = rooms.GetLength(0);
        cols = rooms.GetLength(1);
        center = Tuple.Create(rows / 2, cols / 2);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int X = i - size / 2;
                int Y = j - size / 2;
                int distanceX = Math.Abs(X);
                int distanceY = Math.Abs(Y);

                if (distanceX <= distanceY && distanceX + distanceY <= size - 1)
                {
                    if ((X >= 0 && Y <= 0) || (X <= 0 && Y <= 0))
                    {
                        rooms[i, j] = colorSectors.GetValueOrDefault("bottom");
                        
                    }
                    else
                    {
                        rooms[i, j] = colorSectors.GetValueOrDefault("top");
                        roomsTop.Add(Tuple.Create(i, j));
                    }
                }
                else if (distanceX >= distanceY && distanceX + distanceY <= size - 1)
                {
                    if ((X >= 0 && Y <= 0) || (X >= 0 && Y >= 0))
                    {
                        rooms[i, j] = colorSectors.GetValueOrDefault("right");
                        roomsRight.Add(Tuple.Create(i, j));
                    }
                    else
                    {
                        rooms[i, j] = colorSectors.GetValueOrDefault("left");
                        roomsLeft.Add(Tuple.Create(i, j));
                    }
                }
            }
        }
        rooms[size / 2, size / 2] = "G:NO_KEY:NO_TYPE";
    }

    static void DefineColorSectors()
    {
        string[] colors = {
            "A:NO_KEY:NO_TYPE",
            "V:NO_KEY:NO_TYPE",
            "O:NO_KEY:NO_TYPE",
            "G:NO_KEY:NO_TYPE"
        };
        colors = Shuffle(colors);
        colorSectors = new Dictionary<string, string>
        {
            { "top", colors[0] },
            { "left", colors[1] },
            { "right", colors[2] },
            { "bottom", colors[3] }
        };
    }

    static void DefineSectorsOrder()
    {
        // debug
        Array.Copy(rooms, 0, rooms_debug, 0, rooms.Length);

        start_sector = colorSectors.FirstOrDefault(x => x.Value.StartsWith("G")).Key;
        switch (start_sector)
        {
            case "top":
                end_sector = "bottom";
                (second_sector, third_sector) = ("right", "left");
                AppendKeyData(keysTop, colorSectors[second_sector]);
                AppendKeyData(keysRight, colorSectors[third_sector]);
                AppendKeyData(keysLeft, colorSectors[end_sector]);
                AppendKeyData(keysBottom, colorSectors[start_sector]);
                AppendAlarmData(keysBottom, colorSectors[start_sector]);
                break;
            case "bottom":
                end_sector = "top";
                (second_sector, third_sector) = ("left", "right");
                AppendKeyData(keysBottom, colorSectors[second_sector]);
                AppendKeyData(keysLeft, colorSectors[third_sector]);
                AppendKeyData(keysRight, colorSectors[end_sector]);
                AppendKeyData(keysTop, colorSectors[start_sector]);
                AppendAlarmData(keysTop, colorSectors[start_sector]);
                break;
            case "right":
                end_sector = "left";
                (second_sector, third_sector) = ("top", "bottom");
                AppendKeyData(keysRight, colorSectors[second_sector]);
                AppendKeyData(keysTop, colorSectors[third_sector]);
                AppendKeyData(keysBottom, colorSectors[end_sector]);
                AppendKeyData(keysLeft, colorSectors[start_sector]);
                AppendAlarmData(keysLeft, colorSectors[start_sector]);
                break;
            case "left":
                end_sector = "right";
                (second_sector, third_sector) = ("bottom", "top");
                AppendKeyData(keysLeft, colorSectors[second_sector]);
                AppendKeyData(keysBottom, colorSectors[third_sector]);
                AppendKeyData(keysTop, colorSectors[end_sector]);
                AppendKeyData(keysRight, colorSectors[start_sector]);
                AppendAlarmData(keysRight, colorSectors[start_sector]);
                break;
        }
    }

    static void AppendKeyData(List<Tuple<int, int>> items, string room_data)
    {
        var item = items[0];
        rooms[item.Item1, item.Item2] = rooms[item.Item1, item.Item2].Replace("NO_KEY", room_data.Split(':')[0]);
        // debug
        rooms_debug[item.Item1, item.Item2] = room_data.Split(':')[0] + rooms[item.Item1, item.Item2].Substring(1);
    }

    static void AppendAlarmData(List<Tuple<int, int>> items, string room_data)
    {
        var item = items[0];
        rooms[item.Item1, item.Item2] = rooms[item.Item1, item.Item2] + ":ALARM";
    }

    static void RenderRoom(int x, int z, ColorState color = ColorState.VIOLET)
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = string.Format(box_name, x, z);
        box.transform.position = new Vector3(x, -10, z);
        box.AddComponent<ColorStateApplier>();
        box.GetComponent<ColorStateApplier>().sourceColor = color;
    }

    public static void RenderMap()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                RenderRoom(i, j, SrtToColor(rooms_debug[i, j]));
            }
        }
    }

    public static ColorState SrtToColor(string room_data)
    {
        switch (room_data.Split(':')[0])
        {
            case "W":
                return ColorState.WHITE;
            case "B":
                return ColorState.BLACK;
            case "G":
                return ColorState.GRAY;
            case "A":
                return ColorState.AQUA;
            case "V":
                return ColorState.VIOLET;
            case "O":
                return ColorState.ORANGE;
        }
        return ColorState.GRAY;
    }

    static bool ContainsTuple(List<Tuple<int, int>> list, Tuple<int, int> target)
    {
        return list.Any(tuple => tuple.Item1 == target.Item1 && tuple.Item2 == target.Item2);
    }

    static string[] Shuffle(string[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (array[j], array[i]) = (array[i], array[j]);
        }
        return array;
    }
}
