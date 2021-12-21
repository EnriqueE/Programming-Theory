using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWithPS : Bullet
{
    public GameObject bulletParticleSystem;
    public void Start()
    {
        if (bulletParticleSystem)
        {
            bulletParticleSystem.transform.parent = GameObject.Find("Pool").transform;
            bulletParticleSystem.transform.position = Vector3.zero;
            bulletParticleSystem.transform.localScale = Vector3.one;
            bulletParticleSystem.transform.rotation = Quaternion.identity;
        }
    }
    public override void Collision(Collider other)
    {
        if (bulletParticleSystem)
        {
            bulletParticleSystem.SetActive(true);
            bulletParticleSystem.transform.position = other.ClosestPoint(transform.position);
            bulletParticleSystem.GetComponent<ParticleSystem>().Play();
            bulletParticleSystem.transform.LookAt(transform.position);
        }
        base.Collision(other);
    }
}
