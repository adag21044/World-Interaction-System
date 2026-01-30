using UnityEngine;

namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Provides interaction context for interactable objects.
    /// </summary>
    public readonly struct InteractorContext
    {
        #region Fields

        /// <summary>
        /// The interactor GameObject (usually the player).
        /// </summary>
        public GameObject Interactor { get; }

        /// <summary>
        /// The transform that acts as the interaction origin.
        /// </summary>
        public Transform InteractorTransform { get; }

        /// <summary>
        /// Inventory provider for item checks and additions.
        /// </summary>
        public IInventory Inventory { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new interaction context.
        /// </summary>
        /// <param name="interactor">Interactor GameObject.</param>
        /// <param name="interactorTransform">Interactor transform.</param>
        /// <param name="inventory">Inventory provider.</param>
        public InteractorContext(GameObject interactor, Transform interactorTransform, IInventory inventory)
        {
            Interactor = interactor;
            InteractorTransform = interactorTransform;
            Inventory = inventory;
        }

        #endregion
    }
}
