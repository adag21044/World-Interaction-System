namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Defines the supported interaction types.
    /// </summary>
    public enum InteractionType
    {
        /// <summary>
        /// Single press interaction.
        /// </summary>
        Instant,

        /// <summary>
        /// Hold-to-complete interaction.
        /// </summary>
        Hold,

        /// <summary>
        /// Toggle interaction with on/off state.
        /// </summary>
        Toggle
    }
}
