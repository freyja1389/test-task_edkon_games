using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EdCon.MiniGameTemplate
{
    public class HUDCustomizationController : MonoBehaviour
    {
        private const float DEFAULT_SCALE_MULTIPLIER = 1f;
        private const string TOAST_MESSAGE_SETTINGS_SAVED = "Layout Scheme Saved";

        [SerializeField] private List<HudElement> hudElements;
        [SerializeField] private CustomizationSlider opacitySlider;
        [SerializeField] private CustomizationSlider scaleSlider;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button defaultButton;
        [SerializeField] private ToastController toast;


        private HudElement selectedElement;
        private HUDSettingsJSONSerializer settingsJSONSerializer;
        private HUDElementsSettings playerSettings;

        #region LifeCycle

        private void Awake()
        {
            foreach (var element in hudElements)
            {
                element.ElementSelected += OnElementSelected;
            }

            opacitySlider.SliderValueChanged += OnOpacityChanged;
            scaleSlider.SliderValueChanged += OnScaleChanged;
            saveButton.onClick.AddListener(OnSaveButtonClicked);
            defaultButton.onClick.AddListener(OnDefaultButtonClicked);

            settingsJSONSerializer = new HUDSettingsJSONSerializer();
            settingsJSONSerializer.SettingsLoaded += OnSettingsLoaded;
        }

        private void Start()
        {
            settingsJSONSerializer.LoadUserSettings();
        }

        private void OnDestroy()
        {
            foreach (var element in hudElements)
            {
                element.ElementSelected -= OnElementSelected;
            }

            opacitySlider.SliderValueChanged -= OnOpacityChanged;
            scaleSlider.SliderValueChanged -= OnScaleChanged;
            saveButton.onClick.RemoveListener(OnSaveButtonClicked);
            defaultButton.onClick.RemoveListener(OnDefaultButtonClicked);
        }

        #endregion

        #region Callbacks

        private void OnElementSelected(HudElement element)
        {
            selectedElement = element;

            foreach (var elem in hudElements)
            {
                elem.IsSelected = (elem == element);
            }

            SetSliderValues(element);
            scaleSlider.ShowSlider();
            opacitySlider.ShowSlider();
        }

        private void OnOpacityChanged(float value)
        {
            selectedElement.Opacity = value;
        }

        private void OnScaleChanged(float value)
        {
            float invertedValue = DEFAULT_SCALE_MULTIPLIER - value;
            var defaultSize = selectedElement.DefaultScale;
            Vector2 newSize = defaultSize * value;
            selectedElement.Scale = newSize;
        }

        private void OnSaveButtonClicked()
        {
            selectedElement.IsSelected = false;
            scaleSlider.HideSlider();
            opacitySlider.HideSlider();
            settingsJSONSerializer.SettingsSaved += OnSettingsSaved;
            settingsJSONSerializer.SaveSettings(hudElements);
        }

        private void OnDefaultButtonClicked()
        {
            scaleSlider.HideSlider();
            opacitySlider.HideSlider();
            ApplyDefaultSettings();
        }

        private void OnSettingsSaved()
        {
            selectedElement.IsSelected = false;
            settingsJSONSerializer.SettingsSaved -= OnSettingsSaved;
            toast.ShowToast(TOAST_MESSAGE_SETTINGS_SAVED);
        }

        private void OnSettingsLoaded(HUDElementsSettings settingsList)
        {
            if (settingsJSONSerializer != null)
            {
                settingsJSONSerializer.SettingsLoaded -= OnSettingsLoaded;
            }

            if (settingsList != null)
            {
                playerSettings = settingsList;
                ApplyPlayerSettings(playerSettings);
            }
        }

        #endregion

        #region Private

        private void ApplyPlayerSettings(HUDElementsSettings settingsList)
        {
            foreach (var elem in settingsList.elements)
            {
                var foundElement = hudElements.FirstOrDefault(element => element.Name == elem.elementName);

                if (foundElement != null)
                {
                    foundElement.Opacity = elem.alpha;
                    foundElement.Scale = elem.scale;
                    foundElement.Position = elem.position;
                }
            }
        }

        private void ApplyDefaultSettings()
        {
            foreach (HudElement elem in hudElements)
            {
                elem.Opacity = elem.DefaultOpacity;
                elem.Position = elem.DefaultPosition;
                elem.Scale = elem.DefaultScale;
            }
        }

        private void SetSliderValues(HudElement element)
        {
            var scaleMultiplier = DEFAULT_SCALE_MULTIPLIER;

            if (element.Scale != element.DefaultScale && element.DefaultScale != Vector2.zero)
            {
                scaleMultiplier = element.Scale.x / element.DefaultScale.x;
                scaleMultiplier = Mathf.Clamp(scaleMultiplier, scaleSlider.MinSliderValue, scaleSlider.MaxSliderValue);
            }

            scaleSlider.SetSliderValue(scaleMultiplier);
            opacitySlider.SetSliderValue(element.Opacity);
        }

        #endregion
    }
}
