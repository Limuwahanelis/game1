using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerDetection : MonoBehaviour
{

    public Action OnPlayerDetected;
    public Action OnPlayerLeft;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPlayerDetected?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnPlayerLeft?.Invoke();
    }
}
