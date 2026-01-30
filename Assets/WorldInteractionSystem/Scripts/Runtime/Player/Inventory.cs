using System;
using System.Collections.Generic;
using UnityEngine;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.Items;

namespace WorldInteractionSystem.Runtime.Player
{
    /// <summary>
    /// Simple inventory used for interaction systems.
    /// </summary>
    public class Inventory : MonoBehaviour, IInventory
    {
        #region Fields

        [Header("Inventory")]
        [SerializeField] private bool m_AllowDuplicates;

        private readonly List<ItemDefinition> m_Items = new List<ItemDefinition>();

        #endregion

        #region Events

        /// <summary>
        /// Raised when inventory contents change.
        /// </summary>
        public event Action<IReadOnlyList<ItemDefinition>> InventoryChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Current inventory items.
        /// </summary>
        public IReadOnlyList<ItemDefinition> Items => m_Items;

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to add an item to the inventory.
        /// </summary>
        /// <param name="itemDefinition">Item definition to add.</param>
        /// <returns>True if added; otherwise false.</returns>
        public bool TryAddItem(ItemDefinition itemDefinition)
        {
            if (itemDefinition == null)
            {
                Debug.LogError($"{nameof(Inventory)}: ItemDefinition is null.", this);
                return false;
            }

            if (!m_AllowDuplicates && m_Items.Contains(itemDefinition))
            {
                Debug.LogWarning($"{nameof(Inventory)}: Item already exists: {itemDefinition.name}", this);
                return false;
            }

            m_Items.Add(itemDefinition);
            InventoryChanged?.Invoke(m_Items);
            return true;
        }

        /// <summary>
        /// Checks if the inventory contains the given item definition.
        /// </summary>
        /// <param name="itemDefinition">Item definition to check.</param>
        /// <returns>True if present; otherwise false.</returns>
        public bool ContainsItem(ItemDefinition itemDefinition)
        {
            if (itemDefinition == null)
            {
                return false;
            }

            return m_Items.Contains(itemDefinition);
        }

        /// <summary>
        /// Checks if the inventory contains the given key definition.
        /// </summary>
        /// <param name="keyDefinition">Key definition to check.</param>
        /// <returns>True if present; otherwise false.</returns>
        public bool ContainsKey(KeyItemDefinition keyDefinition)
        {
            if (keyDefinition == null)
            {
                return false;
            }

            return m_Items.Contains(keyDefinition);
        }

        #endregion

        #region Interface Implementations

        event Action<IReadOnlyList<ItemDefinition>> IInventory.InventoryChanged
        {
            add => InventoryChanged += value;
            remove => InventoryChanged -= value;
        }

        IReadOnlyList<ItemDefinition> IInventory.Items => Items;

        bool IInventory.TryAddItem(ItemDefinition itemDefinition)
        {
            return TryAddItem(itemDefinition);
        }

        bool IInventory.ContainsItem(ItemDefinition itemDefinition)
        {
            return ContainsItem(itemDefinition);
        }

        bool IInventory.ContainsKey(KeyItemDefinition keyDefinition)
        {
            return ContainsKey(keyDefinition);
        }

        #endregion
    }
}
