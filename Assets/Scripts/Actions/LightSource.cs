using UnityEngine;

public enum ColorState
{
    YELLOW = 0,
    VIOLET = 1,
    ORANGE = 2,
    AQUA = 3,

    // DEBUG
    GRAY = -1,
    BLACK = -2,
    WHITE = -3,
}

public class ColorConstants
{
    public static Color AquaColor = new Color(0.360f, 0.819f, 0.679f, 1);
    public static Color YellowColor = new Color(0.801f, 0.465f, 0.276f, 1);
    public static Color OrangeColor = new Color(0.801f, 0.465f, 0.276f, 1);
    public static Color VioletColor = new Color(0.411f, 0.360f, 0.819f, 1);

    // DEBUG
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
            case ColorState.YELLOW:
                return ColorConstants.YellowColor;
            case ColorState.AQUA:
                return ColorConstants.AquaColor;
            case ColorState.ORANGE:
                return ColorConstants.OrangeColor;
            case ColorState.VIOLET:
                return ColorConstants.VioletColor;
            // DEBUG
            case ColorState.GRAY:
                return ColorConstants.GrayColor;
            case ColorState.BLACK:
                return ColorConstants.BlackColor;
            case ColorState.WHITE:
                return ColorConstants.WhiteColor;
        }
        return ColorConstants.AquaColor;
    }
}
public class LightSource : MonoBehaviour
{
    public float rayDistance = 30f;

    private MoveCage cage;
    private RaycastHit previousHit;

    void Update()
    {
        int layerMask = 1 << LayerMask.NameToLayer("IgnoreRaycast");
        layerMask = ~layerMask; 

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDistance, layerMask))
        {
            if (hit.collider.isTrigger && hit.collider.gameObject.tag == "Cage")
            {
                if (previousHit.collider != hit.collider)
                {
                    cage = hit.collider.gameObject.GetComponent<MoveCage>();
                    cage.MoveUp();
                }
            }
            previousHit = hit;
        }
        else
        {
            if (previousHit.collider != null && previousHit.collider.isTrigger && previousHit.collider.gameObject.tag == "Cage")
            {
                cage.MoveDown();
            }
            previousHit = new RaycastHit();
        }
    }
}
