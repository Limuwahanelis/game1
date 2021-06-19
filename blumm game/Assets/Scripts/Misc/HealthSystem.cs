using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour, IDamagable,IPushable
{
    public bool isInvincible;
    public IntReference maxHP;
    public IntReference currentHP;
    public Action OnHitEvent;
    public Action OnDeathEvent;
    public Action<GameObject> OnPushEvent;
    // Start is called before the first frame update
    void Start()
    {
        currentHP.value = maxHP.value;
    }
    public void TakeDamage(int dmg)
    {
        if (!isInvincible)
        {
            currentHP.value = (int)Mathf.Clamp(currentHP.value -= dmg, 0, Mathf.Infinity);
            if (currentHP.value <= 0)
            {
                Kill();
                return;
            }
            OnHitEvent?.Invoke();
        }
    }

    public virtual void Kill()
    {
        if (OnDeathEvent == null) Destroy(gameObject);
        else OnDeathEvent.Invoke();
    }

    public void Push(GameObject pusher)
    {
        OnPushEvent?.Invoke(pusher);
    }
}