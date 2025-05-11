using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveZ = Animator.StringToHash("moveZ");

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform cameraTransform;

    private Vector3 _velocity;
    private bool _isGrounded;

    private void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        _isGrounded = Physics.CheckSphere(transform.position + Vector3.down * 0.1f, groundCheckDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; 
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * inputZ + camRight * inputX;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        Vector3 localInput = transform.InverseTransformDirection(moveDirection);
        animator.SetFloat(MoveX, localInput.x);
        animator.SetFloat(MoveZ, localInput.z);

        if (moveDirection.magnitude > 0.1f && localInput.z >= 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        Vector3 finalMove = moveDirection * movementSpeed;
        _velocity.y += gravity * Time.deltaTime;
        finalMove.y = _velocity.y;

        controller.Move(finalMove * Time.deltaTime);
    }
}