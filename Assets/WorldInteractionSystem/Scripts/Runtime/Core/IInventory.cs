using System;
using System.Collections.Generic;
using WorldInteractionSystem.Runtime.Items;

namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Defines inventory operations for interaction systems.
    /// </summary>
    public interface IInventory
    {
        #region Events

        /// <summary>
        /// Raised when the inventory contents change.
        /// </summary>
        event Action<IReadOnlyList<ItemDefinition>> InventoryChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Current inventory items.
        /// </summary>
        IReadOnlyList<ItemDefinition> Items { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to add an item to the inventory.
        /// </summary>
        /// <param name="itemDefinition">Item definition to add.</param>
        /// <returns>True if added; otherwise false.</returns>
        bool TryAddItem(ItemDefinition itemDefinition);

        /// <summary>
        /// Checks if the inventory already contains an item.
        /// </summary>
        /// <param name="itemDefinition">Item definition to check.</param>
        /// <returns>True if present; otherwise false.</returns>
        bool ContainsItem(ItemDefinition itemDefinition);

        /// <summary>
        /// Checks if the inventory contains a specific key.
        /// </summary>
        /// <param name="keyDefinition">Key definition to check.</param>
        /// <returns>True if present; otherwise false.</returns>
        bool ContainsKey(KeyItemDefinition keyDefinition);

        #endregion
    }
}
