using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewrdMovementController : MonoBehaviour
{
    public bool isEnabled = true;
    public Vector2 speed = new Vector2(0.2f,0.1f);
    private Vector3 vectorMovement;
    public float horiztonalBoundary = 9.0f;
    public float bottomBoundary = -5.0f;
    public float timeAlive = 10.0f;

    

    void Start()
    {
        
        vectorMovement = Random.value > 0.5f ? Vector3.right : Vector3.left; 


    }

    void Update()
    {
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
