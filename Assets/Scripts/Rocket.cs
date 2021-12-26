using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 2.0f;
    public float lifeTime = 5.0f;
    public float explosionRadius = 1.0f;
    public int damage = 50;
    public bool destroyOnBoundaries = false;
    public bool autoFire = false;


    private bool fired = false;
    public GameObject target;




    void Start()
    {
        transform.rotation = Quaternion.identity; 
        if (autoFire)
        {          
            Fire();
        }
    }
   
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);        
    }
    public GameObject FindTarget()
    {
        int maxHealt = 0;
        GameObject m_target = null; 
        Transform parent = GameObject.Find("Enemies").transform;


        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag("Enemy"))
            {
                Enemy enemy = parent.GetChild(i).GetComponent<Enemy>();
                if (enemy.health > maxHealt && enemy.gameObject.activeInHierarchy)
                {
                    maxHealt = enemy.health;
                    m_target = parent.GetChild(i).gameObject;
                }               
            }
        }
        return m_target;
    }
    public void Fire()
    {
        target = FindTarget();
        if (target) GetComponent<FollowTarget>().StartFollow(target);
        fired = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with " + other.gameObject.name); 
    }

}
