using UnityEngine;

public class PickupItemCondition : EventBasedStepCondition<OnItemPickedUp>
{
    [SerializeField] private string itemTag;
    
    protected override bool EventConditionSatisfied(OnItemPickedUp obj)
    {
        return obj.ObjectPickedUp.CompareTag(itemTag);
    }
}
