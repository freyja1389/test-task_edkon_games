using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EdCon.MiniGameTemplate
{
    public class HUDSettingsJSONSerializer
    {
        private const string FILE_NAME = "hud_user_settings.json";

        public event Action<HUDElementsSettings> SettingsDeserialized;
        public event Action SettingsSerialized;

        public void SerializeSettings(HudElement[] hudElements, string path)
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

            string json = JsonUtility.ToJson(settings, true);
            File.WriteAllText(path, json);
            SettingsSerialized?.Invoke();
        }

        public void DeserializeSettings(string path)
        {
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

            SettingsDeserialized?.Invoke(outputSettings);
        }
    }
}
