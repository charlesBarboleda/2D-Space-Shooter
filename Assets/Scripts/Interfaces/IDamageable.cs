using System.Collections;

public interface IDamageable
{
    void TakeDamage(float damage);
    void Die();
    IEnumerator HandleDeath();
    IEnumerator DeathAnimation();
}