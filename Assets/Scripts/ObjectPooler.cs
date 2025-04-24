using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooler
{
    public static Dictionary<string, Queue<Component>> pool = new();

    public static void EnqueueObject<T>(T item, string name) where T : Component
    {
        if (!item.gameObject.activeSelf) return;

        item.transform.position = Vector3.zero;
        pool[name].Enqueue(item);
        item.gameObject.SetActive(false);
    }

    public static T DequeueObject<T>(string key) where T : Component
    {
        return (T)pool[key].Dequeue();
    }

    public static void SetupPool<T>(T pooledItemPrefab, int poolSize, string dictionaryEntry) where T : Component
    {
        pool.Add(dictionaryEntry, new Queue<Component>());

        for (int i = 0; i < poolSize; ++i)
        {
            T pooledInstance = Object.Instantiate(pooledItemPrefab);
            pooledInstance.gameObject.SetActive(false);
            pool[dictionaryEntry].Enqueue(pooledInstance);
        }
    }
}
