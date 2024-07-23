using UnityEngine;
using System.Collections;

public enum CageState
{
    UP, DOWN, MOVING
}
public class ControllerCage : MonoBehaviour
{
    public float height;
    public CageState state;
    public float moveSpeed = 10f;
    public Vector3 moveDirection;

    private void Start()
    {
        state = CageState.DOWN;
        height = GetComponent<Renderer>().bounds.size.y;
        moveDirection = new Vector3(0f, 0f, 0f);
    }

    public void MoveUp()
    {
        if (state == CageState.DOWN)
        {
            moveDirection.y = 1f;
            state = CageState.MOVING;
            StartCoroutine(MoveCoroutine(Vector3.up, CageState.UP));
        }
    }

    public void MoveDown()
    {
        if (state == CageState.UP)
        {
            moveDirection.y = -1f;
            state = CageState.MOVING;
            StartCoroutine(MoveCoroutine(Vector3.down, CageState.DOWN));
        }
    }

    IEnumerator MoveCoroutine(Vector3 moveDirection, CageState setState)
    {
        float elapsedTime = 0f;
        float moveDuration = height / moveSpeed;
        Vector3 targetPosition = transform.position + moveDirection * height;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, moveDirection.y * height / 2, transform.position.z);
        state = setState;
    }
}
