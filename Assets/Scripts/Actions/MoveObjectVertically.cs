using UnityEngine;

public class MoveObjectVertically : MonoBehaviour
{
    public float speed = 10f;

    public float height;
    private Vector3 moveDirection;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = GetComponent<Transform>().position;
        height = GetComponent<Renderer>().bounds.size.y;
        moveDirection = new Vector3(0f, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            moveDirection.y = -1f;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            moveDirection.y = 1f;
    }

    private void Update()
    {
        var movement = moveDirection * speed * Time.deltaTime;
        if (moveDirection.y < 0)
        {
            if (transform.position.y > -height * 0.5)
            {
                transform.position += movement;
            }
        }
        if (moveDirection.y > 0)
        {
            if (transform.position.y < height * 0.5)
            {
                transform.position += movement;
            }

        }
    }
}
