using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour, IDamagable
{
    public bool isInvincible;
    public IntReference maxHP;
    public IntReference currentHP;
    public Action<GameObject> OnHitEvent;
    public Action OnDeathEvent;
    // Start is called before the first frame update
    void Start()
    {
        currentHP.value = maxHP.value;
    }
    public virtual void TakeDamage(int dmg,GameObject dmgDealer)
    {
        if (!isInvincible)
        {
            currentHP.value = (int)Mathf.Clamp(currentHP.value -= dmg, 0, Mathf.Infinity);
            if (currentHP.value <= 0)
            {
                Kill();
                return;
            }
            OnHitEvent?.Invoke(dmgDealer);
        }
    }

    public virtual void Kill()
    {
        if (OnDeathEvent == null) Destroy(gameObject);
        else OnDeathEvent.Invoke();
    }
}