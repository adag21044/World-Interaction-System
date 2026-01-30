using System.Collections;
using UnityEngine;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.Items;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Hold interaction chest that opens once and can grant an item.
    /// </summary>
    public class ChestInteractable : HoldInteractableBase
    {
        #region Fields

        private const float k_DefaultOpenAngle = -90f;
        private const float k_DefaultOpenSpeed = 4f;
        private const string k_AlreadyOpenedText = "Already opened";

        [Header("Chest")]
        [SerializeField] private Transform m_LidPivot;
        [SerializeField] private Vector3 m_OpenAxis = Vector3.right;
        [SerializeField] private float m_OpenAngle = k_DefaultOpenAngle;
        [SerializeField] private float m_OpenSpeed = k_DefaultOpenSpeed;
        [SerializeField] private bool m_IsOpened;

        [Header("Contents")]
        [SerializeField] private ItemDefinition m_ContainedItem;
        [SerializeField] private bool m_AddItemToInventory = true;

        [Header("Debug")]
        [SerializeField] private bool m_LogHoldProgress = true;

        private Quaternion m_ClosedRotation;
        private Quaternion m_OpenRotation;
        private Coroutine m_RotationRoutine;
        private int m_LastLoggedPercent = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Interaction type for chest.
        /// </summary>
        public override InteractionType InteractionType => InteractionType.Hold;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_LidPivot == null)
            {
                m_LidPivot = transform;
                Debug.LogWarning($"{nameof(ChestInteractable)}: Lid pivot not assigned. Using self.", this);
            }

            m_ClosedRotation = m_LidPivot.localRotation;
            m_OpenRotation = m_ClosedRotation * Quaternion.AngleAxis(m_OpenAngle, m_OpenAxis.normalized);
        }

        #endregion

        #region Methods

        protected override string GetInteractionVerb(InteractorContext context)
        {
            return "Open";
        }

        protected override bool CanInteractInternal(InteractorContext context, out string reason)
        {
            if (!base.CanInteractInternal(context, out reason))
            {
                return false;
            }

            if (m_IsOpened)
            {
                reason = k_AlreadyOpenedText;
                return false;
            }

            reason = string.Empty;
            return true;
        }

        protected override void OnHoldCompleted(InteractorContext context)
        {
            if (m_IsOpened)
            {
                return;
            }

            if (m_LogHoldProgress)
            {
                Debug.Log($"{name}: Hold completed (100%).", this);
            }

            m_LastLoggedPercent = -1;
            m_IsOpened = true;
            RotateLid();

            if (m_ContainedItem == null || !m_AddItemToInventory)
            {
                return;
            }

            if (context.Inventory == null)
            {
                Debug.LogError($"{nameof(ChestInteractable)}: Inventory is missing.", this);
                return;
            }

            context.Inventory.TryAddItem(m_ContainedItem);
        }

        protected override void OnHoldStarted(InteractorContext context)
        {
            m_LastLoggedPercent = -1;

            if (m_LogHoldProgress)
            {
                Debug.Log($"{name}: Hold started (0%).", this);
            }
        }

        protected override void OnHoldProgress(InteractorContext context, float normalizedProgress)
        {
            if (!m_LogHoldProgress)
            {
                return;
            }

            int percent = Mathf.Clamp(Mathf.RoundToInt(normalizedProgress * 100f), 0, 100);
            if (percent == m_LastLoggedPercent)
            {
                return;
            }

            m_LastLoggedPercent = percent;
            Debug.Log($"{name}: Hold progress {percent}%.", this);
        }

        protected override void OnHoldCanceled(InteractorContext context)
        {
            if (m_LogHoldProgress)
            {
                string progressText = m_LastLoggedPercent >= 0 ? $"{m_LastLoggedPercent}%" : "unknown%";
                Debug.Log($"{name}: Hold canceled at {progressText}.", this);
            }

            m_LastLoggedPercent = -1;
        }

        private void RotateLid()
        {
            if (m_LidPivot == null)
            {
                Debug.LogError($"{nameof(ChestInteractable)}: Lid pivot is missing.", this);
                return;
            }

            if (m_RotationRoutine != null)
            {
                StopCoroutine(m_RotationRoutine);
            }

            m_RotationRoutine = StartCoroutine(RotateRoutine(m_OpenRotation));
        }

        private IEnumerator RotateRoutine(Quaternion targetRotation)
        {
            Quaternion startRotation = m_LidPivot.localRotation;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * m_OpenSpeed;
                m_LidPivot.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            m_LidPivot.localRotation = targetRotation;
        }

        #endregion
    }
}
