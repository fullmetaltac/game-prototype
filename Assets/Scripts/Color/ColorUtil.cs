using UnityEngine;

public enum ColorState
{
    AQUA = 0,
    PINK = 1,
    VIOLET = 2
}

public class ColorConstants
{
    public static Color AquaColor = new Color(0.360f, 0.819f, 0.679f, 1);
    public static Color PinkColor = new Color(0.819f, 0.360f, 0.717f, 1);
    public static Color VioletColor = new Color(0.411f, 0.360f, 0.819f, 1);

}

public class ColorUtil
{
    public static Color StateToColor(ColorState colorState)
    {
        switch (colorState)
        {
            case ColorState.AQUA:
                return ColorConstants.AquaColor;
            case ColorState.PINK:
                return ColorConstants.PinkColor;
            case ColorState.VIOLET:
                return ColorConstants.VioletColor;
        }
        return ColorConstants.AquaColor;
    }
}
