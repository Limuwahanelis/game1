using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomPool<T> : MonoBehaviour
{
    protected List<T> items = new List<T>();
    // Start is called before the first frame update
    public void Add(T item)
    {
        if (!items.Contains(item)) items.Add(item);
    }
    public void Remove(T item)
    {
        if (items.Contains(item)) items.Remove(item);
    }
    public abstract T GetItem();
}
