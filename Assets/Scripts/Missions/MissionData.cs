using UnityEngine;
using UnityEngine.Playables;

public class MissionData : ScriptableObject
{
    [field:SerializeField] public string MissionName { get; private set; }
    [field:SerializeField] public PlayableAsset PlayableAsset { get; private set; }
    [field:SerializeField] public MissionStep[] Steps { get; private set; }
    [field:SerializeField] public MissionReward[] Rewards { get; private set; }
    
    private int _currentStep = 0;
    
    public void StartMission()
    {
        _currentStep = 0;
        Steps[_currentStep].OnStepCompleted += OnStepCompleted;
        Steps[_currentStep].OnStepFailed += OnStepFailed;
        Steps[_currentStep].StepStarted();
    }

    private void OnStepFailed(string stepFailedReason)
    {
        EventSystem<OnMissionFailed>.Invoke(new OnMissionFailed()
        {
            Mission = this,
            Reason = stepFailedReason
        });
    }

    public void OnUpdate()
    {
        Steps[_currentStep].OnUpdate();
    }

    private void OnStepCompleted()
    {
        Steps[_currentStep].OnStepCompleted -= OnStepCompleted;
        Steps[_currentStep].OnStepFailed -= OnStepFailed;

        _currentStep++;
        if (_currentStep >= Steps.Length)
        {
            foreach (var missionReward in Rewards)
            {
                missionReward.ClaimReward();
            }
            EventSystem<OnMissionCompleted>.Invoke(new OnMissionCompleted {Mission = this});
        }
        else
        {
            Steps[_currentStep].OnStepCompleted += OnStepCompleted;
            Steps[_currentStep].OnStepFailed += OnStepFailed;
        }
    }
}
