using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon weapon;

    public void SetWeaponStats(float newfireRate, float newbulletSpeed, float newbulletDamage, int newAmountOfBullets, float newShootingAngle)
    {
        if (weapon != null)
        {
            weapon.SetStats(newfireRate, newbulletSpeed, newbulletDamage, newAmountOfBullets, newShootingAngle);
        }
    }
}
