using System;
using UnityEngine;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    private readonly string box_name = "Box[{0},{1}]";

    void Start()
    {
        var charGrid = GenerateRooms(13);
        RenderRooms(charGrid);
    }

    public static char[,] GenerateRooms(int size)
    {

        if (Math.Abs(size % 2) != 1)
        {
            throw new ArgumentException("Array size must be odd for a center element");
        }

        char[] colors = { 'A', 'V', 'O', 'G' };
        colors = Shuffle(colors);

        char[,] array = new char[size, size];

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
                        array[i, j] = colors[0]; // bottom
                    }
                    else
                    {
                        array[i, j] = colors[1]; // top
                    }
                }
                else if (distanceX >= distanceY && distanceX + distanceY <= size - 1)
                {
                    if ((X >= 0 && Y <= 0) || (X >= 0 && Y >= 0))
                    {
                        array[i, j] = colors[2]; // right
                    }
                    else
                    {
                        array[i, j] = colors[3]; // left
                    }
                }
            }
        }
        array[size / 2, size / 2] = 'G';

        return array;
    }

    void RenderRoom(int x, int z, ColorState color = ColorState.VIOLET)
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = string.Format(box_name, x, z);
        box.transform.position = new Vector3(x, 0, z);
        box.AddComponent<ColorStateApplier>();
        box.GetComponent<ColorStateApplier>().sourceColor = color;
    }

    public void RenderRooms(char[,] array)
    {

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                switch (array[i, j])
                {
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

    public static char[] Shuffle(char[] array)
    {
        Random rng = new Random();
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
