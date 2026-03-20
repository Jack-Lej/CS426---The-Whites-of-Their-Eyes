using System;
using UnityEngine;

public class ShotgunPellet : Projectile
{
    DateTime spawnTimer;
    //Number of milliseconds before collision is detected after spawning, since all pellets spawn on the same spot
    [SerializeField] int spawnTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = DateTime.Now;
        spawnTimer.AddMilliseconds(spawnTime);
    }
    public void OnCollisionEnter(Collision collision)
    {
        //If the pellets collide with something other than each other on spawn, destroy it
        if(collision.gameObject.tag != "Shotgun Pellet")
            Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
