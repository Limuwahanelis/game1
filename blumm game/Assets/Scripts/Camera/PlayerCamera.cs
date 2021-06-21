using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player player;

    public Vector3 offset;

    public Transform leftScreenBorder;
    public Transform rightScreenBorder;


    public float smoothTime = 0.3f;

    private bool _followOnXAxis=true;
    private Vector3 targetPos;
    
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void Update()
    {
        if(player.transform.position.x<leftScreenBorder.position.x)
        {
            _followOnXAxis = false;
            targetPos = new Vector3(leftScreenBorder.position.x, player.transform.position.y) + offset;
        }
        else
        {
            if (player.transform.position.x > rightScreenBorder.position.x)
            {
                _followOnXAxis = false;
                targetPos = new Vector3(rightScreenBorder.position.x, player.transform.position.y) + offset;
            }
            else
            {
                _followOnXAxis = true;
            }
        }





    }
    private void LateUpdate()
    {
        if(_followOnXAxis)
        {
            targetPos = new Vector3( player.transform.position.x, player.transform.position.y)+offset;
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPos,ref velocity, smoothTime);

    }
}
