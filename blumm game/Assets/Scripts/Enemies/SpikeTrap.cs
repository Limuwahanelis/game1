using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float speed;
    public int dmg;
    public LayerMask groundLayer;

    public PlayerDetection playerDetection;
    public Collider2D col;
    public GameObject blockCollider;
    public Transform spikes;
    public Transform placeToFallTo;
    public Vector2 placeToFallToPos;
    private bool _moveSpikes=false;



    public Transform groundCheck;
    public float groundCheckSizeX;
    public float groundCheckSizeY;


    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        playerDetection.OnPlayerDetected = StartTrap;
        _rb = GetComponent<Rigidbody2D>();
        placeToFallToPos = placeToFallTo.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveSpikes)
        {
            transform.position = Vector2.MoveTowards(transform.position, placeToFallToPos, speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.y - placeToFallToPos.y) < 0.1f)
            {
                transform.position = placeToFallToPos;
                _moveSpikes = false;
                StartCoroutine(ChangeColliderToGroundCol());
            }
        }
    }
    private void StartTrap()
    {
        //_rb.velocity = Vector2.down * speed;
        _moveSpikes = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(groundCheckSizeX, groundCheckSizeY));
    }

    IEnumerator ChangeColliderToGroundCol()
    {
        yield return new WaitForSeconds(1f);
        col.enabled = false;
        blockCollider.SetActive(true);

    }

}

