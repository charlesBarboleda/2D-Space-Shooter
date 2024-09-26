using UnityEngine;
public interface ITargetable
{
    Vector3 GetPosition();
    bool IsAlive();
    FactionType GetFactionType();
}
