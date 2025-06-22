using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MissionData))]
public class MissionDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        var missionData = (MissionData)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Set MissionPosition from Scene Transform", EditorStyles.boldLabel);

        var transform = (Transform)EditorGUILayout.ObjectField(
            new GUIContent("Scene Transform"), null, typeof(Transform), true);

        if (transform != null)
        {
            Undo.RecordObject(missionData, "Set Mission Position");
            var pos = transform.position;

            var posProp = serializedObject.FindProperty("<MissionPosition>k__BackingField");
            posProp.vector3Value = pos;

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(missionData);
        }

        serializedObject.ApplyModifiedProperties();
    }
}