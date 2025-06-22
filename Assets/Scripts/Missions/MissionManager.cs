using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    private const string MissionsKey = "Missions";

    [SerializeField] private List<MissionData> missionData = new();
    private Dictionary<string, MissionData> missionDataDict = new();
    private MissionData currentMission;
    private HashSet<string> availableMissions = new();
    

    private void Start()
    {
        missionDataDict = missionData.ToDictionary(x => x.MissionId, x => x);

        availableMissions = GetAvailableMissions();
        foreach (var missionId in availableMissions)
        {
            missionDataDict[missionId].IsAvailable = true;
        }
        EventSystem<OnMissionBecameAvailable>.Subscribe(MissionBecameAvailable);
        EventSystem<OnMissionStarting>.Subscribe(OnPlayerStartingMission);
        EventSystem<OnMissionCompleted>.Unsubscribe(MissionCompleted);
    }

    private void Update()
    {
        currentMission?.OnUpdate();
    }

    private void OnDestroy()
    {
        SetAvailableMissions();
        EventSystem<OnMissionBecameAvailable>.Unsubscribe(MissionBecameAvailable);
        EventSystem<OnMissionStarting>.Unsubscribe(OnPlayerStartingMission);
        EventSystem<OnMissionCompleted>.Unsubscribe(MissionCompleted);
        
        foreach (var mission in missionData)
        {
            mission.IsAvailable = false;
            mission.Cleanup();
        }
    }

    private void MissionCompleted(OnMissionCompleted obj)
    {
        availableMissions.Remove(obj.Mission.MissionId);
        obj.Mission.IsAvailable = false;
    }

    private void OnPlayerStartingMission(OnMissionStarting obj)
    {
        currentMission = obj.Mission;
        currentMission.StartMission();
    }

    private void MissionBecameAvailable(OnMissionBecameAvailable obj)
    {
        availableMissions.Add(obj.Mission.MissionId);
    }

    private void SetAvailableMissions()
    {
        PlayerPrefs.SetString(MissionsKey, string.Join(",", availableMissions));
    }

    private HashSet<string> GetAvailableMissions()
    {
        var missions = PlayerPrefs.GetString(MissionsKey, "");
        availableMissions = string.IsNullOrEmpty(missions)
            ? new HashSet<string> { missionData[0].MissionId }
            : missions.Split(",").ToHashSet();
        return availableMissions;
    }
}