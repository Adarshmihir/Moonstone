using UnityEngine;
using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing {
    using VignetteMode = VignetteModel.Mode;

    [PostProcessingModelEditor(typeof(VignetteModel))]
    public class VignetteModelEditor : PostProcessingModelEditor {
        private SerializedProperty m_Center;
        private SerializedProperty m_Color;
        private SerializedProperty m_Intensity;
        private SerializedProperty m_Mask;
        private SerializedProperty m_Mode;
        private SerializedProperty m_Opacity;
        private SerializedProperty m_Rounded;
        private SerializedProperty m_Roundness;
        private SerializedProperty m_Smoothness;

        public override void OnEnable() {
            m_Mode = FindSetting((VignetteModel.Settings x) => x.mode);
            m_Color = FindSetting((VignetteModel.Settings x) => x.color);
            m_Center = FindSetting((VignetteModel.Settings x) => x.center);
            m_Intensity = FindSetting((VignetteModel.Settings x) => x.intensity);
            m_Smoothness = FindSetting((VignetteModel.Settings x) => x.smoothness);
            m_Roundness = FindSetting((VignetteModel.Settings x) => x.roundness);
            m_Mask = FindSetting((VignetteModel.Settings x) => x.mask);
            m_Opacity = FindSetting((VignetteModel.Settings x) => x.opacity);
            m_Rounded = FindSetting((VignetteModel.Settings x) => x.rounded);
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.PropertyField(m_Mode);
            EditorGUILayout.PropertyField(m_Color);

            if (m_Mode.intValue < (int) VignetteMode.Masked) {
                EditorGUILayout.PropertyField(m_Center);
                EditorGUILayout.PropertyField(m_Intensity);
                EditorGUILayout.PropertyField(m_Smoothness);
                EditorGUILayout.PropertyField(m_Roundness);
                EditorGUILayout.PropertyField(m_Rounded);
            }
            else {
                var mask = (target as VignetteModel).settings.mask;

                // Checks import settings on the mask, offers to fix them if invalid
                if (mask != null) {
                    var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(mask)) as TextureImporter;

                    if (importer != null) // Fails when using an internal texture
                    {
#if UNITY_5_5_OR_NEWER
                        var valid = importer.anisoLevel == 0
                                    && importer.mipmapEnabled == false
                                    //&& importer.alphaUsage == TextureImporterAlphaUsage.FromGrayScale
                                    && importer.alphaSource == TextureImporterAlphaSource.FromGrayScale
                                    && importer.textureCompression == TextureImporterCompression.Uncompressed
                                    && importer.wrapMode == TextureWrapMode.Clamp;
#else
                        bool valid = importer.anisoLevel == 0
                            && importer.mipmapEnabled == false
                            && importer.grayscaleToAlpha == true
                            && importer.textureFormat == TextureImporterFormat.Alpha8
                            && importer.wrapMode == TextureWrapMode.Clamp;
#endif

                        if (!valid) {
                            EditorGUILayout.HelpBox("Invalid mask import settings.", MessageType.Warning);

                            GUILayout.Space(-32);
                            using (new EditorGUILayout.HorizontalScope()) {
                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button("Fix", GUILayout.Width(60))) {
                                    SetMaskImportSettings(importer);
                                    AssetDatabase.Refresh();
                                }

                                GUILayout.Space(8);
                            }

                            GUILayout.Space(11);
                        }
                    }
                }

                EditorGUILayout.PropertyField(m_Mask);
                EditorGUILayout.PropertyField(m_Opacity);
            }
        }

        private void SetMaskImportSettings(TextureImporter importer) {
#if UNITY_5_5_OR_NEWER
            importer.textureType = TextureImporterType.SingleChannel;
            //importer.alphaUsage = TextureImporterAlphaUsage.FromGrayScale;
            importer.alphaSource = TextureImporterAlphaSource.FromGrayScale;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
#else
            importer.textureType = TextureImporterType.Advanced;
            importer.grayscaleToAlpha = true;
            importer.textureFormat = TextureImporterFormat.Alpha8;
#endif

            importer.anisoLevel = 0;
            importer.mipmapEnabled = false;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.SaveAndReimport();
        }
    }
}