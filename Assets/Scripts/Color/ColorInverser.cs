using UnityEngine;

public class ColorInverser : MonoBehaviour
{
    public ParticleSystem _particleSystem;

    private void OnTriggerStay(Collider other)
    {
        PlayerUtil.PlayerAction(other, () =>
        {
            BackpackController.instance.InverseRotation();
            _particleSystem.Play();
        });
    }
}
