using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdCon.MiniGameTemplate
{
    public class CustomizationSliderGroup : MonoBehaviour
    {
        private const float DEFAULT_SCALE_MULTIPLIER = 1f;

        [SerializeField] private CustomizationSlider opacitySlider;
        [SerializeField] private CustomizationSlider scaleSlider;

        public event Action<float> ScaleSliderValueChanged;
        public event Action<float> OpacitySliderValueChanged;

        #region Public

        public void SetSliderValues(HudElement element, HUDElementsSettings defaultSettings)
        {
            var scaleMultiplier = DEFAULT_SCALE_MULTIPLIER;
            var foundElement = defaultSettings.elements.FirstOrDefault(e => e.elementName == element.Name);

            if (element.Scale != foundElement.scale && foundElement.scale != Vector2.zero)
            {
                scaleMultiplier = element.Scale.x / foundElement.scale.x;
                scaleMultiplier = Mathf.Clamp(scaleMultiplier, scaleSlider.MinSliderValue, scaleSlider.MaxSliderValue);
            }

            scaleSlider.SetSliderValue(scaleMultiplier);
            opacitySlider.SetSliderValue(element.Opacity);
        }

        public void ShowSliders()
        {
            scaleSlider.ShowSlider();
            opacitySlider.ShowSlider();
        }

        public void HideSliders()
        {
            scaleSlider.HideSlider();
            opacitySlider.HideSlider();
        }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            scaleSlider.SliderValueChanged += OnScaleSliderValueChanged;
            opacitySlider.SliderValueChanged += OnOpacitySliderValueChanged;
        }

        private void OnDestroy()
        {
            
        }

        #endregion

        #region Callbacks

        private void OnScaleSliderValueChanged(float value)
        {
            ScaleSliderValueChanged?.Invoke(value);
        }

        private void OnOpacitySliderValueChanged(float value)
        {
            OpacitySliderValueChanged?.Invoke(value);
        }

        #endregion
    }
}
