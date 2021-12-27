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
        StartFollow();
        base.Start();
       
    }
   
    public GameObject FindTarget()
    {
        int maxHealt = 0;
        GameObject m_target = null;
        GameObject parent = GameObject.Find("Enemies");
        if (!parent)
        {
            return null; 
        }
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i); 
            if (child.CompareTag("Enemy") )
            {
                Enemy enemy = child.GetComponent<Enemy>();
                if (enemy.health > maxHealt && enemy.gameObject.activeInHierarchy)
                {
                    maxHealt = enemy.health;
                    m_target = child.gameObject;
                }               
            }
        }
      
        return m_target;
    }
    public void FindTargetAndFollow()
    {
        target = FindTarget();
        StartFollow(); 
    }
    public void StartFollow()
    {
        target = FindTarget();
        if (target) GetComponent<FollowTarget>().StartFollow(target);
    }
   

}
