using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour, IDamagable,IPushable
{
    public bool isInvincible;
    public bool isPushable = true;
    public IntReference maxHP;
    public IntReference currentHP;
    public Action OnHit;
    public Action OnDeath;
    public Action<GameObject> OnPush;
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
            OnHit?.Invoke();
        }
    }

    public virtual void Kill()
    {
        if (OnDeath == null) Destroy(gameObject);
        else OnDeath.Invoke();
    }

    public void Push(GameObject pusher)
    {
        if(isPushable) OnPush?.Invoke(pusher);
    }
}