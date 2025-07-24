using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MissionStep))]
[CanEditMultipleObjects] // omogucava multi-edit ako zatreba
public class MissionStepEditor : Editor
{
    private bool drawDefault;

    private SerializedProperty stepText;
    private SerializedProperty successfulSteps;
    private SerializedProperty failStep;
    private SerializedProperty stepStarted;
    private SerializedProperty stepCompleted;
    private int? indexToRemove;

    private Dictionary<Object, Editor> dict = new ();
    
    private void OnEnable()
    {
        stepText = serializedObject.FindProperty("<StepText>k__BackingField");
        successfulSteps = serializedObject.FindProperty("successfulSteps");
        failStep = serializedObject.FindProperty("failedSteps");
        stepStarted = serializedObject.FindProperty("stepStarted");
        stepCompleted = serializedObject.FindProperty("stepFinished");
    }

    public override void OnInspectorGUI()
    {
        drawDefault = EditorGUILayout.Toggle("Draw Default Inspector", drawDefault);
        EditorGUILayout.Space();

        if (drawDefault)
        {
            DrawDefaultInspector();
            return;
        }

        serializedObject.Update();

        DrawStepText();
        DrawSuccessfulSteps();
        DrawFailedSteps();
        DrawStepStarted();
        DrawStepFinished();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStepText()
    {
        EditorGUI.BeginChangeCheck(); 
        EditorGUILayout.PropertyField(stepText);
        if (!EditorGUI.EndChangeCheck()) return;
        
        var step = (target as MissionStep)!;
        step.name = step.StepText;
        EditorUtility.SetDirty(step);
    }


    private void DrawSuccessfulSteps()
    {
        DrawList("Successful Steps", "Successful Step", successfulSteps, () =>
        {
            Common.ShowScriptableObjectDropdown("New Successful Step", Common.GetDerivedTypes<StepCondition>(), (type) =>
            {
                var condition = CreateInstance(type) as StepCondition;
                var element = successfulSteps.AddElementAndCreateEditor(condition);
                serializedObject.ApplyModifiedProperties();
                dict.Add(element.Item1.objectReferenceValue, element.Item2);
            });
        });
    }

    private void DrawList(string title, string elementName, SerializedProperty property, Action onAddClicked)
    {
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        for (int i = 0; i < property.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox); 
      
            if (GUILayout.Button("X", GUILayout.Width(20)))
                indexToRemove = i;
            
            SerializedProperty element = property.GetArrayElementAtIndex(i);
            EditorGUILayout.EndHorizontal();
            dict[element.objectReferenceValue].OnInspectorGUI();

        }

        EditorGUILayout.Space(4);
        if (GUILayout.Button($"Add {elementName}")) 
            onAddClicked?.Invoke();

        if (indexToRemove != null)
            property.DeleteArrayElementAtIndex(indexToRemove.Value);
        indexToRemove = null;

        EditorGUI.indentLevel--;
    }


    private void DrawFailedSteps() { }

    private void DrawStepStarted() { }

    private void DrawStepFinished() { }
}
