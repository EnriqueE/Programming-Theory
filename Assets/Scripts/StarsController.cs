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
        public float offsetHeight; 
    }
    private void Start()
    {
        foreach(Star star in stars)
        {
            ParticleSystem starPS = Instantiate(star.starPS, transform);
            starPS.startSize = star.size;
            
            starPS.GetComponent<StarsMovementController>().StartMove(star.speed, star.offsetHeight);
            starPS.Play();

            ParticleSystem starPSB = Instantiate(star.starPS, transform);
            starPSB.startSize = star.size *2;
            starPSB.gameObject.transform.position = new Vector3(
                starPSB.gameObject.transform.position.x,
                starPSB.gameObject.transform.position.y - star.offsetHeight,
                starPSB.gameObject.transform.position.z);
            starPSB.GetComponent<StarsMovementController>().StartMove(star.speed, star.offsetHeight*2);
            starPSB.Play();
        }
    }
}
