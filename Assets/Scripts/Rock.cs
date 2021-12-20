using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Enemy
{
    public float speed; 
    private void Update()
    {
        //gameObject.transform.Translate(new Vector3(0, speed * Time.deltaTime, 0)); 
        gameObject.transform.position = 
            new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y - speed * Time.deltaTime,
            gameObject.transform.position.z);   
    }
}
