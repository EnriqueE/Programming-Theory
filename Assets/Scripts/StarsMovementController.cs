using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsMovementController : MonoBehaviour
{
    private float speed; 
    private float offsetHeight;
    public float initHeight;
    private bool isMoving = false; 

    
    public void StartMove(float newSpeed, float m_offsetHeight)
    {
        initHeight = transform.position.y;
        speed = newSpeed;
        offsetHeight = m_offsetHeight;
        isMoving = true;
        Debug.Log("Jump at " + (initHeight - offsetHeight));
        
    }
    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            if (transform.position.y < initHeight - offsetHeight)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    initHeight + offsetHeight ,
                    transform.position.z);
                GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
