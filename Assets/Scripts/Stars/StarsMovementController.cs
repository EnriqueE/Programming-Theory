using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsMovementController : MonoBehaviour
{
    public float speed;
    private float quantity; 
    private float origin;
    private float destination;
    private float spawnPosition; 
    private bool isMoving = false;
    private StarsController starsController; 

    
    public void Initialize(float newSpeed, int newQuantity, float newOrigin, float newDestination, float newSpawnPosition)
    {
        
        speed = newSpeed;
        quantity = newQuantity; 
        destination = newDestination;
        origin = newOrigin;
        spawnPosition = newSpawnPosition; 


        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule em = ps.emission;

        em.type = ParticleSystemEmissionType.Time;
        em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst (0, quantity), });

        transform.position = new Vector3(transform.position.x, origin, transform.position.z);
        isMoving = true;
        
    }
    private void Start()
    {
        starsController = GetComponentInParent<StarsController>();
    }
    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.back * speed * starsController.globalSpeed * Time.deltaTime);
            if (transform.position.y < destination)
            {
                transform.position = new Vector3(transform.position.x,spawnPosition,transform.position.z);
                GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
