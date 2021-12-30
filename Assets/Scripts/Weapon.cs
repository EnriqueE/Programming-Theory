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

    // Trigger type to auto fire bullet
    public enum WeaponTriggerType { always, manual, onWeaponTriggerKey }
    private void Start()
    {
        //Debug.Log("Speed: " + speed); 
        initTime = Time.time;

        // Create pool of bullets
        poolController = GetComponent<PoolController>();
        poolController.CreatePool(prefab, damage, speed, maxElements);
        // Debug.Log("Creating Pool: " + damage + " , " + speed); 


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

            // If key Down


            /*        
                    
                    if (weaponTriggerType == WeaponTriggerType.always &&
                        (elementsFired < maxElements || maxElements == 0))
                    {

                    }
                }                  
            }
            if ((Input.GetKey(KeyCode.Space) && weaponTriggerType == WeaponTriggerType.onFireKey) ||
                (Input.GetKey(KeyCode.RightAlt) && weaponTriggerType == WeaponTriggerType.onRightAltKey) ||
                (Input.GetKey(KeyCode.LeftControl) && weaponTriggerType == WeaponTriggerType.onLeftControlKey) ||
                (weaponTriggerType == WeaponTriggerType.always &&              
                (elementsFired < maxElements || maxElements == 0)))
            {
                FireElement();
            }*/
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
        Debug.Log("Updating properties of " + bullets.Length + " bullets."); 
        foreach(Bullet bullet in bullets)
        {
            bullet.speed = m_speed;
            bullet.damage = m_damage; 
        }

    }
}
