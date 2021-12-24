using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public bool destroyPending = false;
    private float timeToDestroy = 4.0f; 

    public void DestroyPool()
    {
        Destroy(gameObject, timeToDestroy); 
    }



}
