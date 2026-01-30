using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WorldInteractionSystem.Runtime.UI
{
    /// <summary>
    /// Handles interaction prompt, status text, and hold progress UI.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private GameObject m_Root;
        [SerializeField] private TMP_Text m_PromptText;
        [SerializeField] private TMP_Text m_StatusText;
        [SerializeField] private Slider m_HoldProgressSlider;
        [SerializeField] private Image m_HoldProgressImage;

        private bool m_HasLoggedMissingRefs;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_PromptText == null || m_StatusText == null || (m_HoldProgressSlider == null && m_HoldProgressImage == null))
            {
                Debug.LogError($"{nameof(InteractionPromptUI)}: UI references are missing.", this);
                m_HasLoggedMissingRefs = true;
            }
        }

        private void OnDisable()
        {
            ResetHoldProgress();
            if (m_HoldProgressImage != null)
            {
                m_HoldProgressImage.fillAmount = 0f;
                m_HoldProgressImage.enabled = false;
            }

            if (m_HoldProgressSlider != null)
            {
                m_HoldProgressSlider.value = 0f;
                m_HoldProgressSlider.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the main prompt text.
        /// </summary>
        /// <param name="text">Prompt text.</param>
        /// <param name="visible">Whether to show prompt.</param>
        public void SetPrompt(string text, bool visible)
        {
            if (m_PromptText == null)
            {
                LogMissingRefs();
                return;
            }

            m_PromptText.text = text;
            m_PromptText.enabled = visible;
            UpdateRootVisibility();
        }

        /// <summary>
        /// Sets the status text (e.g., out of range, locked).
        /// </summary>
        /// <param name="text">Status text.</param>
        /// <param name="visible">Whether to show status.</param>
        public void SetStatus(string text, bool visible)
        {
            if (m_StatusText == null)
            {
                LogMissingRefs();
                return;
            }

            m_StatusText.text = text;
            m_StatusText.enabled = visible;
            UpdateRootVisibility();
        }

        /// <summary>
        /// Sets hold progress fill.
        /// </summary>
        /// <param name="normalizedProgress">Progress from 0 to 1.</param>
        /// <param name="visible">Whether to show progress.</param>
        public void SetHoldProgress(float normalizedProgress, bool visible)
        {
            if (m_HoldProgressSlider == null && m_HoldProgressImage == null)
            {
                LogMissingRefs();
                return;
            }

            float clamped = Mathf.Clamp01(normalizedProgress);
            bool shouldShow = visible && clamped < 1f;

            if (m_HoldProgressSlider != null)
            {
                m_HoldProgressSlider.value = clamped;
                m_HoldProgressSlider.gameObject.SetActive(shouldShow);
            }

            if (m_HoldProgressImage != null)
            {
                m_HoldProgressImage.fillAmount = clamped;
                m_HoldProgressImage.enabled = shouldShow;
            }

            if (!shouldShow)
            {
                ResetHoldProgress();
            }

            UpdateRootVisibility();
        }

        /// <summary>
        /// Clears all UI elements.
        /// </summary>
        public void Clear()
        {
            if (m_PromptText != null)
            {
                m_PromptText.text = string.Empty;
                m_PromptText.enabled = false;
            }

            if (m_StatusText != null)
            {
                m_StatusText.text = string.Empty;
                m_StatusText.enabled = false;
            }

            if (m_HoldProgressImage != null)
            {
                m_HoldProgressImage.fillAmount = 0f;
                m_HoldProgressImage.enabled = false;
            }

            if (m_HoldProgressSlider != null)
            {
                m_HoldProgressSlider.value = 0f;
                m_HoldProgressSlider.gameObject.SetActive(false);
            }

            UpdateRootVisibility();
        }

        private void UpdateRootVisibility()
        {
            if (m_Root == null)
            {
                return;
            }

            bool anyVisible = false;
            if (m_PromptText != null && m_PromptText.enabled)
            {
                anyVisible = true;
            }

            if (m_StatusText != null && m_StatusText.enabled)
            {
                anyVisible = true;
            }

            bool holdVisible = false;
            if (m_HoldProgressImage != null && m_HoldProgressImage.enabled)
            {
                holdVisible = true;
            }

            if (m_HoldProgressSlider != null && m_HoldProgressSlider.gameObject.activeSelf)
            {
                holdVisible = true;
            }

            if (holdVisible)
            {
                anyVisible = true;
            }

            m_Root.SetActive(anyVisible);
        }

        private void LogMissingRefs()
        {
            if (m_HasLoggedMissingRefs)
            {
                return;
            }

            Debug.LogError($"{nameof(InteractionPromptUI)}: UI references are missing.", this);
            m_HasLoggedMissingRefs = true;
        }

        private void ResetHoldProgress()
        {
            if (m_HoldProgressImage != null)
            {
                m_HoldProgressImage.fillAmount = 0f;
                m_HoldProgressImage.enabled = false;
            }

            if (m_HoldProgressSlider != null)
            {
                m_HoldProgressSlider.value = 0f;
                m_HoldProgressSlider.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
