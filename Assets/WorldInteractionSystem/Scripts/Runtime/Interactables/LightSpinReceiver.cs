using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Receives events to toggle a light and spin a target.
    /// </summary>
    public class LightSpinReceiver : MonoBehaviour
    {
        #region Fields

        private const float k_MinSpinDuration = 0.05f;

        [Header("Light")]
        [SerializeField] private GameObject m_LightObject;
        [SerializeField] private bool m_EnableLightOnActivate = true;
        [SerializeField] private bool m_DisableLightOnDeactivate = true;

        [Header("Spin")]
        [SerializeField] private Transform m_SpinTarget;
        [SerializeField] private Vector3 m_SpinEulerPerLoop = new Vector3(0f, 360f, 0f);
        [SerializeField] private float m_SpinDuration = 1.2f;
        [SerializeField] private bool m_StopSpinOnDeactivate = true;

#if DOTWEEN
        private Tween m_SpinTween;
#endif

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_SpinTarget == null)
            {
                m_SpinTarget = transform;
            }
        }

        private void OnDisable()
        {
            StopSpin();
        }

        private void OnValidate()
        {
            if (m_SpinDuration < k_MinSpinDuration)
            {
                Debug.LogWarning(
                    $"{nameof(LightSpinReceiver)}: SpinDuration was below {k_MinSpinDuration}. Clamping value.",
                    this);
                m_SpinDuration = k_MinSpinDuration;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enables the light and starts spinning.
        /// </summary>
        public void Activate()
        {
            if (m_EnableLightOnActivate && m_LightObject != null)
            {
                m_LightObject.SetActive(true);
            }

            StartSpin();
        }

        /// <summary>
        /// Disables the light and stops spinning.
        /// </summary>
        public void Deactivate()
        {
            if (m_DisableLightOnDeactivate && m_LightObject != null)
            {
                m_LightObject.SetActive(false);
            }

            if (m_StopSpinOnDeactivate)
            {
                StopSpin();
            }
        }

        private void StartSpin()
        {
#if DOTWEEN
            if (m_SpinTarget == null)
            {
                return;
            }

            m_SpinTween?.Kill();
            m_SpinTween = m_SpinTarget
                .DOLocalRotate(m_SpinEulerPerLoop, m_SpinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
#else
            if (m_SpinTarget != null)
            {
                Debug.LogWarning(
                    $"{nameof(LightSpinReceiver)}: DOTween is not installed. Rotation skipped.",
                    this);
            }
#endif
        }

        private void StopSpin()
        {
#if DOTWEEN
            if (m_SpinTween == null)
            {
                return;
            }

            m_SpinTween.Kill();
            m_SpinTween = null;
#endif
        }

        #endregion
    }
}
