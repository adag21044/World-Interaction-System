using UnityEngine;
using UnityEngine.Events;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Toggle switch that can be triggered by overlap and invokes events.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerSwitchInteractable : ToggleInteractableBase
    {
        #region Fields

        [Header("Events")]
        [SerializeField] private UnityEvent m_OnSwitchedOn;
        [SerializeField] private UnityEvent m_OnSwitchedOff;

        [Header("Trigger Activation")]
        [SerializeField] private bool m_UseTriggerActivation = true;
        [SerializeField] private LayerMask m_ActivatorMask = ~0;
        [SerializeField] private bool m_ToggleOffOnExit = true;

        [Header("Debug")]
        [SerializeField] private bool m_LogTriggerEvents;

        private int m_ActiveTriggerCount;

        #endregion

        #region Properties

        /// <summary>
        /// Interaction type for trigger switch.
        /// </summary>
        public override InteractionType InteractionType => InteractionType.Toggle;

        #endregion

        #region Unity Methods

        private void OnDisable()
        {
            m_ActiveTriggerCount = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_UseTriggerActivation || !IsValidActivator(other))
            {
                return;
            }

            m_ActiveTriggerCount++;
            if (m_ActiveTriggerCount != 1)
            {
                return;
            }

            InteractorContext context = BuildTriggerContext(other);
            if (!CanInteractInternal(context, out string reason))
            {
                Debug.LogWarning($"{name}: Trigger enter blocked. {reason}", this);
                return;
            }

            if (m_LogTriggerEvents)
            {
                Debug.Log($"{name}: Trigger enter -> ON.", this);
            }

            SetIsOn(true, context);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!m_UseTriggerActivation || !IsValidActivator(other))
            {
                return;
            }

            m_ActiveTriggerCount = Mathf.Max(0, m_ActiveTriggerCount - 1);
            if (!m_ToggleOffOnExit || m_ActiveTriggerCount > 0)
            {
                return;
            }

            InteractorContext context = BuildTriggerContext(other);
            if (!CanInteractInternal(context, out string reason))
            {
                Debug.LogWarning($"{name}: Trigger exit blocked. {reason}", this);
                return;
            }

            if (m_LogTriggerEvents)
            {
                Debug.Log($"{name}: Trigger exit -> OFF.", this);
            }

            SetIsOn(false, context);
        }

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

        private bool IsValidActivator(Collider other)
        {
            if (other == null)
            {
                return false;
            }

            int layerMask = 1 << other.gameObject.layer;
            return (m_ActivatorMask.value & layerMask) != 0;
        }

        private InteractorContext BuildTriggerContext(Collider other)
        {
            GameObject interactor = other != null ? other.gameObject : gameObject;
            Transform source = other != null ? other.transform : transform;
            return new InteractorContext(interactor, source, null);
        }

        #endregion
    }
}
