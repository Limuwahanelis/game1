using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Player _player;
    public Transform attackPos;
    public float attackRange;
    public LayerMask enemyLayer;
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
            Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayer);
            _player.isAttacking = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        
    }
    
}
