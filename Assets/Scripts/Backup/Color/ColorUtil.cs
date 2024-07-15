using UnityEngine;

public enum ColorState
{   WHITE = -3,
    BLACK = -2,
    GRAY = -1,
    YELLOW = 0,
    VIOLET = 1,
    ORANGE = 2,
    AQUA = 3,
}

public class ColorConstants
{
    public static Color AquaColor = new Color(0.360f, 0.819f, 0.679f, 1);
    public static Color OrangeColor = new Color(0.801f, 0.465f, 0.276f, 1);
    public static Color VioletColor = new Color(0.411f, 0.360f, 0.819f, 1);

    public static Color GrayColor = new Color(0.487f, 0.514f, 0.518f, 1);
    public static Color BlackColor = new Color(0f, 0f, 0f, 1);
    public static Color WhiteColor = new Color(1f, 1f, 1f, 1);


}

public class ColorUtil
{
    public static Color StateToColor(ColorState colorState)
    {
        switch (colorState)
        {
            case ColorState.WHITE:
                return ColorConstants.WhiteColor;
            case ColorState.BLACK:
                return ColorConstants.BlackColor;
            case ColorState.GRAY:
                return ColorConstants.GrayColor;
            case ColorState.AQUA:
                return ColorConstants.AquaColor;
            case ColorState.ORANGE:
                return ColorConstants.OrangeColor;
            case ColorState.VIOLET:
                return ColorConstants.VioletColor;
        }
        return ColorConstants.AquaColor;
    }
}
