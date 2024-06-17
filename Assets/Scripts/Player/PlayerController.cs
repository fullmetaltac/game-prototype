using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public GameObject playerModel;
    public CharacterController charController;

    public static PlayerController instance;

    public float moveSpeed;
    public float jumpForce;
    public float rotateSpeed;
    public float gravityScale = 5f;
    private bool canDoubleJump = true;

    //teleport
    public bool isFreeMove = true;
    private Vector3 fixedDirection;

    // dash
    public float dashAmount = 3f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1.0f;
    private bool isDashing = false;
    private bool canDash = true;

    private Vector3 moveDirection;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //playerModel.transform.Rotate(0f, 180f, 0f);
    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        float yStore = moveDirection.y;
        if (!isFreeMove)
        {
            moveDirection = fixedDirection;
        }
        else
        {
            var camTransform = Camera.main.transform;
            float verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            moveDirection = camTransform.forward * verticalInput + camTransform.right * horizontalInput;
        }
        moveDirection.Normalize();
        moveDirection *= moveSpeed;
        moveDirection.y = yStore;


        if (charController.isGrounded)
        {
            moveDirection.y = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
                canDoubleJump = true;
            }
        }
        else if (!charController.isGrounded && canDoubleJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                canDoubleJump = false;
                moveDirection.y = 0f;
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

        charController.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        anim.SetBool("Grounded", charController.isGrounded);
    }

    public void SetFixedDirection(Vector3 vector)
    {
        fixedDirection = vector;
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            var dashDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            charController.Move((dashDirection + dashDirection * dashAmount) * Time.deltaTime);
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}