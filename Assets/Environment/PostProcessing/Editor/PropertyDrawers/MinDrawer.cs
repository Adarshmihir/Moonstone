using UnityEngine;

namespace UnityEditor.PostProcessing {
    [CustomPropertyDrawer(typeof(MinAttribute))]
    internal sealed class MinDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var attribute = (MinAttribute) this.attribute;

            if (property.propertyType == SerializedPropertyType.Integer) {
                var v = EditorGUI.IntField(position, label, property.intValue);
                property.intValue = (int) Mathf.Max(v, attribute.min);
            }
            else if (property.propertyType == SerializedPropertyType.Float) {
                var v = EditorGUI.FloatField(position, label, property.floatValue);
                property.floatValue = Mathf.Max(v, attribute.min);
            }
            else {
                EditorGUI.LabelField(position, label.text, "Use Min with float or int.");
            }
        }
    }
}