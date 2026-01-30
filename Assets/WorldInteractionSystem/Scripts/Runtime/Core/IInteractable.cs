namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Base interaction contract for all interactable objects.
    /// </summary>
    public interface IInteractable
    {
        #region Properties

        /// <summary>
        /// Interaction type used by this interactable.
        /// </summary>
        InteractionType InteractionType { get; }

        /// <summary>
        /// Display name of the interactable object.
        /// </summary>
        string DisplayName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the interactor can interact with this object.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        /// <param name="reason">Failure reason, if any.</param>
        /// <returns>True if interaction is allowed; otherwise false.</returns>
        bool CanInteract(InteractorContext context, out string reason);

        /// <summary>
        /// Returns the action verb used for UI prompts (e.g., "Open").
        /// </summary>
        /// <param name="context">Interaction context.</param>
        /// <returns>Prompt action text.</returns>
        string GetInteractionPrompt(InteractorContext context);

        /// <summary>
        /// Called when the interactor starts focusing this object.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        void OnFocusGained(InteractorContext context);

        /// <summary>
        /// Called when the interactor stops focusing this object.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        void OnFocusLost(InteractorContext context);

        #endregion
    }
}
