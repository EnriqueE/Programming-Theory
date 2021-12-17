using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsController : MonoBehaviour
{
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
            

            ParticleSystem starPS = Instantiate(star.starPS, transform);
            starPS.startSize = star.size;            
            starPS.GetComponent<StarsMovementController>().StartMove(star.speed, star.quantity, star.origin, star.destination, star.spawnPosition);
            starPS.Play();
            
            ParticleSystem starPSB = Instantiate(star.starPS, transform);            
            starPSB.startSize = star.size;           
            
            starPSB.GetComponent<StarsMovementController>().StartMove(star.speed, star.quantity, star.origin - star.destination, star.destination, star.spawnPosition);
            starPSB.Play();
        }
    }
}
