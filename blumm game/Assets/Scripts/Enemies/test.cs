using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public string toPrint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(toPrint);
    }
}
