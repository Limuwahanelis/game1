using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField]
    private float _attackRangeX;
    [SerializeField]
    private float _attackRangeY;
    [SerializeField]
    private Transform _attackTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_attackTransform.position, new Vector3(_attackRangeX, _attackRangeY));
    }
}
