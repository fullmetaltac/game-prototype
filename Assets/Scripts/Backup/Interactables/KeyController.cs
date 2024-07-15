using UnityEngine;

public class KeyController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var color = GetComponent<ColorStateApplier>().sourceColor;
            GameManager_V0.instance.keys.Add(color);
            Destroy(gameObject);
        }
    }
}
