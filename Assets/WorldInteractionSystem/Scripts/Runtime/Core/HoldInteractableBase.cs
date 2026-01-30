using UnityEngine;

namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Base class for hold interactions.
    /// </summary>
    public abstract class HoldInteractableBase : InteractableBase, IHoldInteractable
    {
        #region Fields
        private const float k_MinHoldDuration = 0.1f;

        [Header("Hold")]
        [SerializeField] private float m_HoldDuration = 2f;

        #endregion

        #region Properties

        /// <summary>
        /// Duration required to complete the hold interaction.
        /// </summary>
        public float HoldDuration => m_HoldDuration;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (m_HoldDuration < k_MinHoldDuration)
            {
                Debug.LogWarning(
                    $"{name}: HoldDuration was below {k_MinHoldDuration}. Clamping value.",
                    this);
                m_HoldDuration = k_MinHoldDuration;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when hold begins.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        protected virtual void OnHoldStarted(InteractorContext context) { }

        /// <summary>
        /// Called while holding.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        /// <param name="normalizedProgress">Progress from 0 to 1.</param>
        protected virtual void OnHoldProgress(InteractorContext context, float normalizedProgress) { }

        /// <summary>
        /// Called when hold completes.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        protected abstract void OnHoldCompleted(InteractorContext context);

        /// <summary>
        /// Called when hold is canceled.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        protected virtual void OnHoldCanceled(InteractorContext context) { }

        #endregion

        #region Interface Implementations

        float IHoldInteractable.HoldDuration => HoldDuration;

        void IHoldInteractable.BeginHold(InteractorContext context)
        {
            if (!CanInteractInternal(context, out string reason))
            {
                Debug.LogWarning($"{name}: Cannot start hold. {reason}", this);
                return;
            }

            OnHoldStarted(context);
        }

        void IHoldInteractable.UpdateHold(InteractorContext context, float normalizedProgress)
        {
            OnHoldProgress(context, normalizedProgress);
        }

        void IHoldInteractable.CompleteHold(InteractorContext context)
        {
            OnHoldCompleted(context);
        }

        void IHoldInteractable.CancelHold(InteractorContext context)
        {
            OnHoldCanceled(context);
        }

        #endregion
    }
}
