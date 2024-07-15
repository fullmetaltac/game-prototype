using System;
using UnityEngine;


public enum Tags
{
    Player = 1,
}

public class PlayerConstants
{
    public static KeyCode ACTION = KeyCode.E;
    public static KeyCode ROTATE_BACKPACK = KeyCode.Q;
}

public class PlayerUtil
{
    public static void PlayerAction(Collider other, Action action, bool condition = true)
    {
        if (IsTag(other) && Input.GetKeyDown(PlayerConstants.ACTION) && condition)
        {
            action();
        }
    }

    public static bool IsTag(Collider other, Tags tag = Tags.Player)
    {
        return other.tag == tag.ToString();
    }
}
