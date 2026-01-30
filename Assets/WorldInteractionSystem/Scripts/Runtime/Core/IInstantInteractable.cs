namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Interaction contract for instant interactions.
    /// </summary>
    public interface IInstantInteractable : IInteractable
    {
        #region Methods

        /// <summary>
        /// Executes the interaction instantly.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        void Interact(InteractorContext context);

        #endregion
    }
}
