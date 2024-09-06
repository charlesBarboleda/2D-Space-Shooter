using UnityEngine;
public interface IPickable
{
    void OnTriggerEnter2D(Collider2D other);
    void MoveTowardsPlayer();
    bool isAttracted { get; set; }
    float maxSpeed { get; set; }


}
