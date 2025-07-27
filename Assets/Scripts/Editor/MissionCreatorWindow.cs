using System;
using System.Collections.Generic;
using System.Linq;
using Missions.MissionPrerequisite;
using UnityEditor;
using UnityEngine;

public class MissionCreatorWindow : EditorWindow
{
    private static MissionCreatorWindow _window;
    private readonly List<(MissionStep, Editor)> missionSteps = new();
    private readonly List<(MissionReward, Editor)> rewards = new();
    private (MissionPrerequisite, Editor) prerequisite = new ();
    private string missionName;
    private DefaultAsset exportFolder;

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
        
        EditorGUILayout.LabelField("Mission Prerequisite");
        if (prerequisite != default && prerequisite.Item1 != null)
        {
            if (GUILayout.Button("X"))
            {
                prerequisite = default;
            }
            else
                prerequisite.Item2.DrawDefaultInspector();
        }
        else
            EditorGUILayout.LabelField("No Mission Prerequisite");
        
        if (GUILayout.Button("Set Prerequisite"))
        {
            Common.ShowScriptableObjectDropdown("Prerequisites", Common.GetDerivedTypes<MissionPrerequisite>(), type =>
            {
                var prerequisiteInstance = CreateInstance(type);
                prerequisite = new(prerequisiteInstance as MissionPrerequisite, Editor.CreateEditor(prerequisiteInstance));
            });
        }
        GUILayout.Space(20);
        Common.DrawItemList("Mission Steps", missionSteps, DrawMissionStep, ref foldoutSteps);
        GUILayout.Space(20);
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

        GUILayout.Space(10);
        
        if(!IsInputValid())
            return;
        if (GUILayout.Button("Create Mission"))
        {
            CreateMission();
        }
        
    }

    private void CreateMission()
    {
        var missionData = CreateInstance<MissionData>();
        if (missionData != null)
        {
            missionData.MissionPrerequisite = prerequisite.Item1;
            AssetDatabase.AddObjectToAsset(prerequisite.Item1, missionData);
        }
        missionData.Rewards = rewards.Select(x =>
        {
            AssetDatabase.AddObjectToAsset(x.Item1, missionData);
            return x.Item1;
        }).ToArray();
        missionData.Steps = missionSteps.Select(x =>
        {
            AssetDatabase.AddObjectToAsset(x.Item1, missionData);
            return x.Item1;
        }).ToArray();
        
    }

    private bool IsInputValid()
    {
        if (exportFolder == null)
        {
            EditorGUILayout.HelpBox("You need to set the export folder", MessageType.Error);
            return false;
        }
        if(string.IsNullOrEmpty(missionName?.Trim()))
        {
            EditorGUILayout.HelpBox("Mission Name Cannot Be Empty", MessageType.Error);
            return false;
        }

        return true;
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
        exportFolder = EditorGUILayout.ObjectField("Export Folder",exportFolder, typeof(DefaultAsset), false) as DefaultAsset;
    }
}