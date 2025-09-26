using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    #region Singleton
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectPoolManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ObjectPoolManager");
                    instance = obj.AddComponent<ObjectPoolManager>();
                }
            }
            return instance;
        }
    }
    #endregion
    
    private Dictionary<string, ObjectPoolData> dataDict = new Dictionary<string, ObjectPoolData>(); 
    private Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();

    public void CreatePool(string poolName, GameObject prefab, int initSize = 10)
    {      
        if (poolDict.ContainsKey(poolName) == false)
        {
            ObjectPoolData newData = new ObjectPoolData(poolName, prefab, initSize);

            Queue<GameObject> newPool = new Queue<GameObject>();
            for (int i = 0; i < initSize; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }
            poolDict[poolName] = newPool;
            dataDict[poolName] = newData;
        }
    }

    // 큐에 풀링할 오브젝트가 부족하면, 오브젝트풀 데이터에서 해당 오브젝트를 찾아 캐퍼시티를 2배로 늘린 후 큐에도 2배 넣습니다.
    private void AddCapacity(string poolName)
    {
        ObjectPoolData currentData = dataDict[poolName];
        Queue<GameObject> targetQueue = poolDict[poolName];
        int capacity = currentData.Capacity * 2;
        currentData.Capacity = capacity;

        for (int i = 0; i < capacity; i++)
        {
            GameObject obj = Instantiate(currentData.Prefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            targetQueue.Enqueue(obj);
        }
    }

    public GameObject Instantiate(string poolName, GameObject prefab)
    {
        if(poolDict.ContainsKey(poolName))
        {
            Queue<GameObject> pool = poolDict[poolName];
            ObjectPoolData currentPoolData = dataDict[poolName];
            if(pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                AddCapacity(poolName);
                return Instantiate(poolName, prefab);
            }
        }
        else
        {
            CreatePool(poolName, prefab);
            return Instantiate(poolName, prefab);
        }
    }

    public void Destroy(string poolName, GameObject obj)
    {
        if (poolDict.ContainsKey(poolName))
        {
            obj.SetActive(false);
            poolDict[poolName].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}
