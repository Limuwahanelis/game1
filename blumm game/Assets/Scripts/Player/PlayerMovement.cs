using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _rb;
    [SerializeField]
    private float _speed;
    private float _flipSide = 1; // 1- right, -1 - left
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer(float direction)
    {
        _rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * _speed, _rb.velocity.y, 0);
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
    public void StopPlayer()
    {
        _player.isMoving = false;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }
    public void Jump()
    {

    }
}
