using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsController : MonoBehaviour
{
    public float globalSpeed = 1.0f;
    [SerializeField] private Star[] stars;
    

    [Serializable]
    public struct Star
    {
        public ParticleSystem starPS;
        public float speed;
        public float size;
        public int quantity; 
        public float origin;
        public float destination;
        public float spawnPosition; 
    }
    private void Start()
    {
        foreach(Star star in stars)
        {

            InstantiateStar(star.starPS, star.size, star.speed * globalSpeed, star.quantity, star.origin, star.destination, star.spawnPosition);
            InstantiateStar(star.starPS, star.size, star.speed * globalSpeed, star.quantity, star.origin - star.destination, star.destination, star.spawnPosition);
           
        }
    }
    private void InstantiateStar(ParticleSystem prefab, float size, float speed,  int quantity, float origin, float destination, float spawnPosition)
    {
        ParticleSystem starPS = Instantiate(prefab, transform);
        starPS.startSize = size;
        starPS.GetComponent<StarsMovementController>().Initialize(speed, quantity, origin, destination, spawnPosition);
        starPS.Stop(); 
        starPS.Play();
    }

}
