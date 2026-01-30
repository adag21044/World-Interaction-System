namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Interaction contract for toggle interactions.
    /// </summary>
    public interface IToggleInteractable : IInteractable
    {
        #region Properties

        /// <summary>
        /// Current toggle state.
        /// </summary>
        bool IsOn { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Toggles the interaction state.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        void Toggle(InteractorContext context);

        #endregion
    }
}
