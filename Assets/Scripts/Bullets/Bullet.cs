using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    private float boundaryTop = 9.0f;
    private float boundaryBottom = -5.0f;
    private float boundaryHorizontal = 11.0f;

    public int damage = 1;
    public void Update()
    {

        // Bullet movement
        /*gameObject.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + (speed * Time.deltaTime),
            transform.position.z);*/
        transform.position += transform.TransformDirection(Vector3.forward * speed  * Time.deltaTime);

        /*gameObject.transform.position = transform.rotation.eulerAngles * speed * Time.deltaTime; */

        // Destroy if out of boundary
        if (transform.position.y > boundaryTop ||
            transform.position.x > boundaryHorizontal ||
            transform.position.x < -boundaryHorizontal ||
            transform.position.y < boundaryBottom)
            DestroyBullet();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Enemy>())
        {
            other.GetComponentInParent<Enemy>().Hit(this);
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
