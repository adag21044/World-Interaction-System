using UnityEngine;

namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Base class for all interactable objects.
    /// </summary>
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        #region Fields

        [Header("Interaction")]
        [SerializeField] private string m_DisplayName;
        [SerializeField] private string m_ActionVerb = "Interact";
        [SerializeField] private bool m_IsEnabled = true;

        #endregion

        #region Properties

        /// <summary>
        /// Interaction type used by this interactable.
        /// </summary>
        public abstract InteractionType InteractionType { get; }

        /// <summary>
        /// Display name for UI.
        /// </summary>
        public string DisplayName => string.IsNullOrWhiteSpace(m_DisplayName) ? gameObject.name : m_DisplayName;

        /// <summary>
        /// Whether this interactable is enabled.
        /// </summary>
        protected bool IsEnabled => m_IsEnabled;

        #endregion

        #region Methods

        /// <summary>
        /// Returns the action verb used in prompts.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        /// <returns>Action verb.</returns>
        protected virtual string GetInteractionVerb(InteractorContext context)
        {
            return string.IsNullOrWhiteSpace(m_ActionVerb) ? "Interact" : m_ActionVerb;
        }

        /// <summary>
        /// Determines if the interaction is allowed.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        /// <param name="reason">Failure reason.</param>
        /// <returns>True if allowed.</returns>
        protected virtual bool CanInteractInternal(InteractorContext context, out string reason)
        {
            if (!m_IsEnabled)
            {
                reason = "Disabled";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        /// <summary>
        /// Called when focus starts.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        protected virtual void OnFocusGainedInternal(InteractorContext context) { }

        /// <summary>
        /// Called when focus ends.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        protected virtual void OnFocusLostInternal(InteractorContext context) { }

        #endregion

        #region Interface Implementations

        InteractionType IInteractable.InteractionType => InteractionType;

        string IInteractable.DisplayName => DisplayName;

        bool IInteractable.CanInteract(InteractorContext context, out string reason)
        {
            return CanInteractInternal(context, out reason);
        }

        string IInteractable.GetInteractionPrompt(InteractorContext context)
        {
            return GetInteractionVerb(context);
        }

        void IInteractable.OnFocusGained(InteractorContext context)
        {
            OnFocusGainedInternal(context);
        }

        void IInteractable.OnFocusLost(InteractorContext context)
        {
            OnFocusLostInternal(context);
        }

        #endregion
    }
}
