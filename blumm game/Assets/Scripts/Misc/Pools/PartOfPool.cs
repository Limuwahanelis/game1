using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfPool : MonoBehaviour
{

    public bool isFree = true;
    [HideInInspector]
    public Vector3 poolPos;

    public void ReturnToPool()
    {
        isFree = true;
        transform.position = poolPos;
        gameObject.SetActive(false);
    }

}
