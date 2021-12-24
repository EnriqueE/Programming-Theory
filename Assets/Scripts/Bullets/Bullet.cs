using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 10.0f;
    private float boundaryTop = 9.0f;
    private float boundaryBottom = -5.0f;
    private float boundaryHorizontal = 11.0f;
    public int damage = 1;
    public GameObject bulletParticleSystem;    
    public string parentName { set; get; } 

    public void Start()
    {
        if (bulletParticleSystem)
        {
            //bulletParticleSystem.transform.parent = GameObject.Find("Pool").transform;
            bulletParticleSystem.transform.parent = gameObject.transform.parent.transform;
            bulletParticleSystem.transform.position = Vector3.zero;
            bulletParticleSystem.transform.localScale = Vector3.one;
            bulletParticleSystem.transform.rotation = Quaternion.identity;
        }
    }
    public void Update()
    {

        // Bullet movement
        transform.position += transform.TransformDirection(Vector3.forward * speed  * Time.deltaTime);


        // Destroy if out of boundary
        if (transform.position.y > boundaryTop ||
            transform.position.x > boundaryHorizontal ||
            transform.position.x < -boundaryHorizontal ||
            transform.position.y < boundaryBottom)
            DestroyBullet();


    }
    public void SetBulletSpeed(float newSpeed)
    {
        if(newSpeed>0)
        {
            speed = newSpeed;
        }
        
    }
    public void SetBulletDamage(int newDamage)
    {
        if(damage > 0)
        {
            damage = newDamage;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.transform.root.name;
        if (parentName == "Player" && otherName != "Player" && other.GetComponentInParent<Enemy>())
        {           
            other.GetComponentInParent<Enemy>().Hit(this);
            Collision(other);            
        }
        if(parentName != "Player" && otherName == "Player" && other.transform.root.GetComponent<PlayerController>()) 
        {
            other.transform.root.GetComponent<PlayerController>().Hit(this);
            Collision(other);
        }
    }
    public virtual void Collision(Collider other)
    {
        
        if (bulletParticleSystem)
        {
            bulletParticleSystem.SetActive(true);
            bulletParticleSystem.transform.position = other.ClosestPoint(transform.position);
            bulletParticleSystem.GetComponent<ParticleSystem>().Play();
            bulletParticleSystem.transform.LookAt(transform.position);
        }
        DestroyBullet();
       
    }
    private void TryToDestroyPool()
    {
        if (transform.parent.GetComponent<PoolController>() && transform.parent.GetComponent<PoolController>().destroyPending)
        {
            bool anyChildActive = false;
            foreach (Transform child in transform.parent.gameObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    anyChildActive = true;
                }
            }
            if (!anyChildActive)
            {
                transform.parent.GetComponent<PoolController>().DestroyPool();
            }
        }
    }
    public virtual void DestroyBullet()
    {
        gameObject.SetActive(false);
        TryToDestroyPool();
        
    }
}
