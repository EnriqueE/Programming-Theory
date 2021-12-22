using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteDown : MonoBehaviour
{

    public float speed = 0.0f;
    public bool destroyWhenReachBottomBoundary = true;
    private float boundaryBottom = -6.0f;


    void Update()
    {
        if (speed != 0)
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;
            if(transform.position.y <= boundaryBottom && destroyWhenReachBottomBoundary)
            {
                if(gameObject.GetComponent<Enemy>())
                {
                    gameObject.GetComponent<Enemy>().SilentDeath(); 
                }
            } 
        }
    }
}
