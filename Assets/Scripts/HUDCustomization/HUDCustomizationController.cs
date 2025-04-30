using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EdCon.MiniGameTemplate
{
    public class HUDCustomizationController : MonoBehaviour
    {
        private const string TOAST_MESSAGE_SETTINGS_SAVED = "Layout Scheme Saved";
        private const string SETTINGS_FILE_NAME = "hud_user_settings.json";

        [SerializeField] private HudElement[] hudElements;
        [SerializeField] private CustomizationSliderGroup sliderGroup;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button defaultButton;
        [SerializeField] private ToastController toast;

        private HudElement selectedElement;
        private HUDSettingsJSONSerializer settingsJSONSerializer;
        private HUDElementsSettings playerSettings;
        private HUDElementsSettings defaultSettings;
        private string settingsFilePath;

        #region LifeCycle

        private void Awake()
        {
            foreach (var element in hudElements)
            {
                element.ElementSelected += OnElementSelected;
            }

            sliderGroup.OpacitySliderValueChanged += OnOpacityChanged;
            sliderGroup.ScaleSliderValueChanged += OnScaleChanged;
            saveButton.onClick.AddListener(OnSaveButtonClicked);
            defaultButton.onClick.AddListener(OnDefaultButtonClicked);

            settingsFilePath = Path.Combine(Application.persistentDataPath, SETTINGS_FILE_NAME);
            settingsJSONSerializer = new HUDSettingsJSONSerializer();
            settingsJSONSerializer.SettingsDeserialized += OnSettingsDeserialized;
            settingsJSONSerializer.SettingsSerialized += OnSettingsSerialized;
        }

        private void Start()
        {
            SaveDefaultSettings();
            settingsJSONSerializer.DeserializeSettings(settingsFilePath);
        }

        private void OnDestroy()
        {
            foreach (var element in hudElements)
            {
                element.ElementSelected -= OnElementSelected;
            }

            sliderGroup.OpacitySliderValueChanged -= OnOpacityChanged;
            sliderGroup.ScaleSliderValueChanged -= OnScaleChanged;
            saveButton.onClick.RemoveListener(OnSaveButtonClicked);
            defaultButton.onClick.RemoveListener(OnDefaultButtonClicked);
            settingsJSONSerializer.SettingsDeserialized -= OnSettingsDeserialized;
            settingsJSONSerializer.SettingsSerialized -= OnSettingsSerialized;
        }

        #endregion

        #region Callbacks

        private void OnElementSelected(HudElement selectedElement)
        {
            this.selectedElement = selectedElement;

            foreach (var element in hudElements)
            {
                element.SetSelected(element == selectedElement);
            }

            sliderGroup.SetSliderValues(selectedElement, defaultSettings);
            sliderGroup.ShowSliders();
        }

        private void OnOpacityChanged(float value)
        {
            selectedElement.Opacity = value;
        }

        private void OnScaleChanged(float value)
        {
            var foundElement = defaultSettings.elements.FirstOrDefault(e => e.elementName == selectedElement.Name);

            if (foundElement != null)
            {
                var defaultSize = foundElement.scale;
                Vector2 newSize = defaultSize * value;
                selectedElement.Scale = newSize;
            }
        }

        private void OnSaveButtonClicked()
        {
            if (selectedElement != null)
            {
                selectedElement.SetSelected(false);
            }

            sliderGroup.HideSliders();
            settingsJSONSerializer.SerializeSettings(hudElements, settingsFilePath);
        }

        private void OnDefaultButtonClicked()
        {
            sliderGroup.HideSliders();
            ApplyPlayerSettings(defaultSettings);
        }

        private void OnSettingsSerialized()
        {
            if (selectedElement != null)
            {
                selectedElement.SetSelected(false);
            }

            toast.ShowToast(TOAST_MESSAGE_SETTINGS_SAVED);
        }

        private void OnSettingsDeserialized(HUDElementsSettings settingsList)
        {
            if (settingsList != null)
            {
                playerSettings = settingsList;
                ApplyPlayerSettings(playerSettings);
            }
            else
            {
                Debug.LogWarning($"User settings not found. Loading defaults.");
                ApplyPlayerSettings(defaultSettings);
            }
        }

        #endregion

        #region Private

        private void ApplyPlayerSettings(HUDElementsSettings settingsList)
        {
            foreach (var element in settingsList.elements)
            {
                var foundElement = hudElements.FirstOrDefault(e => e.Name == element.elementName);

                if (foundElement != null)
                {
                    foundElement.Opacity = element.alpha;
                    foundElement.Scale = element.scale;
                    foundElement.Position = element.position;
                }
            }
        }

        private void SaveDefaultSettings()
        {
            var settings = new HUDElementsSettings
            {
                elements = new List<HUDElementSettingsData>()
            };

            foreach (var element in hudElements)
            {
                settings.elements.Add(new HUDElementSettingsData
                {
                    elementName = element.Name,
                    alpha = element.Opacity,
                    scale = element.Scale,
                    position = element.Position
                });
            }

            defaultSettings = settings;
        }

        #endregion
    }
}
