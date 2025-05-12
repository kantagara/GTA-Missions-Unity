using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private DamageableAction damageableAction;

    private float _health;

    private void Awake()
    {
        _health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning($"Incorrect damage value has been passed {damage}");
            return;
        }

        _health -= damage;
        if (_health <= 0)
        {
            EventSystem<OnDamageableDestroyed>.Invoke(new OnDamageableDestroyed { DamageableDestroyed = gameObject });
            damageableAction?.OnDamageableDestroyed(this);
        }
        else
        {
            damageableAction?.OnDamageableHit(this);
        }
    }

    public void Heal(float heal)
    {
        if (heal < 0)
        {
            Debug.LogWarning($"Incorrect heal value has been passed {heal}");
            return;
        }

        _health += heal;
        _health = Mathf.Clamp(_health, 0, maxHealth);

        damageableAction?.OnDamageableHealed(this);
    }
}