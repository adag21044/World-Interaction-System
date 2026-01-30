using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using WorldInteractionSystem.Runtime.Items;
using WorldInteractionSystem.Runtime.Player;

namespace WorldInteractionSystem.Runtime.UI
{
    /// <summary>
    /// Displays a simple inventory list.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private Inventory m_Inventory;
        [SerializeField] private TMP_Text m_ItemsText;

        [Header("Display")]
        [SerializeField] private string m_Header = "Inventory";
        [SerializeField] private string m_EmptyText = "Inventory: Empty";

        private bool m_HasLoggedMissingRefs;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (m_Inventory == null)
            {
                m_Inventory = GetComponentInParent<Inventory>();
                if (m_Inventory == null)
                {
                    LogMissingRefs();
                    return;
                }
            }

            if (m_ItemsText == null)
            {
                LogMissingRefs();
                return;
            }

            m_Inventory.InventoryChanged += OnInventoryChanged;
            Refresh(m_Inventory.Items);
        }

        private void OnDisable()
        {
            if (m_Inventory != null)
            {
                m_Inventory.InventoryChanged -= OnInventoryChanged;
            }
        }

        #endregion

        #region Methods

        private void OnInventoryChanged(IReadOnlyList<ItemDefinition> items)
        {
            Refresh(items);
        }

        private void Refresh(IReadOnlyList<ItemDefinition> items)
        {
            if (m_ItemsText == null)
            {
                LogMissingRefs();
                return;
            }

            if (items == null || items.Count == 0)
            {
                m_ItemsText.text = m_EmptyText;
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine(m_Header);

            for (int i = 0; i < items.Count; i++)
            {
                ItemDefinition item = items[i];
                if (item == null)
                {
                    continue;
                }

                builder.AppendLine($"- {item.DisplayName}");
            }

            m_ItemsText.text = builder.ToString();
        }

        private void LogMissingRefs()
        {
            if (m_HasLoggedMissingRefs)
            {
                return;
            }

            Debug.LogError($"{nameof(InventoryUI)}: Missing inventory or text reference.", this);
            m_HasLoggedMissingRefs = true;
        }

        #endregion
    }
}
