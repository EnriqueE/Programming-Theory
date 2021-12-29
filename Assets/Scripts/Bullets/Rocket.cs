using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    public float lifeTime = 5.0f;
    public float explosionRadius = 1.0f;
    public bool destroyOnBoundaries = false;
    private GameObject target;

    new void Start()
    {
        transform.rotation = Quaternion.identity;        
      //  StartFollowing();
        base.Start();
       
    }
    private void OnEnable()
    {
       // Debug.Log("On Enable"); 
        FindTargetAndFollow();
    }
    public GameObject FindTarget()
    {
        //Debug.Log("Finding Target..."); 
        //int maxHealth = 0;
        GameObject m_target = null;
        GameObject parent = GameObject.Find("Enemies");
        List<GameObject> enemiesToTarget = new List<GameObject>(); 
        if (!parent) return null; 
        
       // Debug.Log("parent.tranform.childCount: " + parent.transform.childCount); 

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
           // Debug.Log("Tag Enemy: " + child.CompareTag("Enemy") + " active: " + child.activeSelf); 
            if (child.CompareTag("Enemy") && child.activeSelf)
            {
            
                    //Debug.Log("Added child: " + child.gameObject.name);
                    enemiesToTarget.Add(child.gameObject); 





                /*Enemy enemy = child.GetComponent<Enemy>();
                if (enemy.health > maxHealth && enemy.gameObject.activeInHierarchy)
                {
                    maxHealth = enemy.health;
                    m_target = child.gameObject;
                } */




            } 
          
        }
       // Debug.Log("enemies.Count: " + enemiesToTarget.Count);


        m_target = enemiesToTarget.Count > 0 ? enemiesToTarget[Random.Range(0, enemiesToTarget.Count)] : null;

       // Debug.Log("Target: " + m_target);
        return m_target;
        
    }
    public void FindTargetAndFollow()
    {
        target = FindTarget();
        
        StartFollowing(); 
    }
    public void StartFollowing()
    {
        //target = FindTarget();
       // Debug.Log("Target: " + target.name);
        if (target) GetComponent<FollowTarget>().StartFollow(target);
    }
   

}
