using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWithPS : Bullet
{
    public GameObject particleSystem;
    public void Start()
    {
        
        particleSystem.transform.parent = GameObject.Find("Pool").transform;
        particleSystem.transform.position = Vector3.zero;
        particleSystem.transform.localScale = Vector3.one; 
        particleSystem.transform.rotation = Quaternion.identity;
    }
    public override void Collision(Collider other)
    {
        particleSystem.SetActive(true); 
        particleSystem.transform.position = other.ClosestPoint(transform.position);
        particleSystem.GetComponent<ParticleSystem>().Play(); 
        particleSystem.transform.LookAt(transform.position);    
        base.Collision(other);
    }
}
