using System;
using Missions.MissionPrerequisite;
using UnityEngine;


public enum MissionAvailabilityStatus
{
    Unavailable,
    Available,
    InProgress,
    Completed
}

[CreateAssetMenu(fileName = "Mission", menuName = "Mission/Mission Data")]
public class MissionData : ScriptableObject
{
    
    [field: SerializeField] public string MissionId { get; private set; } = Guid.NewGuid().ToString();
    [field: SerializeField] public string MissionName { get; private set; }
    [field: SerializeField] public Sprite MissionIcon { get; private set; }
    [field: SerializeField] public Vector3 MissionPosition { get; private set; }

    [field: SerializeField] public MissionPrerequisite MissionPrerequisite { get; set; }
    [field: SerializeField] public MissionStep[] Steps { get;  set; }
    [field: SerializeField] public MissionReward[] Rewards { get; set; }
    [field: SerializeField] public MissionAvailabilityStatus CurrentStatus
    {
        get => currentStatus;
        set
        {
            if(currentStatus == value)
                return;
            
            EventSystem<OnMissionStatusChanged>.Invoke(new OnMissionStatusChanged { Mission = this, PreviousStatus = currentStatus});
            
            currentStatus = value;
            
            if(currentStatus == MissionAvailabilityStatus.Completed)
                EventSystem<OnMissionCompleted>.Invoke(new OnMissionCompleted { Mission = this });
        }
        
    }

    private MissionAvailabilityStatus currentStatus;
    private int currentStep;

    private bool isAvailable;

    public void StartMission()
    {
        currentStep = 0;
        Steps[currentStep].StepStarted(this, OnStepCompleted, OnStepFailed);
    }

    private void OnStepFailed(string stepFailedReason)
    {
        Steps[currentStep].StepFinished(this);
        EventSystem<OnMissionFailed>.Invoke(new OnMissionFailed
        {
            Mission = this,
            Reason = stepFailedReason
        });
    }

    public void OnUpdate()
    {
        if(CurrentStatus != MissionAvailabilityStatus.InProgress)
            return;
        Steps[currentStep].OnUpdate();
    }

    private void OnStepCompleted()
    {
        Steps[currentStep].StepFinished(this);
        currentStep++;

        if (currentStep >= Steps.Length)
        {
            CurrentStatus = MissionAvailabilityStatus.Completed;
            
            foreach (var missionReward in Rewards)
                missionReward.ClaimReward();
        }
        else
        {
            Steps[currentStep].StepStarted(this, OnStepCompleted, OnStepFailed);
        }
    }

    public void Cleanup()
    {
        foreach (var step in Steps)
        {
            step.Cleanup();
        }
    }
}