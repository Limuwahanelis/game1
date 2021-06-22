using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{

    public PlayerDetection playerDetection;
    public Collider2D col;
    public GameObject blockCollider;
    public Transform spikes;

    private bool _moveSpikes=false;
    public float speed;
    public int dmg;


    public Transform groundCheck;
    public float groundCheckSizeX;
    public float groundCheckSizeY;

    public LayerMask groundLayer;

    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        playerDetection.OnPlayerDetected = StartTrap;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveSpikes)
        {
            if (!Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(groundCheckSizeX, groundCheckSizeY), 0, groundLayer))
            {

                transform.Translate(Vector3.down * Time.deltaTime * speed);
            }
            else
            {
                _moveSpikes = false;
                col.enabled = false;
                blockCollider.SetActive( true);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        IDamagable target = collision.GetComponentInParent<IDamagable>();
        if(target!=null)
        {
            target.TakeDamage(dmg);
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Foo");
    //    blockCollider.SetActive(true);
    //}

}

