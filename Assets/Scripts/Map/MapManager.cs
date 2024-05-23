using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    public int size = 13;
    private int rows;
    private int cols;
    private char[,] rooms;
    private readonly string box_name = "Box[{0},{1}]";
    private static Dictionary<string, char> colorSectors;

    List<Tuple<int, int>> borders;
    List<Tuple<int, int>> bordersN1;
    List<Tuple<int, int>> diagonalPrimary;
    List<Tuple<int, int>> diagonalPrimaryN1;
    List<Tuple<int, int>> diagonalSecondary;
    List<Tuple<int, int>> diagonalSecondaryN1;

    List<Tuple<int, int>> keys;
    List<Tuple<int, int>> roomsTop;
    List<Tuple<int, int>> roomsBottom;
    List<Tuple<int, int>> roomsRight;
    List<Tuple<int, int>> roomsLeft;



    void Start()
    {
        DefineColorSectors();
        GenerateRooms(size);

        DefineBorders();
        DefineDiagonals();
        DefineDiagonalN1();
        DefineReverseDiagonalN1();
        DefineKeysLocations();

        //ChangeColor(diagonalPrimary, 'B');
        //ChangeColor(diagonalSecondary, 'B');

        ChangeColor(keys, 'W');
        RenderMap(rooms);
    }

    public void DefineDiagonalN1()
    {
        diagonalPrimaryN1 = new();

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

    public void DefineReverseDiagonalN1()
    {
        diagonalSecondaryN1 = new();

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

    public void DefineKeysLocations()
    {
        keys = new();

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

                if (rooms[i, j] == colorSectors.GetValueOrDefault("top"))
                {
                    areaTop.Add(Tuple.Create(i, j));
                }
                if (rooms[i, j] == colorSectors.GetValueOrDefault("bottom"))
                {
                    areaBottom.Add(Tuple.Create(i, j));
                }
                if (rooms[i, j] == colorSectors.GetValueOrDefault("right") && distanceX > 1)
                {
                    areaRight.Add(Tuple.Create(i, j));
                }
                if (rooms[i, j] == colorSectors.GetValueOrDefault("left") && distanceX > 1)
                {
                    areaLeft.Add(Tuple.Create(i, j));
                }
            }
        }

        Random random = new();
        keys.Add(areaTop[random.Next(areaTop.Count)]);
        keys.Add(areaLeft[random.Next(areaTop.Count)]);
        keys.Add(areaRight[random.Next(areaTop.Count)]);
        keys.Add(areaBottom[random.Next(areaTop.Count)]);
    }

    public void DefineBorders()
    {
        borders = new();
        bordersN1 = new();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i == 0 || i == rows - 1 || j == 0 || j == cols - 1)
                {
                    borders.Add(Tuple.Create(i, j));
                }
                else if (i == 1 || i == rows - 2 || j == 1 || j == cols - 2)
                {
                    bordersN1.Add(Tuple.Create(i, j));
                }
            }
        }
    }

    public void DefineDiagonals()
    {
        diagonalPrimary = new();
        diagonalSecondary = new();

        for (int i = 0; i < rows && i < cols; i++)
        {
            diagonalPrimary.Add(Tuple.Create(i, i));
        }

        for (int i = 0; i < rows; i++)
        {
            diagonalSecondary.Add(Tuple.Create(i, cols - i - 1));
        }
    }

    public void GenerateRooms(int size)
    {
        roomsTop = new();
        roomsLeft = new();
        roomsRight = new();
        roomsBottom = new();

        rooms = new char[size, size];
        rows = rooms.GetLength(0);
        cols = rooms.GetLength(1);

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
                        roomsBottom.Add(Tuple.Create(i, j));
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
        rooms[size / 2, size / 2] = 'G';
    }

    private void DefineColorSectors()
    {
        char[] colors = { 'A', 'V', 'O', 'G' };
        colors = Shuffle(colors);
        colorSectors = new Dictionary<string, char>
        {
            { "top", colors[0] },
            { "left", colors[1] },
            { "right", colors[2] },
            { "bottom", colors[3] }
        };
    }

    void ChangeColor(List<Tuple<int, int>> items, char color)
    {
        items.ForEach(b => { rooms[b.Item1, b.Item2] = color; });
    }
    void RenderRoom(int x, int z, ColorState color = ColorState.VIOLET)
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = string.Format(box_name, x, z);
        box.transform.position = new Vector3(x, 0, z);
        box.AddComponent<ColorStateApplier>();
        box.GetComponent<ColorStateApplier>().sourceColor = color;
    }

    public void RenderMap(char[,] array)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                switch (array[i, j])
                {
                    case 'W':
                        RenderRoom(i, j, ColorState.WHITE);
                        break;
                    case 'B':
                        RenderRoom(i, j, ColorState.BLACK);
                        break;
                    case 'G':
                        RenderRoom(i, j, ColorState.GRAY);
                        break;
                    case 'A':
                        RenderRoom(i, j, ColorState.AQUA);
                        break;
                    case 'V':
                        RenderRoom(i, j, ColorState.VIOLET);
                        break;
                    case 'O':
                        RenderRoom(i, j, ColorState.ORANGE);
                        break;
                }
            }
        }
    }

    public char[] Shuffle(char[] array)
    {
        Random rng = new();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            char temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        return array;
    }
}
