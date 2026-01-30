using UnityEngine;
using UnityEngine.Events;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Toggle switch that invokes events.
    /// </summary>
    public class SwitchInteractable : ToggleInteractableBase
    {
        #region Fields

        [Header("Events")]
        [SerializeField] private UnityEvent m_OnSwitchedOn;
        [SerializeField] private UnityEvent m_OnSwitchedOff;

        #endregion

        #region Properties

        /// <summary>
        /// Interaction type for switch.
        /// </summary>
        public override InteractionType InteractionType => InteractionType.Toggle;

        #endregion

        #region Methods

        protected override string GetInteractionVerb(InteractorContext context)
        {
            return IsOn ? "Turn Off" : "Turn On";
        }

        protected override void OnToggled(InteractorContext context, bool isOn)
        {
            if (isOn)
            {
                m_OnSwitchedOn?.Invoke();
            }
            else
            {
                m_OnSwitchedOff?.Invoke();
            }
        }

        #endregion
    }
}
