using System;

public abstract class EventBasedStepCondition<T> : StepCondition where T : IEvent
{
    public override void OnStart(Action stepCompleted, Action<string> stepFailed)
    {
        base.OnStart(stepCompleted, stepFailed);
        EventSystem<T>.Subscribe(StepEventInvoked);
    }

    private void StepEventInvoked(T obj)
    {
        if (EventConditionSatisfied(obj))
            InvokeAppropriateStepEvent();
    }

    public override void OnStop()
    {
        EventSystem<T>.Unsubscribe(StepEventInvoked);
    }

    protected abstract bool EventConditionSatisfied(T obj);
}