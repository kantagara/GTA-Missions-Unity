using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Mission/Mission Data")]
public class MissionData : ScriptableObject
{
    [field: SerializeField] public string MissionId { get; private set; } = Guid.NewGuid().ToString();
    [field: SerializeField] public string MissionName { get; private set; }
    [field: SerializeField] public Sprite MissionIcon { get; private set; }
    [field: SerializeField] public Vector3 MissionPosition { get; private set; }

    [field: SerializeField] public MissionStep[] Steps { get; private set; }
    [field: SerializeField] public MissionReward[] Rewards { get; private set; }

    private int currentStep;

    private bool isAvailable;

    public bool IsAvailable
    {
        get => isAvailable;
        set
        {
            if (isAvailable == value)
                return;
            isAvailable = value;

            if (isAvailable)
                EventSystem<OnMissionBecameAvailable>.Invoke(new OnMissionBecameAvailable { Mission = this });
        }
    }

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
        if(!IsAvailable)
            return;
        Steps[currentStep].OnUpdate();
    }

    private void OnStepCompleted()
    {
        Steps[currentStep].StepFinished(this);
        currentStep++;

        if (currentStep >= Steps.Length)
        {
            IsAvailable = false;
            EventSystem<OnMissionCompleted>.Invoke(new OnMissionCompleted { Mission = this });
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