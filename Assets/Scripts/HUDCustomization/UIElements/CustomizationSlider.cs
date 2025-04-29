using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EdCon.MiniGameTemplate
{
    public class CustomizationSlider : MonoBehaviour
    {
        private const float PERCENT_MULTIPLIER = 100f;
        private const float ANIM_TIME = 0.2f;
        private const float FULL_TRANSPARENT_VALUE = 0f;
        private const float FULL_OPACITY_VALUE = 1f;

        public event Action<float> SliderValueChanged;

        [SerializeField] private Text sliderValueText;
        [SerializeField] private Slider slider;
        [SerializeField] private CanvasGroup sliderCanvasGroup;

        #region Public

        public float MinSliderValue
        {
            get { return slider.minValue; }
        }

        public float MaxSliderValue
        {
            get { return slider.maxValue; }
        }

        public void SetSliderValue(float value)
        {
            slider.value = value;
            ChangeSliderValueText(value);
        }

        public void ShowSlider()
        {
            sliderCanvasGroup.DOKill();
            sliderCanvasGroup.alpha = FULL_TRANSPARENT_VALUE;
            sliderCanvasGroup.DOFade(FULL_OPACITY_VALUE, ANIM_TIME)
                .SetEase(Ease.InQuad).OnStart(() =>
                {
                    sliderCanvasGroup.interactable = true;
                    sliderCanvasGroup.blocksRaycasts = true;
                });
        }

        public void HideSlider()
        {
            sliderCanvasGroup.DOKill();
            sliderCanvasGroup.DOFade(FULL_TRANSPARENT_VALUE, ANIM_TIME)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    sliderCanvasGroup.interactable = false;
                    sliderCanvasGroup.blocksRaycasts = false;
                });

        }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        #endregion

        #region Callbacks

        private void OnSliderValueChanged(float value)
        {
            ChangeSliderValueText(value);
            SliderValueChanged?.Invoke(value);
        }

        #endregion

        #region Private 

        private void ChangeSliderValueText(float value)
        {
            sliderValueText.text = $"{Mathf.RoundToInt(value * PERCENT_MULTIPLIER)}%";
        }

        #endregion
    }
}
