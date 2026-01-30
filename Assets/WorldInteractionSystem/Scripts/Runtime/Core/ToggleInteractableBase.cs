using UnityEngine;

namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Base class for toggle interactions.
    /// </summary>
    public abstract class ToggleInteractableBase : InteractableBase, IToggleInteractable
    {
        #region Fields

        [Header("Toggle")]
        [SerializeField] private bool m_IsOn;

        #endregion

        #region Properties

        /// <summary>
        /// Current toggle state.
        /// </summary>
        public bool IsOn => m_IsOn;

        #endregion

        #region Methods

        /// <summary>
        /// Called when the toggle state changes.
        /// </summary>
        /// <param name="context">Interaction context.</param>
        /// <param name="isOn">New state.</param>
        protected abstract void OnToggled(InteractorContext context, bool isOn);

        /// <summary>
        /// Sets the toggle state.
        /// </summary>
        /// <param name="isOn">Target state.</param>
        /// <param name="context">Interaction context.</param>
        protected void SetIsOn(bool isOn, InteractorContext context)
        {
            if (m_IsOn == isOn)
            {
                return;
            }

            m_IsOn = isOn;
            OnToggled(context, isOn);
        }

        #endregion

        #region Interface Implementations

        bool IToggleInteractable.IsOn => IsOn;

        void IToggleInteractable.Toggle(InteractorContext context)
        {
            if (!CanInteractInternal(context, out string reason))
            {
                Debug.LogWarning($"{name}: Cannot toggle. {reason}", this);
                return;
            }

            SetIsOn(!m_IsOn, context);
        }

        #endregion
    }
}
