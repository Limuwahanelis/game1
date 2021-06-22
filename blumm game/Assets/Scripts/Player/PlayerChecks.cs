using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecks : MonoBehaviour
{
    private Player _player;
    public Transform groundCheckPos;
    public float groundCheckSizeX;
    public float groundCheckSizeY;
    public LayerMask groundLayer;

    public Transform leftWallCheck;
    public Transform rightWallCheck;

    public Vector2 wallCheckSizes;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGround();
        CheckForWall();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheckPos.transform.position, new Vector3(groundCheckSizeX, groundCheckSizeY, 0));
        Gizmos.DrawWireCube(leftWallCheck.position, wallCheckSizes);
        Gizmos.DrawWireCube(rightWallCheck.position, wallCheckSizes);
    }

    public void CheckForGround()
    {
      _player.isOnGround= Physics2D.OverlapBox(groundCheckPos.transform.position, new Vector2(groundCheckSizeX, groundCheckSizeY), 0,groundLayer);
    }

    public void CheckForWall()
    {
        bool isNearWall = Physics2D.OverlapBox(leftWallCheck.position, wallCheckSizes, 0, groundLayer);
        if(!isNearWall) isNearWall= Physics2D.OverlapBox(rightWallCheck.position, wallCheckSizes, 0, groundLayer);
        _player.isNearWall = isNearWall;
    }
}
