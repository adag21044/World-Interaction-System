using UnityEngine;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.Items;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Instant interaction that adds a key to the inventory.
    /// </summary>
    public class KeyPickupInteractable : InstantInteractableBase
    {
        #region Fields

        private const string k_AlreadyCollectedText = "Already collected";

        [Header("Key")]
        [SerializeField] private KeyItemDefinition m_KeyDefinition;
        [SerializeField] private bool m_DestroyOnPickup = true;
        [SerializeField] private bool m_DisableOnPickup;

        #endregion

        #region Properties

        /// <summary>
        /// Interaction type for key pickup.
        /// </summary>
        public override InteractionType InteractionType => InteractionType.Instant;

        #endregion

        #region Methods

        protected override string GetInteractionVerb(InteractorContext context)
        {
            return "Pick Up";
        }

        protected override bool CanInteractInternal(InteractorContext context, out string reason)
        {
            if (!base.CanInteractInternal(context, out reason))
            {
                return false;
            }

            if (m_KeyDefinition == null)
            {
                Debug.LogError($"{nameof(KeyPickupInteractable)}: KeyDefinition is missing.", this);
                reason = "Key missing";
                return false;
            }

            if (context.Inventory == null)
            {
                Debug.LogError($"{nameof(KeyPickupInteractable)}: Inventory is missing.", this);
                reason = "Inventory missing";
                return false;
            }

            if (context.Inventory.ContainsKey(m_KeyDefinition))
            {
                reason = k_AlreadyCollectedText;
                return false;
            }

            reason = string.Empty;
            return true;
        }

        protected override void OnInstantInteract(InteractorContext context)
        {
            if (context.Inventory == null)
            {
                Debug.LogError($"{nameof(KeyPickupInteractable)}: Inventory is missing.", this);
                return;
            }

            if (m_KeyDefinition == null)
            {
                Debug.LogError($"{nameof(KeyPickupInteractable)}: KeyDefinition is missing.", this);
                return;
            }

            bool added = context.Inventory.TryAddItem(m_KeyDefinition);
            if (!added)
            {
                return;
            }

            if (m_DestroyOnPickup)
            {
                Destroy(gameObject);
            }
            else if (m_DisableOnPickup)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
