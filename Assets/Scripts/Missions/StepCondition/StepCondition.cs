using UnityEngine;


    public abstract class StepCondition : ScriptableObject
    {
        public string StepConditionText { get; private set; }
        private bool _isFailStep;
        private MissionStep missionStep;

        public virtual void StartTracking(MissionStep step, bool isFailStep = false)
        {
            this.missionStep = step;
            _isFailStep = isFailStep;
        }

        public void OnUpdate()
        {
            
        }

        protected void InvokeStepCompleted()
        {
            if(_isFailStep)
                missionStep.OnStepFailed?.Invoke(StepConditionText);
            else 
                missionStep.OnStepCompleted?.Invoke();
        }

        public virtual void StopTracking(MissionStep missionStep)
        {
            
        }
    }
