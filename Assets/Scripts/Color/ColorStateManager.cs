using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ColorStateManager : MonoBehaviour
{
    public static ColorStateManager instance;

    public static ColorState colorState
    {
        get => (ColorState)PlayerPrefs.GetInt("COLOR_STATE", 0);
        set => PlayerPrefs.SetInt("COLOR_STATE", (int)value);
    }

    private List<IColorSubscriber> subscribers = new List<IColorSubscriber>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Resources.FindObjectsOfTypeAll<ColorMaterial>().ToList().ForEach(x => subscribers.Add(x));
        Resources.FindObjectsOfTypeAll<EmissiveMaterial>().ToList().ForEach(x => subscribers.Add(x));
        NotifySubscribers();
    }

    internal void UpdateState(ColorState colorState)
    {
        ColorStateManager.colorState = colorState;
        NotifySubscribers();
    }

    public void NotifySubscribers()
    {
        subscribers.ForEach(x => x.OnColorStateChange());
    }

}