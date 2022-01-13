using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsFollowPlayer : MonoBehaviour
{
    
    void Start()
    {
        FollowTarget[] followTargets = GetComponentsInChildren<FollowTarget>();
        foreach(FollowTarget followTarget in followTargets)
        {
            followTarget.target = GameObject.Find("Player"); 
        }
    }

    
}
