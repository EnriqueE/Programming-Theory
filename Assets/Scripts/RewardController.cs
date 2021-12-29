using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public bool isEnabled = true;
    public Vector2 speed = new Vector2(1f,0.1f);    
    public float horiztonalBoundary = 9.0f;
    public float bottomBoundary = -5.0f;
    public float timeAlive = 10.0f;
    public ParticleSystem collectParticleSystem;
    public float delayToDisapearWhenCollect = 0.4f;
    public float disapearingTimeWhenCollect = 0.2f; 


    private float initTime;
    private Vector3 vectorMovement;



    void Start()
    {
        
        vectorMovement = Random.value > 0.5f ? Vector3.right : Vector3.left;
        initTime = Time.time; 

    }
    private void OnTriggerEnter(Collider other)
    {
        string name = other.transform.root.name; 
        if(name == "Player")

        {
            Collect(); 
        }
    }
    public void Collect()
    {
        collectParticleSystem.Play();
        Debug.Log("Collect"); 
    }
    private void Death()
    {
        Destroy(gameObject); 
    }
    void Update()
    {
        if(Time.time - initTime > timeAlive)
        {
            Death(); 
        }
        if(isEnabled)
        {
            transform.Translate(vectorMovement * speed.x * Time.deltaTime);
            transform.Translate(Vector3.down * speed.y * Time.deltaTime);
            if(transform.position.x > horiztonalBoundary)
            {
                vectorMovement =  Vector3.left;
                transform.position = new Vector3(horiztonalBoundary, transform.position.y, 0); 
            } else if(transform.position.x < -horiztonalBoundary)
            {
                vectorMovement = Vector3.right;
                transform.position = new Vector3(-horiztonalBoundary, transform.position.y, 0);
            }
        }
        
    }
}
