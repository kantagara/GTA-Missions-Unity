using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Steps/Step Condition/Damageable Destroyed")]
public class DamageableDestroyedCondition : EventBasedStepCondition<OnDamageableDestroyed>
{
    [SerializeField] private int amount;
    [SerializeField] private string tag;
    
    private int currentAmount;

    public override void OnStart(Action stepCompleted, Action<string> stepFailed)
    {
        base.OnStart(stepCompleted, stepFailed);
        currentAmount = 0;
    }

    protected override bool EventConditionSatisfied(OnDamageableDestroyed obj)
    {
        if (obj.DamageableDestroyed.CompareTag(tag))
            currentAmount++;
        return currentAmount >= amount;
    }
}
