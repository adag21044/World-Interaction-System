using System.Collections;
using UnityEngine;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.Items;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Toggleable door with optional key lock.
    /// </summary>
    public class DoorInteractable : ToggleInteractableBase
    {
        #region Fields

        private const float k_DefaultOpenAngle = 90f;
        private const float k_DefaultOpenSpeed = 4f;
        private const string k_KeyRequiredText = "Key required";
        private const string k_OpenText = "Open";
        private const string k_CloseText = "Close";

        [Header("Door")]
        [SerializeField] private Transform m_Pivot;
        [SerializeField] private float m_OpenAngle = k_DefaultOpenAngle;
        [SerializeField] private float m_OpenSpeed = k_DefaultOpenSpeed;

        [Header("Lock")]
        [SerializeField] private bool m_IsLocked;
        [SerializeField] private KeyItemDefinition m_RequiredKey;

        private Quaternion m_ClosedRotation;
        private Quaternion m_OpenRotation;
        private Coroutine m_RotationRoutine;

        #endregion

        #region Properties

        public override InteractionType InteractionType => InteractionType.Toggle;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_Pivot == null)
            {
                m_Pivot = transform;
                Debug.LogWarning($"{nameof(DoorInteractable)}: Pivot not assigned. Using self.", this);
            }

            m_ClosedRotation = m_Pivot.localRotation;
            m_OpenRotation = m_ClosedRotation * Quaternion.Euler(0f, m_OpenAngle, 0f);

            if (m_IsLocked && m_RequiredKey == null)
            {
                Debug.LogWarning($"{nameof(DoorInteractable)}: Door is locked but no key assigned.", this);
            }
        }

        #endregion

        #region Methods

        public void SetLocked(bool isLocked)
        {
            m_IsLocked = isLocked;
        }

        protected override string GetInteractionVerb(InteractorContext context)
        {
            return IsOn ? k_CloseText : k_OpenText;
        }

        protected override bool CanInteractInternal(InteractorContext context, out string reason)
        {
            if (!base.CanInteractInternal(context, out reason))
            {
                return false;
            }

            if (!m_IsLocked)
            {
                reason = string.Empty;
                return true;
            }

            if (context.Inventory == null)
            {
                Debug.LogError($"{nameof(DoorInteractable)}: Inventory is missing.", this);
                reason = k_KeyRequiredText;
                return false;
            }

            if (m_RequiredKey == null)
            {
                Debug.LogError($"{nameof(DoorInteractable)}: Required key is not assigned.", this);
                reason = k_KeyRequiredText;
                return false;
            }

            if (!context.Inventory.ContainsKey(m_RequiredKey))
            {
                reason = k_KeyRequiredText;
                return false;
            }

            // Key varsa: unlock
            m_IsLocked = false;
            reason = string.Empty;
            return true;
        }

        protected override void OnToggled(InteractorContext context, bool isOn)
        {
            RotateDoor(isOn);
        }

        private void RotateDoor(bool open)
        {
            if (m_Pivot == null)
            {
                Debug.LogError($"{nameof(DoorInteractable)}: Pivot is missing.", this);
                return;
            }

            if (m_RotationRoutine != null)
            {
                StopCoroutine(m_RotationRoutine);
            }

            Quaternion target = open ? m_OpenRotation : m_ClosedRotation;
            m_RotationRoutine = StartCoroutine(RotateRoutine(target));
        }

        private IEnumerator RotateRoutine(Quaternion targetRotation)
        {
            Quaternion startRotation = m_Pivot.localRotation;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * m_OpenSpeed;
                m_Pivot.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            m_Pivot.localRotation = targetRotation;
        }

        #endregion
    }
}
