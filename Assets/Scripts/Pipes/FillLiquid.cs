using UnityEngine;

public class FillLiquid : MonoBehaviour
{    
    public float fillSpeed = 0.1f;
    public float currentLevel = 0.1f;

    private Renderer _renderer;
    private bool _enabled = false;
    

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetFloat("_FillAmount", currentLevel);
    }

    private void Update()
    {
        if (_enabled && currentLevel < 1f)
        {
            currentLevel += fillSpeed * Time.deltaTime;
            _renderer.material.SetFloat("_FillAmount", currentLevel);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(PlayerConstants.ACTION))
        {
            _enabled = true;
        }
    }
}
