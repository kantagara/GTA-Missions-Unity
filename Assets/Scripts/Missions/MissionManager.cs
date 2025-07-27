using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    private const string MissionsKey = "Missions";

    [SerializeField] private List<MissionData> missionData = new();
    private HashSet<string> availableMissions = new();
    private MissionData currentMission;
    private Dictionary<string, MissionData> missionDataDict = new();


    private void Start()
    {
        missionDataDict = missionData.ToDictionary(x => x.MissionId, x => x);

        availableMissions = GetAvailableMissions();
        foreach (var missionId in availableMissions)
            missionDataDict[missionId].CurrentStatus = MissionAvailabilityStatus.Available;
        EventSystem<OnMissionStatusChanged>.Subscribe(MissionStatusChanged);
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
        EventSystem<OnMissionStatusChanged>.Unsubscribe(MissionStatusChanged);
        EventSystem<OnMissionStarting>.Unsubscribe(OnPlayerStartingMission);
        EventSystem<OnMissionCompleted>.Unsubscribe(MissionCompleted);

        foreach (var mission in missionData)
        {
            mission.CurrentStatus = MissionAvailabilityStatus.Unavailable;
            mission.Cleanup();
        }
    }

    private void MissionCompleted(OnMissionCompleted obj)
    {
        availableMissions.Remove(obj.Mission.MissionId);
    }

    private void OnPlayerStartingMission(OnMissionStarting obj)
    {
        currentMission = obj.Mission;
        currentMission.StartMission();
    }

    private void MissionStatusChanged(OnMissionStatusChanged obj)
    {
        if (obj.PreviousStatus == MissionAvailabilityStatus.Unavailable &&
            obj.Mission.CurrentStatus == MissionAvailabilityStatus.Available)
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