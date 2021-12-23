using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotationSpeed = 0.3f;
    public float lifeTime = 5.0f;
    public float explosionRadius = 1.0f;
    public int damage = 50;
    public bool destroyOnBoundaries = false;
    public bool autoFire = false;


    private bool fired = false;
    public GameObject target;

    

    
    void Start()
    {
        if(autoFire)
        {
            Fire(); 
        }
    }
    
    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.up * speed * Time.deltaTime); 
        if(target)
        {

            //   Vector3 targetPosition = (target.transform.position - transform.position).normalized;


            //targetPosition.
           // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), Time.time * speed);


            //   Debug.Log(targetDirection); 
            //  transform.rotation = Quaternion.Euler(targetPosition); 
            //   Debug.DrawRay(transform.position, targetPosition, Color.green);


            // transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, Time.time * 0.01f);






           // transform.LookAt(target.transform, Vector3.forward);
            //transform.Rotate(new Vector3(90, 0, 0));





            /*
            Vector3 targetDir = target.transform.position - transform.position;
            

            targetDir.y += 0.5f;
            float step = rotationSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
            transform.position = new Vector3(transform.position.x,transform.position.y, 0);
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 90);
            
            */

            /*  targetPosition.z = transform.position.y; //set targetPos y equal to mine, so I only look at my own plane
              Debug.DrawRay(transform.position, targetPosition, Color.green);
              var targetDir = Quaternion.LookRotation(targetPosition - transform.position);
              transform.rotation = Quaternion.Slerp(transform.rotation, targetDir, rotationSpeed * Time.deltaTime);*/
        } else {
            FindTarget(); 
        }
    }
    public void FindTarget()
    {
        int maxHealt = 0;

        Transform parent = GameObject.Find("Enemies").transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            if(parent.GetChild(i).CompareTag("Enemy")) {
                Enemy enemy = parent.GetChild(i).GetComponent<Enemy>();
                if(enemy.health > maxHealt && enemy.gameObject.activeInHierarchy)
                {
                    maxHealt = enemy.health; 
                    target = parent.GetChild(i).gameObject; 
                }
            }
        }
        if(target)
        {
            Debug.Log("Target found: " + target.name); 
        } else
        {
            Debug.Log("No target Found"); 
        }
    }
    public void Fire()
    {
        FindTarget();
        fired = true; 
    }
}
