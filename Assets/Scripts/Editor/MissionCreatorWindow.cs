using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissionCreatorWindow : EditorWindow
{
    private static MissionCreatorWindow _window;
    private readonly List<(MissionStep, Editor)> missionSteps = new();
    private readonly List<(MissionReward, Editor)> rewards = new();
    private string missionName;

    private List<(Type type, string displayName)> rewardTypes;
    private Vector2 scrollPosition;
    private bool foldoutSteps, foldoutRewards;

    private void OnEnable()
    {
        foldoutSteps = foldoutRewards = true;
        rewardTypes = Common.GetDerivedTypes<MissionReward>();
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        DrawMissionHeader();
        EditorGUILayout.Space(10);

        Common.DrawItemList("Mission Steps", missionSteps, DrawMissionStep, ref foldoutSteps);
        if (GUILayout.Button("Add Mission Step"))
        {
            var missionStep = CreateInstance<MissionStep>();
            missionSteps.Add((missionStep, Editor.CreateEditor(missionStep)));
        }

        EditorGUILayout.Space(10);


        Common.DrawItemList("Rewards", rewards, i => { EditorGUILayout.LabelField(rewards[i].Item1.name); }, ref foldoutRewards);
        if (GUILayout.Button("Add Reward"))
        {
            Common.ShowScriptableObjectDropdown("Select Reward Type", rewardTypes, type =>
            {
                var reward = CreateInstance(type) as MissionReward;
                reward!.name = type.Name;
                rewards.Add((reward, Editor.CreateEditor(reward)));
            });
        }

        EditorGUILayout.EndScrollView();
    }


    [MenuItem("Tools/Mission Creator")]
    public static void ShowWindow()
    {
        if (_window != null) return;
        _window = GetWindow<MissionCreatorWindow>();
        _window.titleContent = new GUIContent("Mission Creator");
    }

    private void DrawMissionStep(int index)
    {
        var step = missionSteps[index].Item1;
        if (step == null) return;

        if (step.name == step.StepText) return;
        step.name = step.StepText;
        EditorUtility.SetDirty(step);
    }


    private void DrawMissionHeader()
    {
        GUILayout.Label("Mission Creator", EditorStyles.boldLabel);
        missionName = EditorGUILayout.TextField("Mission Name", missionName);
    }
}