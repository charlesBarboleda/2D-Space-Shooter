using UnityEngine;
public interface IPickable
{
    void MoveTowardsPlayer();
    void OnPickUp();
    bool isAttracted { get; set; }
    float maxSpeed { get; set; }


}
