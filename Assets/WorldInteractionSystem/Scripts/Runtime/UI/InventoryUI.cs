using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using WorldInteractionSystem.Runtime.Items;
using PlayerInventory = WorldInteractionSystem.Runtime.Player.Inventory;

namespace WorldInteractionSystem.Runtime.UI
{
    /// <summary>
    /// Displays a simple inventory list.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private PlayerInventory m_Inventory;
        [SerializeField] private TMP_Text m_ItemsText;
        [SerializeField] private TMP_Text[] m_SlotTexts;

        [Header("Display")]
        [SerializeField] private string m_Header = "Inventory";
        [SerializeField] private string m_EmptyText = "Inventory: Empty";
        [SerializeField] private string m_EmptySlotText = "No item";

        private bool m_HasLoggedMissingRefs;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (m_Inventory == null)
            {
                m_Inventory = GetComponentInParent<PlayerInventory>();
                if (m_Inventory == null)
                {
                    LogMissingRefs();
                    return;
                }
            }

            if (!HasOutputTarget())
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
            if (!HasOutputTarget())
            {
                LogMissingRefs();
                return;
            }

            if (HasSlotOutput())
            {
                RefreshSlots(items);
                return;
            }

            RefreshList(items);
        }

        private void RefreshList(IReadOnlyList<ItemDefinition> items)
        {
            if (m_ItemsText == null)
            {
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

                builder.AppendLine($"- {GetDisplayName(item)}");
            }

            m_ItemsText.text = builder.ToString();
        }

        private void RefreshSlots(IReadOnlyList<ItemDefinition> items)
        {
            if (m_SlotTexts == null || m_SlotTexts.Length == 0)
            {
                return;
            }

            int itemIndex = 0;

            for (int i = 0; i < m_SlotTexts.Length; i++)
            {
                TMP_Text slotText = m_SlotTexts[i];
                if (slotText == null)
                {
                    continue;
                }

                ItemDefinition item = null;

                if (items != null)
                {
                    while (itemIndex < items.Count && items[itemIndex] == null)
                    {
                        itemIndex++;
                    }

                    if (itemIndex < items.Count)
                    {
                        item = items[itemIndex];
                        itemIndex++;
                    }
                }

                slotText.text = item == null ? m_EmptySlotText : GetDisplayName(item);
            }
        }

        private string GetDisplayName(ItemDefinition item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return string.IsNullOrWhiteSpace(item.DisplayName) ? item.name : item.DisplayName;
        }

        private bool HasOutputTarget()
        {
            return HasSlotOutput() || m_ItemsText != null;
        }

        private bool HasSlotOutput()
        {
            return m_SlotTexts != null && m_SlotTexts.Length > 0;
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
