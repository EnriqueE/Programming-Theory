using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolController))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponTriggerType weaponTriggerType;
    public KeyCode[] weaponTriggerKey;
    
    public GameObject prefab;
    public int damage;
    public float speed;
    public float delay = 0.1f;
    // When weaponTriggerType is set to always maxBullets act as a max amount
    public int maxElements = 0;
    private int elementsFired = 0;
    public float startDelay = 0.0f;
    private float lastFireTime;
    private PoolController poolController;
    private float initTime = 0.0f;

    [System.Serializable]
    public struct WeaponData
    {        
        public int damage;
        public int maxElements;
        public float delay;
        public float startDelay;
        public float speed;
    }
   
    public void UpdateWeaponData(WeaponData weaponData)
    {
        damage = weaponData.damage; 
        speed = weaponData.speed;
        delay = weaponData.delay;   
        maxElements = weaponData.maxElements;
        startDelay = weaponData.startDelay;
        UpdatePoolProperties(damage, speed); 
    }
    
    // Trigger type to auto fire bullet
    public enum WeaponTriggerType { always, manual, onWeaponTriggerKey }
    private void Start()
    {
        initTime = Time.time;
        // Create pool of bullets
        poolController = GetComponent<PoolController>();
        poolController.CreatePool(prefab, damage, speed, maxElements);
    }
    private void Update()
    {
        // If it allowed to fire, start delay and rate apply
        if (Time.time - lastFireTime > delay && Time.time - initTime > startDelay)

        {

            switch (weaponTriggerType)
            {
                case WeaponTriggerType.always:
                    if (elementsFired < maxElements || maxElements == 0)
                    {
                        FireElement();
                    }
                    break;
                case WeaponTriggerType.onWeaponTriggerKey:
                    for (int i = 0; i < weaponTriggerKey.Length; i++)
                    {
                        if (Input.GetKey(weaponTriggerKey[i]))
                        {
                            FireElement();
                        }
                    }
                    break;
            }
        }
    }

    // Shoot one bullet
    public void FireElement()
    {

        GameObject element = poolController.GetOne();
        if (element)
        {
            Bullet bullet = element.GetComponent<Bullet>();
            // Set position and rotation 
            element.transform.position = transform.position;
            element.transform.rotation = gameObject.transform.rotation;
            // Transfer speed and damage from weapon 
            bullet.SetDamage(damage);
            bullet.SetSpeed(speed);
            // Prevent rotation in x y, only z allowed
            bullet.transform.eulerAngles = new Vector3(0, 0, bullet.transform.eulerAngles.z);
            bullet.parentName = gameObject.transform.root.gameObject.name;
            element.SetActive(true);

            lastFireTime = Time.time;
            elementsFired++;
        }
    }
    public void UpdatePoolProperties(int m_damage, float m_speed)
    {
        Bullet[] bullets = GetComponentsInChildren<Bullet>(true);
        foreach(Bullet bullet in bullets)
        {
            bullet.speed = m_speed;
            bullet.damage = m_damage; 
            
        }

    }
}
