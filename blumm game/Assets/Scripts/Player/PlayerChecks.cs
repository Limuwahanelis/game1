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
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGround();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheckPos.transform.position, new Vector3(groundCheckSizeX, groundCheckSizeY, 0));
    }

    public void CheckForGround()
    {
      _player.isOnGround= Physics2D.OverlapBox(groundCheckPos.transform.position, new Vector2(groundCheckSizeX, groundCheckSizeY), 0,groundLayer);
    }
}
