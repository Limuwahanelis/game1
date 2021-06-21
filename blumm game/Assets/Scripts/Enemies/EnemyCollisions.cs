using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable tmp = collision.gameObject.GetComponentInParent<IDamagable>();
        IPushable tmp2 = collision.gameObject.GetComponentInParent<IPushable>();

        if (tmp != null)
        {
            tmp.TakeDamage(_enemy.collisionDmg);

        }
        if (tmp2 != null) tmp2.Push(gameObject);
    }
}
