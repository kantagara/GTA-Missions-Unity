using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Steps/Step Condition/Pickup Item", order = int.MaxValue)]
public class PickupItemCondition : EventBasedStepCondition<OnItemPickedUp>
{
    [SerializeField] private string itemTag;
    [SerializeField] private int amount;

    private int currentAmount;
    
    protected override bool EventConditionSatisfied(OnItemPickedUp obj)
    {
        if (obj.ObjectPickedUp.CompareTag(itemTag))
            currentAmount++;
        return currentAmount >= amount;
    }

    public override void OnStart(Action stepCompleted, Action<string> stepFailed)
    {
        base.OnStart(stepCompleted, stepFailed);
        currentAmount++;
    }
}
