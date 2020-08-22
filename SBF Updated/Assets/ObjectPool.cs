using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    public T prefab;
    public int count;
    List<T> freeList;
    List<T> usedList;

    public void Awake()
    {
        freeList = new List<T>(count);
        usedList = new List<T>(count);

        // Instantiate the pooled objects and disable them.
        for (var i = 0; i < count; i++)
        {
            var pooledObject = Instantiate(prefab, transform);
            pooledObject.gameObject.SetActive(false);
            freeList.Add(pooledObject);
        }
    }

    public T Get()
    {
        var numFree = freeList.Count;
        if (numFree == 0)
            return null;

        // Pull an object from the end of the free list.
        var pooledObject = freeList[numFree - 1];
        freeList.RemoveAt(numFree - 1);
        usedList.Add(pooledObject);
        return pooledObject;
    }

    public void ReturnObject(T pooledObject)
    {
        Debug.Assert(usedList.Contains(pooledObject));

        // Put the pooled object back in the free list.
        usedList.Remove(pooledObject);
        freeList.Add(pooledObject);

        // Reparent the pooled object to us, and disable it.
        var pooledObjectTransform = pooledObject.transform;
        pooledObjectTransform.parent = transform;
        pooledObjectTransform.localPosition = Vector3.zero;
        pooledObject.gameObject.SetActive(false);
    }
}

