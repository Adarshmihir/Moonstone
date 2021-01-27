using System;

namespace UnityEngine.PostProcessing {
    [Serializable]
    public class MotionBlurModel : PostProcessingModel {
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
            [Range(0f, 360f)] [Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
            public float shutterAngle;

            [Range(4, 32)] [Tooltip("The amount of sample points, which affects quality and performances.")]
            public int sampleCount;

            [Range(0f, 1f)]
            [Tooltip(
                "The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
            public float frameBlending;

            public static Settings defaultSettings =>
                new Settings {
                    shutterAngle = 270f,
                    sampleCount = 10,
                    frameBlending = 0f
                };
        }
    }
}