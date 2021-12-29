using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private List<Background> backgrounds;
    public List<GameObject> instantiatedBackgrounds; 
    public bool randomizeBackgrounds = false;
    public float speed;
    public bool isEnabled = true;
    private Vector2 maxSize = new Vector2(27.9f, 15.708f);
    private int currentBgCount = 0;
    private Vector3 currentBgPosition;
    private float previousBgHeight;
    private int totalBackgrounds;
    public float positionToCreateNewOne;
    private int startBackgrounds = 2; 

    [System.Serializable]
    public struct Background
    {
        public GameObject prefab;
        public int quantity;

    }

    private GameObject GetNextBackgroundPrefab()
    {
        int current = 0; 
        foreach(Background bg in backgrounds)
        {
            for(int i = 0; i< bg.quantity; i++)
            {
                
                if(current == currentBgCount % totalBackgrounds )
                {
                    currentBgCount++; 
                    return bg.prefab; 
                }
                current++;
            }
        }
        return null; 
    }
    private void ResizeBackground(GameObject bg)
    {
        Vector3 backgroundSize = bg.GetComponent<SpriteRenderer>().bounds.size;
        Vector2 newScale = new Vector2(maxSize.x / backgroundSize.x, maxSize.y / backgroundSize.y);
        float scale = Mathf.Max(newScale.x, newScale.y);
        bg.gameObject.transform.localScale = new Vector3(scale, scale, 1);
    }
    private void MoveBackgrond(GameObject bg)
    {
        if (currentBgCount == 1)
        {
            currentBgPosition = Vector3.up * (bg.GetComponent<SpriteRenderer>().bounds.size.y - maxSize.y) / 2;
        } else
        {
            currentBgPosition += new Vector3(0,previousBgHeight / 2 + bg.GetComponent<SpriteRenderer>().bounds.size.y / 2,0); 
        }
        bg.transform.Translate(currentBgPosition);


    }
    private void NewBackground()
    {
        //Debug.Log("Creating background " + currentBgCount); 
        GameObject bg = Instantiate(GetNextBackgroundPrefab(), transform);
        instantiatedBackgrounds.Add(bg); 
        ResizeBackground(bg);
        MoveBackgrond(bg);
        previousBgHeight = bg.GetComponent<SpriteRenderer>().bounds.size.y;
        if (currentBgCount >= startBackgrounds)
        {
            positionToCreateNewOne += previousBgHeight;
        }

    }
    private void CreateAndDeleteOne()
    {
        Destroy(instantiatedBackgrounds[0]);
        instantiatedBackgrounds.RemoveAt(0); 
        NewBackground(); 
    }
    private void Start()
    {
        // Calcultate total backgrounds
        foreach(Background bg in backgrounds) { totalBackgrounds += bg.quantity; }

        for(int i = 0; i < startBackgrounds; i++)
        {
            NewBackground();
        }
        
        









        /*
             if(backgrounds.Count == 1)
            {
               // backgrounds.Add(backgrounds[0]);
            }
            for(int i = 0; i < backgrounds.Count; i++)
            {
                // scale image
                GameObject bg = Instantiate(backgrounds[i].gameObject, gameObject.transform); 
                Vector3 backgroundSize = bg.GetComponent<SpriteRenderer>().bounds.size;
                Vector2 newScale = new Vector2(maxSize.x / backgroundSize.x, maxSize.y / backgroundSize.y);
                float scale = Mathf.Max(newScale.x, newScale.y);
                bg.gameObject.transform.localScale = new Vector3(scale, scale, 1);
                Vector3 newBackgroundSize = backgroundSize * scale;
                bg.transform.Translate(Vector3.up * (bg.GetComponent<SpriteRenderer>().bounds.size.y - maxSize.y) / 2);
                if(i>0)
                {
                    for(int a = 0; a < i; a++)
                    {
                        bg.transform.Translate(Vector3.up * backgrounds[a].GetComponent<SpriteRenderer>().bounds.size.y);
                    }
                }
            

            }
        */


    }

    private void Update()
    {
        if (isEnabled)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if(transform.position.y <= -positionToCreateNewOne)
            {
                CreateAndDeleteOne();
                
            }
            /*  if (transform.position.y < destination)
              {
                  transform.position = new Vector3(transform.position.x, spawnPosition, transform.position.z);
                      GetComponent<ParticleSystem>().Play();

              1.54




              }*/
        }

    }

   
}
