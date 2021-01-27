using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing {
    [PostProcessingModelEditor(typeof(FogModel))]
    public class FogModelEditor : PostProcessingModelEditor {
        private SerializedProperty m_ExcludeSkybox;

        public override void OnEnable() {
            m_ExcludeSkybox = FindSetting((FogModel.Settings x) => x.excludeSkybox);
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.HelpBox(
                "This effect adds fog compatibility to the deferred rendering path; enabling it with the forward rendering path won't have any effect. Actual fog settings should be set in the Lighting panel.",
                MessageType.Info);
            EditorGUILayout.PropertyField(m_ExcludeSkybox);
            EditorGUI.indentLevel--;
        }
    }
}