using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolController))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponTriggerType weaponTriggerType;
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
    public enum WeaponTriggerType {  always, manual, onFireKey, onRightAltKey, onLeftControlKey }
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
        if (Time.time - lastFireTime > delay)
        {
            if ((Input.GetKey(KeyCode.Space) && weaponTriggerType == WeaponTriggerType.onFireKey) ||
                (Input.GetKey(KeyCode.RightAlt) && weaponTriggerType == WeaponTriggerType.onRightAltKey) ||
                (Input.GetKey(KeyCode.LeftControl) && weaponTriggerType == WeaponTriggerType.onLeftControlKey) ||
                (weaponTriggerType == WeaponTriggerType.always &&
                Time.time - initTime > startDelay &&
                (elementsFired < maxElements || maxElements == 0)))
            {
                FireElement();
            }
        }
    }
   
    // Shoot one bullet
    public void FireElement()
    {

        GameObject element = poolController.GetOne(); 
        if(element)
        {
            Bullet bullet = element.GetComponent<Bullet>();
            element.transform.position = transform.position;
            element.transform.rotation = gameObject.transform.rotation;
            bullet.SetDamage(damage);
            bullet.SetSpeed(speed);
            bullet.transform.rotation = Quaternion.identity; 
            bullet.parentName = gameObject.transform.root.gameObject.name;
            element.SetActive(true);

            lastFireTime = Time.time;
            elementsFired++;
        }
    }
}
