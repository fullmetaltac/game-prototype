using UnityEngine;

public class CharacterMaterialChanger : MonoBehaviour
{
    public Material material;
    public Color color = Color.cyan;

    public bool updateColor = true;
    public static CharacterMaterialChanger instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (updateColor)
        {
            material.color = color;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color);
            updateColor = false;
        }

    }
}
