using UnityEngine;

namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Base class for instant interactions.
    /// </summary>
    public abstract class InstantInteractableBase : InteractableBase, IInstantInteractable
    {
        #region Methods

        /// <summary>
        /// Runs the instant interaction behavior.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        protected abstract void OnInstantInteract(InteractorContext context);

        #endregion

        #region Interface Implementations

        void IInstantInteractable.Interact(InteractorContext context)
        {
            if (!CanInteractInternal(context, out string reason))
            {
                Debug.LogWarning($"{name}: Cannot interact. {reason}", this);
                return;
            }

            OnInstantInteract(context);
        }

        #endregion
    }
}
