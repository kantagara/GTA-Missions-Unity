using System;
using Missions;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission Step", menuName = "Mission/Steps/Step")]
public class MissionStep : ScriptableObject
{
    [field: SerializeField] public string StepText { get; private set; }

    [SerializeField] private StepCondition[] successfulSteps;
    [SerializeField] private StepCondition[] failedSteps;
    [SerializeField] private StepLifecycleEvent[] stepStarted;
    [SerializeField] private StepLifecycleEvent[] stepFinished;

    
    public void StepStarted(MissionData mission, Action onStepCompleted, Action<string> onStepFailed)
    {
        foreach (var lifecycleEvent in stepStarted)
        {
            lifecycleEvent.Invoke(mission);
        }
        
        EventSystem<OnStepStarted>.Invoke(new OnStepStarted(){Step = this});
        
        foreach (var stepCondition in successfulSteps) stepCondition.OnStart(onStepCompleted, null);

        foreach (var stepCondition in failedSteps) stepCondition.OnStart(null, onStepFailed);
    }

    public void OnUpdate()
    {
        foreach (var stepCondition in successfulSteps) stepCondition.OnUpdate();

        foreach (var stepCondition in failedSteps) stepCondition.OnUpdate();
    }


    public void StepFinished(MissionData mission)
    {
        foreach (var lifecycleEvent in stepFinished)
        {
            lifecycleEvent.Invoke(mission);
        }
        EventSystem<OnStepFinished>.Invoke(new OnStepFinished(){Step = this});

        foreach (var stepCondition in successfulSteps) stepCondition.OnStop();

        foreach (var stepCondition in failedSteps) stepCondition.OnStop();
    }

    public void Cleanup()
    {
        foreach (var stepCondition in successfulSteps) stepCondition.OnStop();

        foreach (var stepCondition in failedSteps) stepCondition.OnStop();
    }
}