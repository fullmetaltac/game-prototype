using UnityEngine;

public class ColorInverser : MonoBehaviour
{
    public ParticleSystem _particleSystem;

    private void OnTriggerStay(Collider other)
    {
        PlayerUtil.PlayerAction(other, (int)BackpackController.backPackState > 2, () =>
        {
            BackpackController.instance.InverseRotation();
            _particleSystem.Play();
        });
    }
}
