using UnityEngine;

public abstract class DamageableAction : ScriptableObject
{
    public abstract void OnDamageableHealthReachedZero(Damageable damageable);

    public abstract void OnDamageableHit(Damageable damageable);

    public virtual void OnDamageableHealed(Damageable damageable) { }
}
