using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int laserStock = 0;
    [SerializeField] private GameObject laserPrefab;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaserLogic();
        }

    }
    private void LaserLogic()
    {
        if (laserStock > 0)
        {
            FireLaser();
        }
    }
    private void FireLaser()
    {

        laserStock--;
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.transform.rotation = transform.rotation;
        laser.transform.SetParent(transform);
        Destroy(laser, 5.0f);
        CameraShaker.Instance.ShakeOnce(6f, 6f, 0.1f, 15f);

    }

    public void IncreaseStock(int amount)
    {
        laserStock += amount;
    }
}
