using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Enemy
{
    public float speed;
    public float destroyWhenReachY = -5.0f;

    private void Update()
    { 
        //gameObject.transform.Translate(new Vector3(0, speed * Time.deltaTime, 0)); 
        gameObject.transform.position = 
            new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y - speed * Time.deltaTime,
            gameObject.transform.position.z);
        // Destroy on out of screen
        if (gameObject.transform.position.y <= destroyWhenReachY)
        {
            SilentDeath();
        }
    }
}
