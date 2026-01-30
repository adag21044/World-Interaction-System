using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using WorldInteractionSystem.Runtime.Items;
using PlayerInventory = WorldInteractionSystem.Runtime.Player.Inventory;

namespace WorldInteractionSystem.Runtime.Inventory
{
    /// <summary>
    /// Very simple inventory display toggled by a key.
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private PlayerInventory m_Inventory;
        [SerializeField] private TMP_Text m_OutputText;
        [SerializeField] private GameObject m_OutputRoot;

        [Header("Input")]
        [SerializeField] private InputActionReference m_ToggleAction;
        [SerializeField] private Key m_ToggleKey = Key.Tab;

        [Header("Display")]
        [SerializeField] private bool m_StartHidden = true;
        [SerializeField] private string m_Header = "Inventory";
        [SerializeField] private string m_KeysHeader = "Keys";
        [SerializeField] private string m_ItemsHeader = "Items";
        [SerializeField] private string m_NoneText = "None";
        [SerializeField] private string m_EmptyText = "Inventory: Empty";

        private bool m_IsVisible;
        private bool m_HasLoggedMissingRefs;
        private InputAction m_CustomToggleAction;
        private InputAction m_BoundToggleAction;
        private Key m_LastToggleKey = Key.None;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_Inventory == null)
            {
                m_Inventory = GetComponentInParent<PlayerInventory>();
                if (m_Inventory == null)
                {
                    m_Inventory = Object.FindFirstObjectByType<PlayerInventory>();
                }
            }

            if (m_OutputText == null)
            {
                m_OutputText = GetComponentInChildren<TMP_Text>();
            }

            if (m_OutputRoot == null && m_OutputText != null)
            {
                m_OutputRoot = m_OutputText.gameObject;
            }

            SetVisible(!m_StartHidden);
        }

        private void OnEnable()
        {
            if (m_Inventory == null || m_OutputText == null)
            {
                LogMissingRefs();
                return;
            }

            BindToggleAction();
            m_Inventory.InventoryChanged += OnInventoryChanged;
            Refresh(m_Inventory.Items);
        }

        private void OnDisable()
        {
            if (m_Inventory != null)
            {
                m_Inventory.InventoryChanged -= OnInventoryChanged;
            }

            UnbindToggleAction();
        }

        private void OnDestroy()
        {
            if (m_CustomToggleAction != null)
            {
                m_CustomToggleAction.Disable();
                m_CustomToggleAction.Dispose();
                m_CustomToggleAction = null;
            }
        }

        private void Update()
        {
            if (m_BoundToggleAction == null || m_LastToggleKey != m_ToggleKey)
            {
                BindToggleAction();
            }

            if (WasTogglePressed())
            {
                SetVisible(!m_IsVisible);
                if (m_IsVisible && m_Inventory != null)
                {
                    Refresh(m_Inventory.Items);
                }
            }
        }

        #endregion

        #region Methods

        private void OnInventoryChanged(IReadOnlyList<ItemDefinition> items)
        {
            if (!m_IsVisible)
            {
                return;
            }

            Refresh(items);
        }

        private void Refresh(IReadOnlyList<ItemDefinition> items)
        {
            if (m_OutputText == null)
            {
                LogMissingRefs();
                return;
            }

            if (items == null || items.Count == 0)
            {
                m_OutputText.text = m_EmptyText;
                return;
            }

            var keys = new List<string>();
            var others = new List<string>();

            for (int i = 0; i < items.Count; i++)
            {
                ItemDefinition item = items[i];
                if (item == null)
                {
                    continue;
                }

                string displayName = string.IsNullOrWhiteSpace(item.DisplayName) ? item.name : item.DisplayName;
                if (item is KeyItemDefinition)
                {
                    keys.Add(displayName);
                }
                else
                {
                    others.Add(displayName);
                }
            }

            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(m_Header))
            {
                builder.AppendLine(m_Header);
            }

            AppendSection(builder, m_KeysHeader, keys);
            AppendSection(builder, m_ItemsHeader, others);

            m_OutputText.text = builder.ToString();
        }

        private void AppendSection(StringBuilder builder, string header, List<string> entries)
        {
            if (!string.IsNullOrWhiteSpace(header))
            {
                builder.AppendLine($"{header}:");
            }

            if (entries.Count == 0)
            {
                builder.AppendLine($"- {m_NoneText}");
                return;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                builder.AppendLine($"- {entries[i]}");
            }
        }

        private void SetVisible(bool visible)
        {
            m_IsVisible = visible;

            if (m_OutputRoot != null)
            {
                m_OutputRoot.SetActive(visible);
            }
        }

        private void BindToggleAction()
        {
            UnbindToggleAction();

            if (m_ToggleAction != null && m_ToggleAction.action != null)
            {
                m_BoundToggleAction = m_ToggleAction.action;
            }
            else
            {
                EnsureCustomToggleAction();
                m_BoundToggleAction = m_CustomToggleAction;
            }

            if (m_BoundToggleAction == null)
            {
                Debug.LogError($"{nameof(InventoryManager)}: Toggle action is missing.", this);
                return;
            }

            m_BoundToggleAction.Enable();
        }

        private void UnbindToggleAction()
        {
            if (m_BoundToggleAction == null)
            {
                return;
            }

            m_BoundToggleAction.Disable();
            m_BoundToggleAction = null;
        }

        private void EnsureCustomToggleAction()
        {
            if (m_CustomToggleAction != null && m_LastToggleKey == m_ToggleKey)
            {
                return;
            }

            if (m_CustomToggleAction != null)
            {
                m_CustomToggleAction.Dispose();
            }

            string bindingPath = GetToggleBindingPath();
            m_CustomToggleAction = new InputAction("ToggleInventory", InputActionType.Button, bindingPath);
            m_LastToggleKey = m_ToggleKey;
        }

        private string GetToggleBindingPath()
        {
            if (Keyboard.current != null)
            {
                KeyControl keyControl = Keyboard.current[m_ToggleKey];
                if (keyControl != null)
                {
                    return keyControl.path;
                }
            }

            return $"<Keyboard>/{m_ToggleKey.ToString().ToLowerInvariant()}";
        }

        private bool WasTogglePressed()
        {
            return m_BoundToggleAction != null
                && m_BoundToggleAction.enabled
                && m_BoundToggleAction.WasPressedThisFrame();
        }

        private void LogMissingRefs()
        {
            if (m_HasLoggedMissingRefs)
            {
                return;
            }

            Debug.LogError($"{nameof(InventoryManager)}: Missing inventory or output text.", this);
            m_HasLoggedMissingRefs = true;
        }

        #endregion
    }
}
