using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSInputController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [Header("References")]
    [SerializeField] private Transform cameraRoot;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 7.5f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float jumpHeight = 1.6f;
    [SerializeField] private float gravity = -25f;

    [Header("Look")]
    [SerializeField] private float lookSensitivity = 1.6f;
    [SerializeField] private float maxLookAngle = 85f;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchCenterY = 0.6f;

    private CharacterController controller;
    private InputSystem_Actions actions;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private bool sprintHeld;
    private bool crouchHeld;

    private float verticalVelocity;
    private float pitch;
    private float originalHeight;
    private Vector3 originalCenter;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        originalCenter = controller.center;

        if (cameraRoot == null)
        {
            var cam = Camera.main;
            if (cam != null)
            {
                cameraRoot = cam.transform;
            }
        }

        actions = new InputSystem_Actions();
        actions.Player.AddCallbacks(this);
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }

    private void OnDestroy()
    {
        actions.Dispose();
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        HandleCrouch();
    }

    private void HandleLook()
    {
        if (cameraRoot == null)
        {
            return;
        }

        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        cameraRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        float speed = walkSpeed;
        if (crouchHeld)
        {
            speed = crouchSpeed;
        }
        else if (sprintHeld)
        {
            speed = sprintSpeed;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * (speed * Time.deltaTime));

        if (jumpPressed && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        jumpPressed = false;

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * (verticalVelocity * Time.deltaTime));
    }

    private void HandleCrouch()
    {
        if (crouchHeld)
        {
            controller.height = Mathf.MoveTowards(controller.height, crouchHeight, Time.deltaTime * 8f);
            var center = controller.center;
            center.y = Mathf.MoveTowards(center.y, crouchCenterY, Time.deltaTime * 8f);
            controller.center = center;
        }
        else
        {
            controller.height = Mathf.MoveTowards(controller.height, originalHeight, Time.deltaTime * 8f);
            var center = controller.center;
            center.y = Mathf.MoveTowards(center.y, originalCenter.y, Time.deltaTime * 8f);
            controller.center = center;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context) { }

    public void OnInteract(InputAction.CallbackContext context) { }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            crouchHeld = true;
        }
        else if (context.canceled)
        {
            crouchHeld = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
    }

    public void OnPrevious(InputAction.CallbackContext context) { }

    public void OnNext(InputAction.CallbackContext context) { }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprintHeld = true;
        }
        else if (context.canceled)
        {
            sprintHeld = false;
        }
    }
}
