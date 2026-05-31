using UnityEngine;

public class FirstPersonControllerr : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandlers playerInputHandlers;

    private Vector3 currentMovement;
    private float verticalRotation;

    private float CurrentSpeed =>
        walkSpeed * (playerInputHandlers.SprintTriggered ? sprintMultiplier : 1);

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 input = new Vector3(
            playerInputHandlers.MovementInput.x, 0f,
            playerInputHandlers.MovementInput.y);
        Vector3 worldDir = transform.TransformDirection(input).normalized;

        currentMovement.x = worldDir.x * CurrentSpeed;
        currentMovement.z = worldDir.z * CurrentSpeed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;
            if (playerInputHandlers.JumpTriggered)
                currentMovement.y = jumpForce;
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    private void HandleRotation()
    {
        float mouseX = playerInputHandlers.RotationInput.x * mouseSensitivity;
        float mouseY = playerInputHandlers.RotationInput.y * mouseSensitivity;

        transform.Rotate(0, mouseX, 0);

        verticalRotation = Mathf.Clamp(
            verticalRotation - mouseY, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}