using UnityEngine;

public enum ColorState
{
    AQUA = 0,
    VIOLET = 1,
    ORANGE = 2
}

public class ColorConstants
{
    public static Color AquaColor = new Color(0.360f, 0.819f, 0.679f, 1);
    public static Color OrangeColor = new Color(0.801f, 0.465f, 0.276f, 1);
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
            case ColorState.ORANGE:
                return ColorConstants.OrangeColor;
            case ColorState.VIOLET:
                return ColorConstants.VioletColor;
        }
        return ColorConstants.AquaColor;
    }
}
