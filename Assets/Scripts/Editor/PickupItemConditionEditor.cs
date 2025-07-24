using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(PickupItemCondition))]
public class PickupItemConditionEditor : Editor
{
    private SerializedProperty itemTag;
    private SerializedProperty amount;

    private AdvancedDropdownState dropdownState;

    private void OnEnable()
    {
        itemTag = serializedObject.FindProperty("itemTag");
        amount = serializedObject.FindProperty("amount");
        dropdownState = new AdvancedDropdownState();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Item Tag");
        if (GUILayout.Button(string.IsNullOrEmpty(itemTag.stringValue) ? "Select Tag..." : itemTag.stringValue))
        {
            var dropdown = new SearchableTagDropdown(dropdownState, selected =>
            {
                itemTag.stringValue = selected;
                serializedObject.ApplyModifiedProperties();
            });

            dropdown.Show(new Rect(Event.current.mousePosition, Vector2.zero));
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(amount);

        serializedObject.ApplyModifiedProperties();
    }
}