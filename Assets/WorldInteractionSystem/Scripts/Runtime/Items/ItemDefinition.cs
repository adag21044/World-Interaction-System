using UnityEngine;

namespace WorldInteractionSystem.Runtime.Items
{
    /// <summary>
    /// Scriptable definition for an inventory item.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Item", menuName = "WorldInteractionSystem/Items/Item")]
    public class ItemDefinition : ScriptableObject
    {
        #region Fields

        [Header("Item")]
        [SerializeField] private string m_ItemId = "item_id";
        [SerializeField] private string m_DisplayName = "Item";
        [TextArea(2, 4)]
        [SerializeField] private string m_Description;

        #endregion

        #region Properties

        /// <summary>
        /// Unique item identifier.
        /// </summary>
        public string ItemId => m_ItemId;

        /// <summary>
        /// Display name for UI.
        /// </summary>
        public string DisplayName => m_DisplayName;

        /// <summary>
        /// Item description.
        /// </summary>
        public string Description => m_Description;

        #endregion

        #region Unity Methods

        protected virtual void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(m_ItemId))
            {
                Debug.LogWarning($"{name}: ItemId is empty.", this);
            }
        }

        #endregion
    }
}
