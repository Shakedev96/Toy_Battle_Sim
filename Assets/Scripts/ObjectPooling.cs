using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] public static ObjectPooling SharedInstance;

    [Tooltip("List containing Gameobjects or objects that need to be pooled in a place")]
    [SerializeField] private List<GameObject> pooledObjects;

    [Tooltip("What to Pool together")]

    [SerializeField] GameObject objectToPool;

    [Tooltip("Total amount to pool")]

    [SerializeField] private int amountToPool;
 
    // Start is called before the first frame update
    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        pooledObjects = new List<GameObject>();

        GameObject temp;

        for(int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(objectToPool);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i<amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
}
