using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Player _player;
    public Transform attackPos;
    public float attackRangeX;
    public float attackRangeY;
    public float InvincibilityTime;
    public LayerMask enemyLayer;
    public int dmg;

    public Coroutine invincibilityCor;

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
            Collider2D[] hitEnemies =Physics2D.OverlapBoxAll(attackPos.position,new Vector2(attackRangeX,attackRangeY),0 ,enemyLayer);
            for(int i=0;i<hitEnemies.Length;i++)
            {
                IDamagable enemy = hitEnemies[i].GetComponentInParent<IDamagable>();
                if (enemy!=null) enemy.TakeDamage(dmg );
            }
            _player.isAttacking = true;
        }
    }

    public void PlayerHit()
    {
        _player.TakeControlFromPlayer(Player.Cause.ENEMY);
        StartCoroutine(_player.WaitAndExecuteFunction(_player.GetAnimationLength("Hit"),()=> { _player.ReturnControlToPlayer(Player.Cause.ENEMY); }));
        _player.OverPlayAnimation("Hit");
        invincibilityCor=StartCoroutine(InvincibilityCor());
    }

    public void KillPlayer()
    {
        _player.playerHealth.isInvincible = true;
        if(!_player.checkForLastPush)
        {
            _player.checkForLastPush = true;
            StartCoroutine(CheckForLatPushCor());
            return;
        }
        if (_player.checkForLastPush)
        {
            _player.StopAllActions();
            _player.PlayAnimation("Death");
            _player.playerCore.enabled = false;
            _player.isAlive = false;
        }
    }
    public IEnumerator InvincibilityCor()
    {
        if (_player.playerHealth.isInvincible) yield break;
        else _player.playerHealth.isInvincible = true;
        yield return new WaitForSeconds(InvincibilityTime);
        _player.playerHealth.isInvincible = false;
    }
    public IEnumerator NotPushableCor()
    {
        if (!_player.playerHealth.isPushable) yield break;
        _player.playerHealth.isPushable = false;
        yield return new WaitForSeconds(InvincibilityTime);
        _player.playerHealth.isPushable = true;
    }
    private IEnumerator CheckForLatPushCor()
    {
        yield return new WaitForSeconds(0.02f);
        if (_player.checkForLastPush) KillPlayer();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY));
    }
}
