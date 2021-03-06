using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    private float boundaryTop = 8.0f;
    private float boundaryBottom = -5.0f;
    private float boundaryHorizontal = 11.0f;    
    public GameObject bulletParticleSystem;
    public AudioClip impactClip;
    public string parentName { set; get; } 

    public void Start()
    {
        if (bulletParticleSystem)
        {
            bulletParticleSystem.transform.parent = gameObject.transform.parent.transform;
            bulletParticleSystem.transform.position = Vector3.zero;
            bulletParticleSystem.transform.localScale = Vector3.one;
            bulletParticleSystem.transform.rotation = Quaternion.identity;
        }
    }
    public void Update()
    {

        // Bullet movement
        transform.position += transform.up * speed * Time.deltaTime; 

        // Destroy if out of boundary
        if (transform.position.y > boundaryTop ||
            transform.position.x > boundaryHorizontal ||
            transform.position.x < -boundaryHorizontal ||
            transform.position.y < boundaryBottom)
        {
        
            DestroyBullet();

        }


    }
    public void SetSpeed(float newSpeed)
    {
        if(newSpeed>0)
        {
            speed = newSpeed;
        }
        
    }
    public void SetDamage(int newDamage)
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
            other.GetComponentInParent<Enemy>().Hit(damage);
            Collision(other);            
        }
        if(parentName != "Player" && otherName == "Player" && other.transform.root.GetComponent<PlayerController>()) 
        {
            other.transform.root.GetComponent<PlayerController>().Hit(damage);
            Collision(other);
        }
    }
    public virtual void Collision(Collider other)
    {
        AudioController.instance.Play(impactClip);
        if (bulletParticleSystem)
        {
            bulletParticleSystem.SetActive(true);
            bulletParticleSystem.transform.position = other.ClosestPoint(transform.position);
            bulletParticleSystem.GetComponent<ParticleSystem>().Play();
            bulletParticleSystem.transform.LookAt(transform.position);
        }
        DestroyBullet();
       
    }
    private protected void TryToDestroyPool()
    {
        PoolController poolController = transform.parent.GetComponent<PoolController>(); 
        if (poolController && poolController.isPendingDestroy())
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
                poolController.DestroyPool();
            }
        }
    }
    public virtual void DestroyBullet()
    {
        gameObject.SetActive(false);
        TryToDestroyPool();
        
    }
}
