using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public float boundaryTop = 9.0f;
    public int damage = 1;
    public void Update()
    {
        // Destroy if out of boundary
        gameObject.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + (speed * Time.deltaTime),
            transform.position.z);
        if (transform.position.y > boundaryTop)
            DestroyBullet();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().Hit(this);
            Collision(other);
        }
    }
    public virtual void Collision(Collider other)
    {
        DestroyBullet();
    }
    public virtual void DestroyBullet()
    {
        gameObject.SetActive(false);
    }
}
