using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tras : MonoBehaviour
{
    private Rigidbody2D _rb;
    public int dmg;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable tmp1 = collision.GetComponentInParent<IDamagable>();
        IPushable tmp2 = collision.GetComponentInParent<IPushable>();
        if (tmp1 != null) tmp1.TakeDamage(dmg);
        if (tmp2 != null) tmp2.Push(gameObject);
    }
}
