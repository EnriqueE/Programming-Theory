using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Enemy rock1;
    public float boundaryHorizontal = 9.0f;     
    public float rock1Rate = 3.0f;
    public int quantity = 1; 
    private float lastRockTime = 0.0f;
    public float spawnPositionY = 8.0f;
    private int quantitySpawned = 0; 


    private void Start()
    {
        
    }
    private void Update()
    {

        if(Time.time - lastRockTime >= rock1Rate && quantitySpawned < quantity)
        {
            quantitySpawned++; 
            Enemy rockInstance = Instantiate(rock1);
            rockInstance.transform.position = new Vector3(
                Random.Range(-boundaryHorizontal, boundaryHorizontal),spawnPositionY,0);
            lastRockTime = Time.time;
        }
    }


}
