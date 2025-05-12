
    public abstract class EventBasedStepCondition<T> : StepCondition where T:IEvent
    {
        public override void StartTracking(MissionStep missionStep, bool isFailStep = false)
        {
            base.StartTracking(missionStep, isFailStep);
            EventSystem<T>.Subscribe(StepEventInvoked);
        }

        private void StepEventInvoked(T obj)
        {
            if(EventConditionSatisfied(obj))
                InvokeStepCompleted();
        }

        public override void StopTracking(MissionStep missionStep)
        {
            EventSystem<T>.Unsubscribe(StepEventInvoked);
        }

        protected abstract bool EventConditionSatisfied(T obj);
    }
