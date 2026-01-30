namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Interaction contract for hold interactions.
    /// </summary>
    public interface IHoldInteractable : IInteractable
    {
        #region Properties

        /// <summary>
        /// Duration required to complete the hold interaction.
        /// </summary>
        float HoldDuration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Called when hold interaction starts.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        void BeginHold(InteractorContext context);

        /// <summary>
        /// Called while hold interaction progresses.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        /// <param name="normalizedProgress">Progress from 0 to 1.</param>
        void UpdateHold(InteractorContext context, float normalizedProgress);

        /// <summary>
        /// Called when hold interaction completes.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        void CompleteHold(InteractorContext context);

        /// <summary>
        /// Called when hold interaction is canceled.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        void CancelHold(InteractorContext context);

        #endregion
    }
}
