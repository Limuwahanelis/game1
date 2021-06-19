using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSet<T> : ScriptableObject
{
    public List<T> items = new List<T>();
    // Start is called before the first frame update
    public void Add(T item)
    {
        if (!items.Contains(item)) items.Add(item);
    }
    public void Remove(T item)
    {
        if (items.Contains(item)) items.Remove(item);
    }
}
