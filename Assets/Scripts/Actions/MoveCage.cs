using UnityEngine;
using System.Collections;

public class MoveCage : MonoBehaviour
{
    public float height;
    public float moveSpeed = 5f;
    public Vector3 moveDirection;

    private void Start()
    {
        height = GetComponent<Renderer>().bounds.size.y;
        moveDirection = new Vector3(0f, 0f, 0f);
    }

    public void MoveUp()
    {
        moveDirection.y = 1f;
        StartCoroutine(MoveCoroutine(Vector3.up));
    }

    public void MoveDown()
    {
        moveDirection.y = -1f;
        StartCoroutine(MoveCoroutine(Vector3.down));
    }

    IEnumerator MoveCoroutine(Vector3 moveDirection)
    {
        float elapsedTime = 0f;
        float moveDuration = height / moveSpeed;

        while (elapsedTime < moveDuration)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
