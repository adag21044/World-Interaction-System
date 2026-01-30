using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.UI;

namespace WorldInteractionSystem.Runtime.Player
{
    /// <summary>
    /// Handles player interaction input, targeting, and UI feedback.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        #region Fields

        private const string k_DefaultCannotInteractText = "Cannot interact";
        private const string k_DefaultOutOfRangeText = "Out of range";

        [Header("References")]
        [SerializeField] private InteractionDetector m_Detector;
        [SerializeField] private Inventory m_Inventory;
        [SerializeField] private InteractionPromptUI m_InteractionUI;

        [Header("Input")]
        [SerializeField] private InputActionReference m_InteractAction;
        [SerializeField] private string m_DefaultInteractDisplay = "E";
        [SerializeField] private string m_PressPromptFormat = "Press {0} to {1}";
        [SerializeField] private string m_HoldPromptFormat = "Hold {0} to {1}";
        [SerializeField] private bool m_UseCustomInteractKey;
        [SerializeField] private Key m_CustomInteractKey = Key.E;

        [Header("Interaction Lock")]
        [SerializeField] private bool m_LockToSingleInteractable = true;

        [Header("Debug")]
        [SerializeField] private bool m_EnableDebugLogs;

        private InputSystem_Actions m_FallbackActions;
        private InputAction m_ResolvedInteractAction;
        private InputAction m_CustomInteractAction;
        private InputAction m_BoundInteractAction;
        private Key m_LastCustomInteractKey = Key.None;
        private bool m_LastUseCustomInteractKey;
        private string m_InteractDisplayString;

        private IInteractable m_CurrentInteractable;
        private bool m_IsOutOfRange;
        private IInteractable m_LockedInteractable;

        private IHoldInteractable m_CurrentHold;
        private float m_HoldElapsed;
        private bool m_IsHolding;
        private CancellationTokenSource m_HoldCancellation;

        private bool m_HasLoggedMissingInventory;
        private bool m_HasLoggedMissingUI;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_Detector == null)
            {
                m_Detector = GetComponent<InteractionDetector>();
                if (m_Detector == null)
                {
                    Debug.LogError($"{nameof(PlayerInteractor)}: InteractionDetector is missing.", this);
                }
            }

            if (m_Inventory == null)
            {
                m_Inventory = GetComponent<Inventory>();
                if (m_Inventory == null && !m_HasLoggedMissingInventory)
                {
                    Debug.LogWarning($"{nameof(PlayerInteractor)}: Inventory is not assigned.", this);
                    m_HasLoggedMissingInventory = true;
                }
            }

            if (m_InteractionUI == null && !m_HasLoggedMissingUI)
            {
                Debug.LogWarning($"{nameof(PlayerInteractor)}: Interaction UI is not assigned.", this);
                m_HasLoggedMissingUI = true;
            }
        }

        private void OnEnable()
        {
            BindInteractAction();
        }

        private void OnDisable()
        {
            CancelHold();
            UnbindInteractAction();
        }

        private void Update()
        {
            if (HasInputSettingsChanged())
            {
                BindInteractAction();
            }

            UpdateTarget();
            TryStartHoldFromInput();
            if (m_IsHolding && !IsInteractInputPressed())
            {
                CancelHold();
            }
        }

        private void OnDestroy()
        {
            CancelHold();

            if (m_FallbackActions != null)
            {
                m_FallbackActions.Dispose();
                m_FallbackActions = null;
            }

            if (m_CustomInteractAction != null)
            {
                m_CustomInteractAction.Disable();
                m_CustomInteractAction.Dispose();
                m_CustomInteractAction = null;
            }
        }

        #endregion

        #region Methods

        private void ResolveInteractAction()
        {
            if (m_UseCustomInteractKey)
            {
                if (m_CustomInteractKey == Key.None)
                {
                    Debug.LogWarning(
                        $"{nameof(PlayerInteractor)}: Custom interact key is None. Falling back to default action.",
                        this);
                }
                else
                {
                    EnsureCustomInteractAction();
                    m_ResolvedInteractAction = m_CustomInteractAction;
                    m_InteractDisplayString = GetInteractDisplayString();
                    return;
                }
            }

            if (m_InteractAction != null && m_InteractAction.action != null)
            {
                m_ResolvedInteractAction = m_InteractAction.action;
            }
            else
            {
                if (m_FallbackActions == null)
                {
                    m_FallbackActions = new InputSystem_Actions();
                }

                m_ResolvedInteractAction = m_FallbackActions.Player.Interact;
            }

            m_InteractDisplayString = GetInteractDisplayString();
        }

        private string GetInteractDisplayString()
        {
            if (m_ResolvedInteractAction == null)
            {
                return m_DefaultInteractDisplay;
            }

            string display = m_ResolvedInteractAction.GetBindingDisplayString();
            return string.IsNullOrWhiteSpace(display) ? m_DefaultInteractDisplay : display;
        }

        private void BindInteractAction()
        {
            UnbindInteractAction();

            ResolveInteractAction();
            if (m_ResolvedInteractAction == null)
            {
                Debug.LogError($"{nameof(PlayerInteractor)}: Interact action is missing.", this);
                return;
            }

            m_BoundInteractAction = m_ResolvedInteractAction;
            m_BoundInteractAction.started += OnInteractStarted;
            m_BoundInteractAction.performed += OnInteractPerformed;
            m_BoundInteractAction.canceled += OnInteractCanceled;
            m_BoundInteractAction.Enable();

            m_LastUseCustomInteractKey = m_UseCustomInteractKey;
        }

        private void UnbindInteractAction()
        {
            if (m_BoundInteractAction == null)
            {
                return;
            }

            m_BoundInteractAction.started -= OnInteractStarted;
            m_BoundInteractAction.performed -= OnInteractPerformed;
            m_BoundInteractAction.canceled -= OnInteractCanceled;
            m_BoundInteractAction.Disable();
            m_BoundInteractAction = null;
        }

        private bool HasInputSettingsChanged()
        {
            if (m_UseCustomInteractKey != m_LastUseCustomInteractKey)
            {
                return true;
            }

            if (m_UseCustomInteractKey && m_CustomInteractKey != m_LastCustomInteractKey)
            {
                return true;
            }

            return false;
        }

        private void EnsureCustomInteractAction()
        {
            if (m_CustomInteractAction != null && m_LastCustomInteractKey == m_CustomInteractKey)
            {
                return;
            }

            if (m_CustomInteractAction != null)
            {
                m_CustomInteractAction.Dispose();
            }

            string bindingPath = GetCustomKeyBindingPath();
            m_CustomInteractAction = new InputAction("InteractCustom", InputActionType.Button, bindingPath);
            m_LastCustomInteractKey = m_CustomInteractKey;
        }

        private string GetCustomKeyBindingPath()
        {
            if (Keyboard.current != null)
            {
                KeyControl keyControl = Keyboard.current[m_CustomInteractKey];
                if (keyControl != null)
                {
                    return keyControl.path;
                }
            }

            return $"<Keyboard>/{m_CustomInteractKey.ToString().ToLowerInvariant()}";
        }

        private void UpdateTarget()
        {
            if (m_Detector == null)
            {
                return;
            }

            if (m_LockToSingleInteractable && m_LockedInteractable != null)
            {
                if (!IsInteractableValid(m_LockedInteractable))
                {
                    ClearInteractionLock();
                }
                else
                {
                    bool isPressed = IsInteractInputPressed();
                    bool lockedOutOfRange = IsOutOfRange(m_LockedInteractable);
                    SetCurrentInteractable(m_LockedInteractable, lockedOutOfRange);
                    UpdateUI(m_LockedInteractable, lockedOutOfRange);

                    if (lockedOutOfRange)
                    {
                        CancelHold();
                        if (!m_IsHolding)
                        {
                            ClearInteractionLock();
                        }
                    }
                    else if (!m_IsHolding && !isPressed)
                    {
                        ClearInteractionLock();
                    }

                    if (m_LockedInteractable != null)
                    {
                        return;
                    }
                }
            }

            IInteractable target = null;
            bool outOfRange = false;

            if (m_Detector.TryGetInteractableInRange(out InteractableBase inRange, out _))
            {
                target = inRange;
            }
            else if (m_Detector.TryGetFocusedInteractable(out InteractableBase focused, out _))
            {
                target = focused;
                outOfRange = true;
            }

            SetCurrentInteractable(target, outOfRange);
            UpdateUI(target, outOfRange);
        }

        private void SetCurrentInteractable(IInteractable newInteractable, bool outOfRange)
        {
            if (ReferenceEquals(m_CurrentInteractable, newInteractable) && m_IsOutOfRange == outOfRange)
            {
                return;
            }

            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnFocusLost(BuildContext());
            }

            CancelHold();

            m_CurrentInteractable = newInteractable;
            m_IsOutOfRange = outOfRange;

            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnFocusGained(BuildContext());
            }
        }

        private void UpdateUI(IInteractable interactable, bool outOfRange)
        {
            if (m_InteractionUI == null)
            {
                return;
            }

            if (interactable == null)
            {
                m_InteractionUI.Clear();
                return;
            }

            InteractorContext context = BuildContext();
            string actionVerb = interactable.GetInteractionPrompt(context);

            if (string.IsNullOrWhiteSpace(actionVerb))
            {
                actionVerb = "Interact";
            }

            string format = interactable is IHoldInteractable ? m_HoldPromptFormat : m_PressPromptFormat;
            string prompt = string.Format(format, m_InteractDisplayString, actionVerb);

            m_InteractionUI.SetPrompt(prompt, true);

            if (outOfRange)
            {
                m_InteractionUI.SetStatus(k_DefaultOutOfRangeText, true);
                m_InteractionUI.SetHoldProgress(0f, false);
                return;
            }

            bool canInteract = interactable.CanInteract(context, out string reason);
            if (canInteract)
            {
                m_InteractionUI.SetStatus(string.Empty, false);
            }
            else
            {
                string status = string.IsNullOrWhiteSpace(reason) ? k_DefaultCannotInteractText : reason;
                m_InteractionUI.SetStatus(status, true);
            }
        }

        private void TryStartHoldFromInput()
        {
            if (m_IsHolding || m_IsOutOfRange)
            {
                return;
            }

            if (!(m_CurrentInteractable is IHoldInteractable holdInteractable))
            {
                return;
            }

            if (m_BoundInteractAction == null || !m_BoundInteractAction.enabled)
            {
                return;
            }

            bool pressedThisFrame = m_BoundInteractAction.WasPressedThisFrame();
            bool isPressed = m_BoundInteractAction.IsPressed();

            if (!pressedThisFrame && !isPressed)
            {
                return;
            }

            if (!m_CurrentInteractable.CanInteract(BuildContext(), out string reason))
            {
                if (m_EnableDebugLogs)
                {
                    Debug.Log($"{nameof(PlayerInteractor)}: Hold blocked. {reason}", this);
                }

                return;
            }

            StartHold(holdInteractable);
        }

        private void StartHold(IHoldInteractable holdInteractable)
        {
            if (m_LockToSingleInteractable && m_CurrentInteractable != null)
            {
                m_LockedInteractable = m_CurrentInteractable;
            }

            CancelHold();

            m_CurrentHold = holdInteractable;
            m_HoldElapsed = 0f;
            m_IsHolding = true;
            m_CurrentHold.BeginHold(BuildContext());
            m_InteractionUI?.SetHoldProgress(0f, true);

            m_HoldCancellation = new CancellationTokenSource();
            RunHoldAsync(m_CurrentHold, m_HoldCancellation.Token).Forget();
        }

        private void CompleteHold()
        {
            if (m_CurrentHold == null)
            {
                return;
            }

            if (!m_CurrentHold.CanInteract(BuildContext(), out string reason))
            {
                if (m_EnableDebugLogs)
                {
                    Debug.Log($"{nameof(PlayerInteractor)}: Hold completion blocked. {reason}", this);
                }

                CancelHold();
                m_InteractionUI?.SetStatus(
                    string.IsNullOrWhiteSpace(reason) ? k_DefaultCannotInteractText : reason,
                    true);
                return;
            }

            m_CurrentHold.CompleteHold(BuildContext());
            m_IsHolding = false;
            m_CurrentHold = null;
            m_HoldElapsed = 0f;
            m_InteractionUI?.SetHoldProgress(0f, false);

            DisposeHoldCancellation();
            ClearInteractionLock();
        }

        private void CancelHold()
        {
            if (!m_IsHolding || m_CurrentHold == null)
            {
                return;
            }

            m_CurrentHold.CancelHold(BuildContext());
            m_IsHolding = false;
            m_CurrentHold = null;
            m_HoldElapsed = 0f;
            m_InteractionUI?.SetHoldProgress(0f, false);

            DisposeHoldCancellation();
            ClearInteractionLock();
        }

        private async UniTaskVoid RunHoldAsync(IHoldInteractable holdInteractable, CancellationToken token)
        {
            float duration = holdInteractable.HoldDuration;
            if (duration <= 0f)
            {
                Debug.LogError($"{nameof(PlayerInteractor)}: Hold duration is invalid.", this);
                CancelHold();
                return;
            }

            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (token.IsCancellationRequested || !m_IsHolding || m_CurrentHold == null)
                {
                    return;
                }

                elapsed += Time.deltaTime;
                m_HoldElapsed = elapsed;

                float progress = Mathf.Clamp01(elapsed / duration);
                holdInteractable.UpdateHold(BuildContext(), progress);
                m_InteractionUI?.SetHoldProgress(progress, true);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (token.IsCancellationRequested)
            {
                return;
            }

            CompleteHold();
        }

        private void DisposeHoldCancellation()
        {
            if (m_HoldCancellation == null)
            {
                return;
            }

            if (!m_HoldCancellation.IsCancellationRequested)
            {
                m_HoldCancellation.Cancel();
            }

            m_HoldCancellation.Dispose();
            m_HoldCancellation = null;
        }

        private bool TryGetCurrentInteractable(out IInteractable interactable, out bool outOfRange)
        {
            interactable = m_CurrentInteractable;
            outOfRange = m_IsOutOfRange;
            return interactable != null;
        }

        private InteractorContext BuildContext()
        {
            Transform source = m_Detector != null && m_Detector.Source != null ? m_Detector.Source : transform;
            return new InteractorContext(gameObject, source, m_Inventory);
        }

        private bool IsInteractableValid(IInteractable interactable)
        {
            if (interactable == null)
            {
                return false;
            }

            if (interactable is Object unityObject && unityObject == null)
            {
                return false;
            }

            return true;
        }

        private bool IsOutOfRange(IInteractable interactable)
        {
            if (m_Detector == null)
            {
                return false;
            }

            if (!(interactable is Component component) || component == null)
            {
                return false;
            }

            Transform source = m_Detector.Source != null ? m_Detector.Source : transform;
            float distance = Vector3.Distance(source.position, component.transform.position);
            return distance > m_Detector.InteractionRange;
        }

        private bool IsInteractInputPressed()
        {
            return m_BoundInteractAction != null && m_BoundInteractAction.enabled && m_BoundInteractAction.IsPressed();
        }

        private void ClearInteractionLock()
        {
            m_LockedInteractable = null;
        }

        private void OnInteractStarted(InputAction.CallbackContext context)
        {
            if (!TryGetCurrentInteractable(out IInteractable interactable, out bool outOfRange))
            {
                return;
            }

            if (outOfRange)
            {
                return;
            }

            if (interactable is IHoldInteractable holdInteractable)
            {
                if (!interactable.CanInteract(BuildContext(), out string reason))
                {
                    if (m_EnableDebugLogs)
                    {
                        Debug.Log($"{nameof(PlayerInteractor)}: Hold blocked. {reason}", this);
                    }

                    return;
                }

                StartHold(holdInteractable);
            }
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (!TryGetCurrentInteractable(out IInteractable interactable, out bool outOfRange))
            {
                return;
            }

            if (outOfRange)
            {
                return;
            }

            if (!interactable.CanInteract(BuildContext(), out string reason))
            {
                if (m_EnableDebugLogs)
                {
                    Debug.Log($"{nameof(PlayerInteractor)}: Interaction blocked. {reason}", this);
                }

                return;
            }

            if (m_LockToSingleInteractable && m_CurrentInteractable != null)
            {
                m_LockedInteractable = m_CurrentInteractable;
            }

            if (interactable is IInstantInteractable instantInteractable)
            {
                instantInteractable.Interact(BuildContext());
                return;
            }

            if (interactable is IToggleInteractable toggleInteractable)
            {
                toggleInteractable.Toggle(BuildContext());
            }
        }

        private void OnInteractCanceled(InputAction.CallbackContext context)
        {
            CancelHold();
        }

        #endregion
    }
}
