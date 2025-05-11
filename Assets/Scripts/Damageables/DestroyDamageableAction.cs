using UnityEngine;

[CreateAssetMenu(fileName = "Destroy Damageable", menuName = "Damageable/Destroy Damageable", order = 1)]
public class DestroyDamageableAction :DamageableAction
{
    public override void OnDamageableDestroyed(Damageable damageable)
    {
        Destroy(damageable.gameObject);
    }

    public override void OnDamageableHit(Damageable damageable)
    {
    }

    public override void OnDamageableHealed(Damageable damageable)
    {
    }
}
