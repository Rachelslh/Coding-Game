using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler _instance;

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    // Start is called before the first frame update
    void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                obj.tag = "Obstacle";
                objectPool.Enqueue(obj);
            }

            poolDict.Add(pool.tag, objectPool);   
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.Log("Pool with tag = " + tag + " dosen't exist");
            return null;
        }
        GameObject objectToSpawn = poolDict[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDict[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
