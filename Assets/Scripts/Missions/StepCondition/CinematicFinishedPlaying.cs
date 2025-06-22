using System;
using UnityEngine;
using Utils;

[CreateAssetMenu(menuName = "Mission/Step Condition/Cinematic Finished Playing")]
public class CinematicFinishedPlaying : StepCondition
{
    private double duration;
    private double time;

    public override void OnStart(Action stepCompleted, Action<string> stepFailed)
    {
        base.OnStart(stepCompleted, stepFailed);
        time = Time.time;
        duration = MissionCinematicUIController.Instance.PlayableDirector.playableAsset.duration;
    }

    public override void OnUpdate()
    {
        if (Time.time - time >= duration)
            InvokeAppropriateStepEvent();
    }
}