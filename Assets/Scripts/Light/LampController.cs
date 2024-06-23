using UnityEngine;

public class LampController : MonoBehaviour
{
    public float lightFadingSpeed = 0.04f;
    void Update()
    {
        GetComponent<Light>().intensity -= lightFadingSpeed * Time.deltaTime;
    }
}
