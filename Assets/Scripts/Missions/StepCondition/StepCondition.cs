using System;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class StepCondition : ScriptableObject
{
    [SerializeField] private string failReasonText;
    private bool isTracking;
    private event Action<string> StepFailed;
    private event Action StepCompleted;

    public virtual void OnStart(Action stepCompleted, Action<string> stepFailed)
    {
        Assert.IsFalse(isTracking, "Already tracking this condition ");
        isTracking = true;
        StepCompleted = stepCompleted;
        StepFailed = stepFailed;
    }

    public virtual void OnUpdate()
    {
    }

    protected void InvokeAppropriateStepEvent()
    {
        StepCompleted?.Invoke();
        StepFailed?.Invoke(failReasonText);
    }

    public virtual void OnStop()
    {
        isTracking = false;
        StepCompleted = null;
        StepFailed = null;
    }
}