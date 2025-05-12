using UnityEngine;


public class DamageableDestroyedCondition : EventBasedStepCondition<OnDamageableDestroyed>
{
    [SerializeField] private string tag;
    protected override bool EventConditionSatisfied(OnDamageableDestroyed obj)
    {
        return obj.DamageableDestroyed.CompareTag(tag);

    }
}
