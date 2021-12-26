using UnityEngine;

public class Rocket : Bullet
{
    public float lifeTime = 5.0f;
    public float explosionRadius = 1.0f;
    public bool destroyOnBoundaries = false;
    public bool autoFire = false;
    private bool fired = false;
    public GameObject target;




    new void Start()
    {
        transform.rotation = Quaternion.identity; 
        if (autoFire)
        {
            StartFollow();
        }
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
            if (parent.transform.GetChild(i).CompareTag("Enemy"))
            {
                Enemy enemy = parent.transform.GetChild(i).GetComponent<Enemy>();
                if (enemy.health > maxHealt && enemy.gameObject.activeInHierarchy)
                {
                    maxHealt = enemy.health;
                    m_target = parent.transform.GetChild(i).gameObject;
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
        fired = true;
    }
   

}
