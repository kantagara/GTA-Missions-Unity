using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Steps/Step Condition/Pickup Item", order = int.MaxValue)]
public class PickupItemCondition : EventBasedStepCondition<OnItemPickedUp>
{
    [SerializeField] private string itemTag;
    
    protected override bool EventConditionSatisfied(OnItemPickedUp obj)
    {
        return obj.ObjectPickedUp.CompareTag(itemTag);
    }
}
