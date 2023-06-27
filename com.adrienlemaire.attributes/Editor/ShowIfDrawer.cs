using UnityEditor;
using UnityEngine;

namespace Com.AdrienLemaire.Attributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (IsAttributeShown(property))
                EditorGUI.PropertyField(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsAttributeShown(property))
                return EditorGUI.GetPropertyHeight(property, label);
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }

        private bool IsAttributeShown(SerializedProperty property)
        {
            ShowIfAttribute _attribute = attribute as ShowIfAttribute;
            SerializedProperty rule = property.serializedObject.FindProperty(_attribute.fieldName);

            switch (rule.propertyType)
            {

                case SerializedPropertyType.Boolean:
                    return (bool)_attribute.fieldValue == rule.boolValue;
                case SerializedPropertyType.String:
                    return (string)_attribute.fieldValue == rule.stringValue;
                case SerializedPropertyType.Integer:
                    return (int)_attribute.fieldValue == rule.intValue;
                default:
                    Debug.LogError("Error on the usage of the attribute \"ShowOn\".");
                    return false;
            }
        }
    }
}
