using UnityEngine;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Player
{
    /// <summary>
    /// Detects nearby interactables using overlap and raycast.
    /// </summary>
    public class InteractionDetector : MonoBehaviour
    {
        #region Fields

        [Header("Detection")]
        [SerializeField] private Transform m_Source;
        [SerializeField] private float m_InteractionRange = 3f;
        [SerializeField] private float m_FocusRange = 6f;
        [SerializeField] private LayerMask m_InteractableMask = ~0;
        [SerializeField] [Range(0f, 180f)] private float m_ViewAngle = 180f;
        [SerializeField] private QueryTriggerInteraction m_TriggerInteraction = QueryTriggerInteraction.Ignore;
        [SerializeField] private int m_MaxOverlapResults = 32;

        private Collider[] m_OverlapResults;

        #endregion

        #region Properties

        /// <summary>
        /// Interaction origin transform.
        /// </summary>
        public Transform Source => m_Source;

        /// <summary>
        /// Current interaction range.
        /// </summary>
        public float InteractionRange => m_InteractionRange;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_Source == null)
            {
                m_Source = transform;
                Debug.LogWarning(
                    $"{nameof(InteractionDetector)}: Source not assigned. Using own transform.",
                    this);
            }

            EnsureOverlapBuffer();
        }

        private void OnValidate()
        {
            if (m_InteractionRange < 0f)
            {
                Debug.LogWarning($"{nameof(InteractionDetector)}: InteractionRange was negative.", this);
                m_InteractionRange = 0f;
            }

            if (m_FocusRange < m_InteractionRange)
            {
                Debug.LogWarning(
                    $"{nameof(InteractionDetector)}: FocusRange was below InteractionRange. Clamping.",
                    this);
                m_FocusRange = m_InteractionRange;
            }

            if (m_MaxOverlapResults < 1)
            {
                Debug.LogWarning($"{nameof(InteractionDetector)}: MaxOverlapResults was below 1. Clamping.", this);
                m_MaxOverlapResults = 1;
            }

            EnsureOverlapBuffer();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Transform source = m_Source != null ? m_Source : transform;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(source.position, m_InteractionRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(source.position, m_FocusRange);

            Gizmos.color = Color.white;
            Gizmos.DrawRay(source.position, source.forward * m_FocusRange);
        }
#endif

        #endregion

        #region Methods

        /// <summary>
        /// Finds the closest interactable within range.
        /// </summary>
        /// <param name="interactable">Found interactable.</param>
        /// <param name="distance">Distance to interactable.</param>
        /// <returns>True if found; otherwise false.</returns>
        public bool TryGetInteractableInRange(out InteractableBase interactable, out float distance)
        {
            interactable = null;
            distance = 0f;

            if (m_Source == null)
            {
                Debug.LogError($"{nameof(InteractionDetector)}: Source is missing.", this);
                return false;
            }

            if (m_OverlapResults == null || m_OverlapResults.Length != m_MaxOverlapResults)
            {
                EnsureOverlapBuffer();
            }

            int hitCount = Physics.OverlapSphereNonAlloc(
                m_Source.position,
                m_InteractionRange,
                m_OverlapResults,
                m_InteractableMask,
                m_TriggerInteraction);

            if (hitCount == 0)
            {
                return false;
            }

            if (m_OverlapResults != null && hitCount >= m_OverlapResults.Length)
            {
                Debug.LogWarning(
                    $"{nameof(InteractionDetector)}: Overlap results exceeded buffer size. Increase capacity.",
                    this);
            }

            float bestDistance = float.MaxValue;
            InteractableBase bestInteractable = null;

            for (int i = 0; i < hitCount; i++)
            {
                Collider candidate = m_OverlapResults[i];
                if (candidate == null)
                {
                    continue;
                }

                InteractableBase candidateInteractable = candidate.GetComponentInParent<InteractableBase>();
                if (candidateInteractable == null)
                {
                    continue;
                }

                if (!IsWithinView(candidateInteractable.transform.position))
                {
                    continue;
                }

                Vector3 closestPoint = candidate.ClosestPoint(m_Source.position);
                float candidateDistance = Vector3.Distance(m_Source.position, closestPoint);

                if (candidateDistance < bestDistance)
                {
                    bestDistance = candidateDistance;
                    bestInteractable = candidateInteractable;
                }
            }

            if (bestInteractable == null)
            {
                return false;
            }

            interactable = bestInteractable;
            distance = bestDistance;
            return true;
        }

        /// <summary>
        /// Finds the interactable currently in focus (raycast).
        /// </summary>
        /// <param name="interactable">Found interactable.</param>
        /// <param name="distance">Distance to interactable.</param>
        /// <returns>True if found; otherwise false.</returns>
        public bool TryGetFocusedInteractable(out InteractableBase interactable, out float distance)
        {
            interactable = null;
            distance = 0f;

            if (m_Source == null)
            {
                Debug.LogError($"{nameof(InteractionDetector)}: Source is missing.", this);
                return false;
            }

            if (Physics.Raycast(
                m_Source.position,
                m_Source.forward,
                out RaycastHit hit,
                m_FocusRange,
                m_InteractableMask,
                m_TriggerInteraction))
            {
                InteractableBase focused = hit.collider.GetComponentInParent<InteractableBase>();
                if (focused != null)
                {
                    interactable = focused;
                    distance = hit.distance;
                    return true;
                }
            }

            return false;
        }

        private bool IsWithinView(Vector3 worldPosition)
        {
            Vector3 direction = worldPosition - m_Source.position;
            if (direction == Vector3.zero)
            {
                return true;
            }

            float angle = Vector3.Angle(m_Source.forward, direction);
            return angle <= m_ViewAngle * 0.5f;
        }

        private void EnsureOverlapBuffer()
        {
            if (m_MaxOverlapResults < 1)
            {
                m_MaxOverlapResults = 1;
            }

            if (m_OverlapResults == null || m_OverlapResults.Length != m_MaxOverlapResults)
            {
                m_OverlapResults = new Collider[m_MaxOverlapResults];
            }
        }

        #endregion
    }
}
