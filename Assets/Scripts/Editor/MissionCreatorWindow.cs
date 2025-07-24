using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class MissionCreatorWindow : EditorWindow
{
    private static MissionCreatorWindow _window;

    private string _missionName;
    private readonly List<(MissionStep, Editor)> _missionSteps = new();

    private List<(Type type, string displayName)> _missionStepTypes;
    private readonly List<(MissionReward, Editor)> _rewards = new();
    private List<(Type type, string displayName)> _rewardTypes;

    private Vector2 scrollPosition;

    private void OnEnable()
    {
        _rewardTypes = GetDerivedTypes<MissionReward>();
        _missionStepTypes = GetDerivedTypes<MissionStep>();
    }
    
    
    [MenuItem("Tools/Mission Creator")]
    public static void ShowWindow()
    {
        if (_window == null)
        {
            _window = GetWindow<MissionCreatorWindow>();
            _window.titleContent = new GUIContent("Mission Creator");
        }
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        DrawMissionHeader();
        EditorGUILayout.Space(10);

        DrawItemList("Mission Steps", _missionSteps, DrawMissionStep,
            () =>
            {
                var missionStep = CreateInstance<MissionStep>();
                _missionSteps.Add((missionStep, Editor.CreateEditor(missionStep)));
            });

        EditorGUILayout.Space(10);

        DrawItemList("Rewards", _rewards, i => { EditorGUILayout.LabelField(_rewards[i].Item1.name); },
            () =>
            {
                ShowScriptableObjectDropdown("Select Reward Type", _rewardTypes, type =>
                {
                    var reward = CreateInstance(type) as MissionReward;
                    if (reward != null)
                    {
                        reward.name = type.Name;
                        _rewards.Add((reward, Editor.CreateEditor(reward)));
                    }
                });
            });

        EditorGUILayout.EndScrollView();
    }

    private void DrawMissionStep(int index)
    {
        var step = _missionSteps[index].Item1;
        if (step == null) return;

        if (step.name != step.StepText)
        {
            step.name = step.StepText;
            EditorUtility.SetDirty(step);
        }
    }



    private void DrawMissionHeader()
    {
        GUILayout.Label("Mission Creator", EditorStyles.boldLabel);
        _missionName = EditorGUILayout.TextField("Mission Name", _missionName);
    }

    private void DrawItemList<T>(string title, List<(T Item1, Editor Item2)> list, Action<int> drawItem, Action onAdd)
        where T : ScriptableObject
    {
        GUILayout.Label(title, EditorStyles.boldLabel);

        int? removeIndex = null;

        for (var i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("X", GUILayout.Width(20))) removeIndex = i;
            EditorGUILayout.EndHorizontal();
            
            var editor = list[i].Item2;
            Editor.DrawFoldoutInspector(list[i].Item1, ref editor);

            drawItem?.Invoke(i);

            EditorGUILayout.EndVertical();
        }

        if (removeIndex.HasValue)
        {
            var toRemove = list[removeIndex.Value];
            if (toRemove.Item2 != null) DestroyImmediate(toRemove.Item2);
            list.RemoveAt(removeIndex.Value);
        }

        if (GUILayout.Button($"Add {title.TrimEnd('s')}")) onAdd?.Invoke();
    }


    private void ShowScriptableObjectDropdown(string title, List<(Type, string)> types, Action<Type> onSelected)
    {
        var dropdown = new ScriptableObjectTypeDropdown(title, new AdvancedDropdownState(), types, onSelected);
        var rect = new Rect(Event.current.mousePosition, Vector2.zero);
        dropdown.Show(rect);
    }

    private List<(Type, string)> GetDerivedTypes<T>() where T : ScriptableObject
    {
        return Assembly.GetAssembly(typeof(T))
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsClass && typeof(T).IsAssignableFrom(t))
            .Select(t => (t, RegexReplacePascal(t.Name)))
            .ToList();
    }

    private string RegexReplacePascal(string input)
    {
        return string.Join(" ", Regex.Split(input, @"(?<!^)(?=[A-Z])"));
    }
}

internal class ScriptableObjectTypeDropdown : AdvancedDropdown
{
    private readonly string _dropdownTitle;
    private readonly Action<Type> _onSelected;
    private readonly List<(Type type, string displayName)> _types;

    public ScriptableObjectTypeDropdown(string dropdownTitle, AdvancedDropdownState state,
        List<(Type, string)> types, Action<Type> onSelected)
        : base(state)
    {
        _dropdownTitle = dropdownTitle;
        _onSelected = onSelected;
        _types = types;
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem(_dropdownTitle);
        foreach (var (type, name) in _types) root.AddChild(new ScriptableObjectDropdownItem(name, type));

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        if (item is ScriptableObjectDropdownItem dropdownItem) _onSelected?.Invoke(dropdownItem.Type);
    }

    private class ScriptableObjectDropdownItem : AdvancedDropdownItem
    {
        public ScriptableObjectDropdownItem(string name, Type type) : base(name)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}