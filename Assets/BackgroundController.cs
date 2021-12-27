using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    public GameObject[] backgrounds;
    public bool randomizeBackgrounds = false;
    public float speed; 
    public bool isEnabled = true;
    private Vector2 maxSize = new Vector2(27.8f, 15.708f);
    private void Start()
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            // scale image 
            Vector3 backgroundSize = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size;
            Vector2 newScale = new Vector2(maxSize.x / backgroundSize.x, maxSize.y / backgroundSize.y);
            float scale = Mathf.Max(newScale.x, newScale.y);
            backgrounds[i].gameObject.transform.localScale = new Vector3(scale, scale, 1);
            Vector3 newBackgroundSize = backgroundSize * scale;
            transform.Translate(Vector3.up * (newBackgroundSize.y - maxSize.y) / 2);
            

        }
        
        
    }

    private void Update()
    {
        if (isEnabled)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            /*  if (transform.position.y < destination)
              {
                  transform.position = new Vector3(transform.position.x, spawnPosition, transform.position.z);
                      GetComponent<ParticleSystem>().Play();

              1.54




              }*/ 
        }

    }


}
