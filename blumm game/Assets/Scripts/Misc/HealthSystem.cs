using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour, IDamagable
{

    public IntReference maxHP;
    public IntReference currentHP;
    public Action OnHitEvent;
    public Action OnDeathEvent;
    // Start is called before the first frame update
    void Start()
    {
        currentHP.value = maxHP.value;
    }
    public virtual void TakeDamage(int dmg)
    {
        currentHP.value =(int)Mathf.Clamp(currentHP.value -= dmg, 0, Mathf.Infinity);
        
        OnHitEvent?.Invoke();
        if (currentHP.value <= 0) Kill();
    }

    public virtual void Kill()
    {
        if (OnDeathEvent == null) Destroy(gameObject);
        else OnDeathEvent.Invoke();
    }
}