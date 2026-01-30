using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.Items;
using System;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Hold interaction chest that opens once and can grant an item.
    /// </summary>
    public class ChestInteractable : HoldInteractableBase
    {
        #region Fields

        private const float k_DefaultOpenSpeed = 4f;
        private const string k_AlreadyOpenedText = "Already opened";

        [Header("Chest")]
        [SerializeField] private Transform m_LidPivot;
        [SerializeField] private Vector3 m_OpenOffset = new Vector3(0.5f, 0f, 0f);
        [SerializeField] private float m_OpenSpeed = k_DefaultOpenSpeed;
        [SerializeField] private bool m_IsOpened;

        [Header("Contents")]
        [SerializeField] private ItemDefinition m_ContainedItem;
        [SerializeField] private bool m_AddItemToInventory = true;
        [SerializeField] private GameObject m_ContainedInteractable;
        [SerializeField] private bool m_DisableContainedInteractableOnStart = true;

        [Header("Debug")]
        [SerializeField] private bool m_LogHoldProgress = true;

        private Vector3 m_ClosedLocalPosition;
        private Vector3 m_OpenLocalPosition;
        private CancellationTokenSource m_MoveCts;
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

            m_ClosedLocalPosition = m_LidPivot.localPosition;
            m_OpenLocalPosition = m_ClosedLocalPosition + m_OpenOffset;

            if (m_IsOpened)
            {
                m_LidPivot.localPosition = m_OpenLocalPosition;
            }

            if (m_ContainedInteractable != null)
            {
                if (m_IsOpened)
                {
                    m_ContainedInteractable.SetActive(true);
                }
                else if (m_DisableContainedInteractableOnStart)
                {
                    m_ContainedInteractable.SetActive(false);
                }
            }
        }

        private void OnDisable()
        {
            CancelMove();
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
            MoveLid();
            EnableContainedInteractable();

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

        private void MoveLid()
        {
            if (m_LidPivot == null)
            {
                Debug.LogError($"{nameof(ChestInteractable)}: Lid pivot is missing.", this);
                return;
            }

            CancelMove();
            m_MoveCts = new CancellationTokenSource();
            MoveRoutineAsync(m_OpenLocalPosition, m_MoveCts.Token).Forget();
        }

        private async UniTaskVoid MoveRoutineAsync(Vector3 targetPosition, CancellationToken token)
        {
            Vector3 startPosition = m_LidPivot.localPosition;
            float t = 0f;

            try
            {
                while (t < 1f)
                {
                    t += Time.deltaTime * m_OpenSpeed;
                    m_LidPivot.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                m_LidPivot.localPosition = targetPosition;
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void EnableContainedInteractable()
        {
            if (m_ContainedInteractable == null)
            {
                return;
            }

            m_ContainedInteractable.SetActive(true);
        }

        private void CancelMove()
        {
            if (m_MoveCts == null)
            {
                return;
            }

            if (!m_MoveCts.IsCancellationRequested)
            {
                m_MoveCts.Cancel();
            }

            m_MoveCts.Dispose();
            m_MoveCts = null;
        }

        #endregion
    }
}
