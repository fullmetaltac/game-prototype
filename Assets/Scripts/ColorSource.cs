using UnityEngine;

public class ColorSource : MonoBehaviour
{
    public Color color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CharacterMaterialChanger.instance.color = color;
            CharacterMaterialChanger.instance.updateColor = true;
        }
    }
}
