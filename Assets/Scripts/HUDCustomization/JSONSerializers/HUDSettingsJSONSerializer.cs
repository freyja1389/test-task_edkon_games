using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EdCon.MiniGameTemplate
{
    public class HUDSettingsJSONSerializer
    {
        private const string FILE_NAME = "hud_user_settings.json";

        public event Action<HUDElementsSettings> SettingsLoaded;
        public event Action SettingsSaved;

        public void SaveSettings(List<HudElement> hudElements)
        {
            var saveDataList = new List<HUDElementSettingsData>();

            foreach (var element in hudElements)
            {
                saveDataList.Add(new HUDElementSettingsData
                {
                    elementName = element.name,
                    alpha = element.Opacity,
                    scale = element.Scale,
                    position = element.Position
                });
            }

            var settingList = new HUDElementsSettings { elements = saveDataList };
            string json = JsonUtility.ToJson(settingList, true);
            string path = Path.Combine(Application.persistentDataPath, FILE_NAME);
            File.WriteAllText(path, json);

            Debug.Log($"Layout scheme saved!");
            SettingsSaved?.Invoke();
        }

        public void LoadUserSettings()
        {
            string path = Path.Combine(Application.persistentDataPath, FILE_NAME);
            HUDElementsSettings outputSettings = null;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                if (!string.IsNullOrEmpty(json))
                {
                    var settingList = JsonUtility.FromJson<HUDElementsSettings>(json);

                    if (settingList != null && settingList.elements != null && settingList.elements.Count > 0)
                    {
                        outputSettings = settingList;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"User settings not found. Loading defaults.");
            }

            SettingsLoaded?.Invoke(outputSettings);
        }
    }
}
