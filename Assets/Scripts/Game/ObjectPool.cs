using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> poolObjects = new List<GameObject>();
    public int poolSize;
    public GameObject prefab;
    // Start is called before the first frame update
    private void Awake()
    {
        InitialisePool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitialisePool()
    {
        poolObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            poolObjects.Add(obj);
        }
    }
    public GameObject InstantiateObject(Vector3 position)
    {
        if(poolObjects.Count > 0)
        {
            GameObject obj = poolObjects[0];
            poolObjects.RemoveAt(0);
            obj.transform.position = position;
            obj.SetActive(true);
            return obj;
        }
        return null;
    }
    public GameObject InstantiateObject(Transform parent)
    {
        if (poolObjects.Count > 0)
        {
            GameObject obj = poolObjects[0];
            poolObjects.RemoveAt(0);
            obj.transform.SetParent(parent);
            obj.transform.position = parent.position;
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void DestroyObject(GameObject obj)
    {
        obj.SetActive(false);
        poolObjects.Add(obj);
    }

    public void ClearPool()
    {
        for (int i = poolObjects.Count - 1; i > 0; i--)
        {
            Destroy(poolObjects[i]);
        }
        poolObjects = null;
    }

}
