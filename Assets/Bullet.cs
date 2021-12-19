using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public float boundaryTop = 9.0f;
    public void Update()
    {
       // Debug.Log(speed * Time.deltaTime); 
        gameObject.transform.position = new Vector3(
            transform.position.x,
            transform.position.y  + (speed * Time.deltaTime),   
            transform.position.z);
        if(transform.position.y > boundaryTop)
        {
            gameObject.SetActive(false); 
        }
             
    }
    private void Start() 
    {
    }
}
 