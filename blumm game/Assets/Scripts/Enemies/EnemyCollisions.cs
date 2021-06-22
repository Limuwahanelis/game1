using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    public int collisionDmg;
    private Enemy enemy;
    private void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable tmp = collision.gameObject.GetComponentInParent<IDamagable>();
        IPushable tmp2 = collision.gameObject.GetComponentInParent<IPushable>();

        if (tmp != null)
        {
            tmp.TakeDamage(collisionDmg);

        }
        if (tmp2 != null) tmp2.Push(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamagable tmp = collision.gameObject.GetComponentInParent<IDamagable>();
        IPushable tmp2 = collision.gameObject.GetComponentInParent<IPushable>();

        if (tmp != null)
        {
            tmp.TakeDamage(collisionDmg);

        }
        if (tmp2 != null) tmp2.Push(gameObject);
    }
    private void OnValidate()
    {
        enemy = GetComponentInParent<Enemy>();
        if (enemy != null) collisionDmg = enemy.collisionDmg;
    }
}
