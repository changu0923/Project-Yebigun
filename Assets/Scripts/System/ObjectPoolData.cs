using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPoolData
{
    private string poolName;
    private GameObject prefab;
    private int capacity;

    public string PoolName { get => poolName; set => poolName = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Capacity { get => capacity; set => capacity = value; }

    public ObjectPoolData(string poolName, GameObject prefab, int initSize = 50) 
    { 
        this.poolName = poolName;
        this.prefab = prefab;
        capacity = initSize;
    }
}
