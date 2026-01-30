using UnityEngine;

namespace WorldInteractionSystem.Runtime.Items
{
    /// <summary>
    /// Scriptable definition for a key item.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Key", menuName = "WorldInteractionSystem/Items/Key")]
    public class KeyItemDefinition : ItemDefinition
    {
        #region Fields

        [Header("Key")]
        [SerializeField] private string m_KeyId = "key_id";

        #endregion

        #region Properties

        /// <summary>
        /// Key identifier used by locks.
        /// </summary>
        public string KeyId => m_KeyId;

        #endregion

        #region Unity Methods

        protected override void OnValidate()
        {
            base.OnValidate();

            if (string.IsNullOrWhiteSpace(m_KeyId))
            {
                Debug.LogWarning($"{name}: KeyId is empty.", this);
            }
        }

        #endregion
    }
}
