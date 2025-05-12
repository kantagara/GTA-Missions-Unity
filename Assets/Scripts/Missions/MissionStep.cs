using System;
using UnityEngine;
public class MissionStep : ScriptableObject
{
    [field:SerializeField] public string StepText { get; private set; }
    [field:SerializeField] private StepCondition[] SuccessfulSteps { get; set; }
    [field:SerializeField] private StepCondition[] FailedSteps { get; set; }
    
    public Action OnStepCompleted;
    public Action<string> OnStepFailed;

    public void OnUpdate()
    {
        foreach (var stepCondition in SuccessfulSteps)
        {
            stepCondition.OnUpdate();
        }

        foreach (var stepCondition in FailedSteps)
        {
            stepCondition.OnUpdate();
        }
    }

    public void StepStarted()
    {
        foreach (var stepCondition in SuccessfulSteps)
        {
            stepCondition.StartTracking(this, true);
        }

        foreach (var stepCondition in FailedSteps)
        {
            stepCondition.StartTracking(this, true);
        }
    }

    public void StepFinished()
    {
        foreach (var stepCondition in SuccessfulSteps)
        {
            stepCondition.StartTracking(this, true);
        }

        foreach (var stepCondition in FailedSteps)
        {
            stepCondition.StopTracking(this);
        }
    }
}
