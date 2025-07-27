using System;
using System.Collections.Generic;
using Missions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MissionStep))]
[CanEditMultipleObjects]
public class MissionStepEditor : Editor
{
    private readonly Dictionary<Object, Editor> editorCache = new();
    private bool drawDefault;
    private int? indexToRemove;

    private MissionStepProperties properties;
    private StepListConfig[] listConfigs;

    private void OnEnable()
    {
        properties = new MissionStepProperties(serializedObject);
        listConfigs = new[]
        {
            new StepListConfig("Successful Steps", "Successful Step",
                properties.SuccessfulSteps, Common.GetDerivedTypes<StepCondition>),
            new StepListConfig("Failed Steps", "Failed Step",
                properties.FailStep, Common.GetDerivedTypes<StepCondition>),
            new StepListConfig("Step started", "Step started",
                properties.StepStarted, Common.GetDerivedTypes<StepLifecycleEvent>),
            new StepListConfig("Step finished", "Step finished",
                properties.StepCompleted, Common.GetDerivedTypes<StepLifecycleEvent>)
        };
    }

    public override void OnInspectorGUI()
    {
        DrawInspectorToggle();

        if (drawDefault)
        {
            DrawDefaultInspector();
            return;
        }

        serializedObject.Update();
        DrawCustomInspector();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawInspectorToggle()
    {
        drawDefault = EditorGUILayout.Toggle("Draw Default Inspector", drawDefault);
        EditorGUILayout.Space();
    }

    private void DrawCustomInspector()
    {
        DrawStepText();
        DrawStepLists();
    }

    private void DrawStepText()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(properties.StepText);

        if (EditorGUI.EndChangeCheck()) UpdateStepName();
    }

    private void UpdateStepName()
    {
        var step = (target as MissionStep)!;
        step.name = step.StepText;
        EditorUtility.SetDirty(step);
    }

    private void DrawStepLists()
    {
        foreach (var config in listConfigs)
        {
            DrawStepList(config);
        }
    }

    private void DrawStepList(StepListConfig config)
    {
        config.FoldOut = EditorGUILayout.Foldout(config.FoldOut, config.Title);
        if (!config.FoldOut)
            return;
        EditorGUI.indentLevel++;

        DrawListElements(config.Property);
        GUILayout.Space(20);
        DrawAddButton(config);
        HandleElementRemoval(config.Property);

        EditorGUI.indentLevel--;
    }

    private void DrawListElements(SerializedProperty property)
    {
        for (var i = 0; i < property.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.Width(20)))
                indexToRemove = i;
            
            var element = property.GetArrayElementAtIndex(i);
            EditorGUILayout.LabelField(element.objectReferenceValue.GetType().Name.RegexReplacePascal(), EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            if (editorCache.TryGetValue(element.objectReferenceValue, out var editor)) editor.OnInspectorGUI();
            
            EditorGUILayout.EndVertical();

        }
    }

    private void DrawAddButton(StepListConfig config)
    {
        EditorGUILayout.Space(4);
        if (GUILayout.Button($"Add {config.ElementName}")) ShowDropdownForType(config);
    }

    private void ShowDropdownForType(StepListConfig config)
    {
        Common.ShowScriptableObjectDropdown($"New {config.ElementName}",
            config.GetTypes(), type =>
            {
                var instance = CreateInstance(type);
                var element = config.Property.AddElementAndCreateEditor(instance);
                serializedObject.ApplyModifiedProperties();
                editorCache.Add(element.Item1.objectReferenceValue, element.Item2);
            });
    }

    private void HandleElementRemoval(SerializedProperty property)
    {
        if (indexToRemove != null)
        {
            property.DeleteArrayElementAtIndex(indexToRemove.Value);
            indexToRemove = null;
        }
    }
}

// Extracted property wrapper
public class MissionStepProperties
{
    public MissionStepProperties(SerializedObject serializedObject)
    {
        StepText = serializedObject.FindProperty("<StepText>k__BackingField");
        SuccessfulSteps = serializedObject.FindProperty("successfulSteps");
        FailStep = serializedObject.FindProperty("failedSteps");
        StepStarted = serializedObject.FindProperty("stepStarted");
        StepCompleted = serializedObject.FindProperty("stepFinished");
    }

    public SerializedProperty StepText { get; }
    public SerializedProperty SuccessfulSteps { get; }
    public SerializedProperty FailStep { get; }
    public SerializedProperty StepStarted { get; }
    public SerializedProperty StepCompleted { get; }
}

public class StepListConfig
{
    public StepListConfig(string title, string elementName,
        SerializedProperty property, Func<List<(Type, string)>> getTypes)
    {
        Title = title;
        ElementName = elementName;
        Property = property;
        GetTypes = getTypes;
    }

    public string Title { get; }
    public string ElementName { get; }
    public SerializedProperty Property { get; }
    public Func<List<(Type, string)>> GetTypes { get; }
    
    public bool FoldOut { get; set; }
}