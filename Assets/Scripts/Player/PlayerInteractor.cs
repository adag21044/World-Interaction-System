using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [Header("Interaction")]
    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactorRange = 3f;
    [SerializeField] private LayerMask interactableMask = ~0;

    private InputSystem_Actions m_Actions;

    private void Awake()
    {
        m_Actions = new InputSystem_Actions();
        m_Actions.Player.AddCallbacks(this);

        if (interactorSource == null)
        {
            Debug.LogWarning($"{nameof(PlayerInteractor)}: InteractorSource not assigned.", this);
        }
    }

    private void OnEnable()
    {
        m_Actions.Player.Enable();
    }

    private void OnDisable()
    {
        m_Actions.Player.Disable();
    }

    private void OnDestroy()
    {
        m_Actions.Player.RemoveCallbacks(this);
        m_Actions.Dispose();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("[Player Interactor] INTERACT CALLBACK"); 

        if (!context.performed) return;

        Debug.Log("[Player Interactor] INTERACT PRESSED");

        if (interactorSource == null) return;

        if (Physics.Raycast(interactorSource.position, interactorSource.forward, out RaycastHit hit, interactorRange, interactableMask, QueryTriggerInteraction.Ignore))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            Debug.Log("[Player Interactor] Hit: " + hit.collider.name);

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
        else
        {
            Debug.Log("[Player Interactor] Nothing hit"); 
        }
    }

    public void OnMove(InputAction.CallbackContext context) { }
    public void OnLook(InputAction.CallbackContext context) { }
    public void OnAttack(InputAction.CallbackContext context) { }
    public void OnCrouch(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnPrevious(InputAction.CallbackContext context) { }
    public void OnNext(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }
}
