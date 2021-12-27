using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    private bool destroyPending = false;
    private float timeToDestroy = 4.0f;
    private GameObject mainPool;
    private GameObject pool;
    private GameObject prefab;
    private List<GameObject> elements;
    public int startPoolSize = 5;    
    private int damage;
    private float speed;
    public float maxPoolSize = 0; 

    public void DestroyPool()
    {
        Destroy(gameObject, timeToDestroy); 
    }
    public void MarkToDestroy()
    {
        destroyPending = true; 
    }

    private void FindOrCreateMainPool()
    {
        mainPool = GameObject.Find("Pool");
        if (!mainPool)
        {
            mainPool = new GameObject();
            mainPool.name = "Pool";
        }
    }
   
    public void CreatePool(GameObject m_prefab)
    {
        prefab = m_prefab;         
        FindOrCreateMainPool();
        pool = new GameObject();
        pool.name = "Pool of " + gameObject.name + " of " + gameObject.transform.parent.name;
        pool.transform.parent = mainPool.transform;
        pool.AddComponent<PoolController>();
        elements = new List<GameObject>();
        for(int i = 0; i < startPoolSize; i++) {
            IncreasePool();
        }
    }
    public void CreatePool(GameObject m_prefab, int m_damage, float m_speed, int m_maxPoolSize)
    {
        speed = m_speed;
        damage = m_damage;
        maxPoolSize = m_maxPoolSize; 
        CreatePool(m_prefab);
    }

    // Add one new element to the pool List
    private void IncreasePool()
    {
        GameObject prefabInstance;
        prefabInstance = Instantiate(prefab, pool.transform);
        prefabInstance.name = "Element of " + gameObject.name + " of " + gameObject.transform.root.name;
        prefabInstance.transform.Translate(gameObject.transform.position);        
        prefabInstance.SetActive(false);
        elements.Add(prefabInstance);
    }
    public GameObject GetOne()
    {
        GameObject element = TryToGetOne();
        if(element == null && (elements.Count < maxPoolSize || maxPoolSize == 0))
        {
            IncreasePool();
            element = TryToGetOne();            
        }
        return element; 

    }
    private GameObject TryToGetOne()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (!elements[i].activeInHierarchy)
            {
                return elements[i];
            }
        }
        return null;

    }
    public bool isPendingDestroy()
    {
        return destroyPending;
    }
}
