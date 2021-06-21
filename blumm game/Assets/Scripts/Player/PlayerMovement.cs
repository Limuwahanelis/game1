using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform t2;

    private Player _player;
    private Rigidbody2D _rb;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _pushForce;
    [SerializeField]
    private float _jumpForce;

    private float _flipSide = 1; // 1- right, -1 - left
    private float _previousDirection;

    public GameObject jumpEffectPrefab;
    public Transform jumpEffectPos;
    // Start is called before the first frame update
    void Start()
    {

        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rb.velocity.y < 0) _player.isFalling = true;
        else _player.isFalling = false;

    }

    public void MovePlayer(float direction)
    {
        if (direction != 0)
        {
            if (_player.isMovableByPlayer)
            {
                _rb.velocity = new Vector3(direction * _speed, _rb.velocity.y, 0);
                if (direction > 0)
                {
                    _flipSide = 1;
                    _player.mainBody.transform.localScale = new Vector3(_flipSide, _player.mainBody.transform.localScale.y, _player.mainBody.transform.localScale.z);
                }
                if (direction < 0)
                {
                    _flipSide = -1;
                    _player.mainBody.transform.localScale = new Vector3(_flipSide, _player.mainBody.transform.localScale.y, _player.mainBody.transform.localScale.z);
                }
                _player.isMoving = true;
            }
        }
        else
        {
           if(_previousDirection!=0 && !_player.isPushedBack) StopPlayerOnXAxis();
        }
        _previousDirection = direction;
    }
    public void StopPlayer()
    {
        _player.isMoving = false;
        _rb.velocity = new Vector2(0, 0);
    }
    public void StopPlayerOnXAxis()
    {
        _player.isMoving = false;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }
    public void Jump()
    {
        if (!_player.isJumping && _player.isOnGround)
        {
            _player.isJumping = true;   
            _player.TakeControlFromPlayer(Player.Cause.JUMP);
            _player.PlayAnimation("Jump",false);
            _player.StartCoroutine(_player.WaitAndExecuteFunction(_player.GetAnimationLength("Jump"), () => 
            {
                _player.isJumping = !_player.isJumping;
                _player.ReturnControlToPlayer(Player.Cause.JUMP);
            }));
        }
    }

    public void JumpAnimationLogic()
    {
        _rb.AddForce(new Vector2(0, _jumpForce));
        GameObject tmp= Instantiate(jumpEffectPrefab, jumpEffectPos.position, jumpEffectPrefab.transform.rotation);
        Destroy(tmp, 1f);
    }
    public void CollidedWithEnemy(GameObject enemy)
    {
        _player.TakeControlFromPlayer(Player.Cause.COLLISION);
        _player.isPushedBack = true;
        float pushDirection = 1;
        if (enemy.transform.position.x > transform.position.x) pushDirection = -1;
        Vector2 tmp = t2.position - transform.position;
        Vector2 pushVector = new Vector2(tmp.x * pushDirection, tmp.y)*_pushForce;
        _rb.AddForce(pushVector,ForceMode2D.Impulse);
        StartCoroutine(_player.WaitForPlayerToLandOnGroundAfterPush());
    }
}
