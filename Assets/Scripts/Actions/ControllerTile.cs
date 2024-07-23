using UnityEngine;
using System.Collections;

public class ControllerTile : MonoBehaviour
{
    private float height;
    private bool touched = false;
    private float moveSpeed = 10f;

    private void Start()
    {
        height = GetComponent<Renderer>().bounds.size.y * 0.5f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !touched)
        {
            touched = true;
            SourceCage.isCageSourceActive = true;
            SourceLight.isLightSourceActive = true;
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(MoveCoroutine());
        }
    }

    IEnumerator MoveCoroutine()
    {
        float elapsedTime = 0f;
        float moveDuration = height / moveSpeed;
        Vector3 targetPosition = transform.position + Vector3.down * height;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
