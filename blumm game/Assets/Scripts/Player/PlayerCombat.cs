using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Player _player;
    public Transform attackPos;
    public float attackRange;
    public LayerMask enemyLayer;
    public int dmg;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (!_player.isAttacking && _player.isOnGround)
        {
            _player.playerMovement.StopPlayer();
            Collider2D[] hitEnemies =Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayer);
            for(int i=0;i<hitEnemies.Length;i++)
            {
                IDamagable enemy = hitEnemies[i].GetComponentInParent<IDamagable>();
                if (enemy!=null) enemy.TakeDamage(dmg,gameObject);
            }
            _player.isAttacking = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        
    }
    
}
