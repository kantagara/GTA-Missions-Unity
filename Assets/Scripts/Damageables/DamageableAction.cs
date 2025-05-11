using UnityEngine;

public abstract class DamageableAction : ScriptableObject
{
    public abstract void OnDamageableDestroyed(Damageable damageable);

    public abstract void OnDamageableHit(Damageable damageable);

    public virtual void OnDamageableHealed(Damageable damageable) { }
}
