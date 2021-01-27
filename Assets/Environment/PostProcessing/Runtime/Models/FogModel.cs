using System;

namespace UnityEngine.PostProcessing {
    [Serializable]
    public class FogModel : PostProcessingModel {
        [SerializeField] private Settings m_Settings = Settings.defaultSettings;

        public Settings settings {
            get => m_Settings;
            set => m_Settings = value;
        }

        public override void Reset() {
            m_Settings = Settings.defaultSettings;
        }

        [Serializable]
        public struct Settings {
            [Tooltip("Should the fog affect the skybox?")]
            public bool excludeSkybox;

            public static Settings defaultSettings =>
                new Settings {
                    excludeSkybox = true
                };
        }
    }
}