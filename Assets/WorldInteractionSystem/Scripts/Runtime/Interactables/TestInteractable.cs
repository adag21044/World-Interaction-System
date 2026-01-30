using UnityEngine;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Simple test interactable for debug usage.
    /// </summary>
    public class TestInteractable : InstantInteractableBase
    {
        #region Fields

        [Header("Debug")]
        [SerializeField] private string m_Message = "Test interaction";

        #endregion

        #region Properties

        /// <summary>
        /// Interaction type for testing.
        /// </summary>
        public override InteractionType InteractionType => InteractionType.Instant;

        #endregion

        #region Methods

        protected override void OnInstantInteract(InteractorContext context)
        {
            Debug.Log($"{nameof(TestInteractable)}: {m_Message}", this);
        }

        #endregion
    }
}
