using UnityEngine;
using UnityEngine.InputSystem;

namespace WorldInteractionSystem.Runtime.Player
{
    /// <summary>
    /// First-person input controller using Unity's Input System.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FPSInputController : MonoBehaviour, InputSystem_Actions.IPlayerActions
    {
        #region Fields

        [Header("References")]
        [Tooltip("Assign a pivot under the player (recommended). Fallback: Camera.main")]
        [SerializeField] private Transform m_CameraRoot;

        [Header("Movement")]
        [SerializeField] private float m_WalkSpeed = 4.5f;
        [SerializeField] private float m_SprintSpeed = 7.5f;
        [SerializeField] private float m_CrouchSpeed = 2.5f;
        [SerializeField] private float m_JumpHeight = 1.6f;
        [SerializeField] private float m_Gravity = -25f;

        [Header("Look")]
        [SerializeField] private float m_LookSensitivity = 1.6f;
        [SerializeField] private float m_MaxLookAngle = 85f;

        [Header("Crouch")]
        [SerializeField] private float m_CrouchHeight = 1.2f;
        [SerializeField] private float m_CrouchCenterY = 0.6f;

        [Header("Collision / Headroom Check")]
        [Tooltip("Which layers should block standing up (Environment, Default, etc.)")]
        [SerializeField] private LayerMask m_ObstacleMask = ~0;

        [Tooltip("Extra radius padding for capsule checks.")]
        [SerializeField] private float m_HeadroomPadding = 0.02f;

        [Header("Cursor")]
        [SerializeField] private bool m_LockCursorOnEnable = true;

        private CharacterController m_Controller;
        private InputSystem_Actions m_Actions;

        private Vector2 m_MoveInput;
        private Vector2 m_LookInput;

        private bool m_JumpPressed;
        private bool m_IsSprinting;
        private bool m_IsCrouching;

        private float m_VerticalVelocity;
        private float m_Pitch;

        private float m_OriginalHeight;
        private Vector3 m_OriginalCenter;

        private bool m_IsMouseLook;

        private const float k_GroundedStickVelocity = -2f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_Controller = GetComponent<CharacterController>();
            m_OriginalHeight = m_Controller.height;
            m_OriginalCenter = m_Controller.center;

            if (m_CameraRoot == null)
            {
                Camera cam = Camera.main;
                if (cam != null)
                {
                    m_CameraRoot = cam.transform;
                    Debug.LogWarning(
                        $"{nameof(FPSInputController)}: CameraRoot was not assigned. Falling back to Camera.main.",
                        this);
                }
                else
                {
                    Debug.LogError(
                        $"{nameof(FPSInputController)}: CameraRoot is missing and Camera.main not found.",
                        this);
                }
            }

            m_Actions = new InputSystem_Actions();
            m_Actions.Player.AddCallbacks(this);
        }

        private void OnEnable()
        {
            if (m_Actions == null)
            {
                m_Actions = new InputSystem_Actions();
                m_Actions.Player.AddCallbacks(this);
            }

            m_Actions.Player.Enable();

            if (m_LockCursorOnEnable)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnDisable()
        {
            if (m_Actions != null)
            {
                m_Actions.Player.Disable();
            }

            if (m_LockCursorOnEnable)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void OnDestroy()
        {
            if (m_Actions != null)
            {
                m_Actions.Player.RemoveCallbacks(this);
                m_Actions.Dispose();
            }
        }

        private void Update()
        {
            HandleLook();
            HandleCrouch();
            HandleMovement();
        }

        #endregion

        #region Methods

        private void HandleLook()
        {
            if (m_CameraRoot == null)
            {
                return;
            }

            float dtMultiplier = m_IsMouseLook ? 1f : Time.deltaTime;

            float yawDelta = m_LookInput.x * m_LookSensitivity * dtMultiplier;
            float pitchDelta = m_LookInput.y * m_LookSensitivity * dtMultiplier;

            m_Pitch -= pitchDelta;
            m_Pitch = Mathf.Clamp(m_Pitch, -m_MaxLookAngle, m_MaxLookAngle);

            m_CameraRoot.localRotation = Quaternion.Euler(m_Pitch, 0f, 0f);
            transform.Rotate(Vector3.up * yawDelta);
        }

        private void HandleMovement()
        {
            if (m_Controller.isGrounded && m_VerticalVelocity < 0f)
            {
                m_VerticalVelocity = k_GroundedStickVelocity;
            }

            float speed = GetCurrentSpeed();
            Vector3 planarMove = (transform.right * m_MoveInput.x + transform.forward * m_MoveInput.y) * speed;

            if (m_JumpPressed && m_Controller.isGrounded)
            {
                m_VerticalVelocity = Mathf.Sqrt(m_JumpHeight * -2f * m_Gravity);
            }

            m_JumpPressed = false;
            m_VerticalVelocity += m_Gravity * Time.deltaTime;

            Vector3 velocity = planarMove + (Vector3.up * m_VerticalVelocity);
            m_Controller.Move(velocity * Time.deltaTime);
        }

        private void HandleCrouch()
        {
            if (m_IsCrouching)
            {
                ApplyControllerDimensions(
                    m_CrouchHeight,
                    new Vector3(m_OriginalCenter.x, m_CrouchCenterY, m_OriginalCenter.z));
                return;
            }

            if (CanApplyControllerDimensions(m_OriginalHeight, m_OriginalCenter))
            {
                ApplyControllerDimensions(m_OriginalHeight, m_OriginalCenter);
            }
        }

        private float GetCurrentSpeed()
        {
            if (m_IsCrouching)
            {
                return m_CrouchSpeed;
            }

            if (m_IsSprinting)
            {
                return m_SprintSpeed;
            }

            return m_WalkSpeed;
        }

        private void ApplyControllerDimensions(float targetHeight, Vector3 targetCenter)
        {
            m_Controller.height = Mathf.MoveTowards(m_Controller.height, targetHeight, Time.deltaTime * 8f);

            Vector3 center = m_Controller.center;
            center.y = Mathf.MoveTowards(center.y, targetCenter.y, Time.deltaTime * 8f);
            center.x = targetCenter.x;
            center.z = targetCenter.z;

            m_Controller.center = center;
        }

        private bool CanApplyControllerDimensions(float desiredHeight, Vector3 desiredCenter)
        {
            float radius = Mathf.Max(0.01f, m_Controller.radius - m_HeadroomPadding);
            Vector3 worldCenter = transform.TransformPoint(desiredCenter);

            float halfHeight = desiredHeight * 0.5f;
            float capsuleHalf = Mathf.Max(radius, halfHeight);

            Vector3 bottom = worldCenter + Vector3.down * (capsuleHalf - radius);
            Vector3 top = worldCenter + Vector3.up * (capsuleHalf - radius);

            bool blocked = Physics.CheckCapsule(
                bottom,
                top,
                radius,
                m_ObstacleMask,
                QueryTriggerInteraction.Ignore);

            return !blocked;
        }

        #endregion

        #region Interface Implementations

        void InputSystem_Actions.IPlayerActions.OnMove(InputAction.CallbackContext context)
        {
            m_MoveInput = context.ReadValue<Vector2>();
        }

        void InputSystem_Actions.IPlayerActions.OnLook(InputAction.CallbackContext context)
        {
            m_LookInput = context.ReadValue<Vector2>();
            InputDevice device = context.control?.device;
            m_IsMouseLook = device is Mouse;
        }

        void InputSystem_Actions.IPlayerActions.OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_IsCrouching = true;
            }
            else if (context.canceled)
            {
                m_IsCrouching = false;
            }
        }

        void InputSystem_Actions.IPlayerActions.OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_JumpPressed = true;
            }
        }

        void InputSystem_Actions.IPlayerActions.OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_IsSprinting = true;
            }
            else if (context.canceled)
            {
                m_IsSprinting = false;
            }
        }

        void InputSystem_Actions.IPlayerActions.OnAttack(InputAction.CallbackContext context) { }
        void InputSystem_Actions.IPlayerActions.OnInteract(InputAction.CallbackContext context) { }
        void InputSystem_Actions.IPlayerActions.OnPrevious(InputAction.CallbackContext context) { }
        void InputSystem_Actions.IPlayerActions.OnNext(InputAction.CallbackContext context) { }

        #endregion
    }
}
