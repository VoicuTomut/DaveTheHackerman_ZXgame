using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedObjectPool : MonoBehaviour
{

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public static AdvancedObjectPool instance;
    void Awake()
    {
        instance = this;
    }

    public void Initialise()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab, transform);
                go.SetActive(false);
                objectPool.Enqueue(go);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject InstantiateObject(string tag, Vector3 position)
    {
        GameObject obj = poolDictionary[tag].Dequeue();
        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public GameObject InstantiateObject(string tag, Transform transform)
    {
        GameObject obj = poolDictionary[tag].Dequeue();
        obj.transform.position = transform.position;
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        poolDictionary[tag].Enqueue(obj);
        return obj;
    }

    public GameObject InstantiateObject(string tag, Vector3 position, Transform transform)
    {
        GameObject obj = poolDictionary[tag].Dequeue();
        obj.transform.position = position;
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        poolDictionary[tag].Enqueue(obj);
        return obj;
    }

    public void DestroyObject(string tag, GameObject gameObject)
    {
        gameObject.SetActive(false);       
    }

    public void ResetPool(string tag)
    {
        Queue<GameObject> q = poolDictionary[tag];
        foreach(GameObject g in q)
        {
            g.SetActive(false);
        }
    }
    public void ResetAll()
    {
        foreach (Pool p in pools)
        {
            foreach (GameObject g in poolDictionary[p.tag])
            {
                g.SetActive(false);
            }
        }

    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
}
