using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Steps/Step Condition/Damageable Destroyed")]
public class DamageableDestroyedCondition : EventBasedStepCondition<OnDamageableDestroyed>
{
    [SerializeField] private string tag;
    protected override bool EventConditionSatisfied(OnDamageableDestroyed obj)
    {
        return obj.DamageableDestroyed.CompareTag(tag);
    }
}
