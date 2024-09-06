using System.Collections;
using System.Collections.Generic;

public interface IDamageable
{
    void TakeDamage(float damage);
    void Die();
    bool isDead { get; set; }
    List<string> deathEffect { get; set; }
    string deathExplosion { get; set; }
    IEnumerator HandleDeath();
    IEnumerator DeathAnimation();
}