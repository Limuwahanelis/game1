using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        if (direction!=0)
        {
            player.playerMovement.MovePlayer(direction);
        }
        else
        {
            player.playerMovement.StopPlayer();
        }
    }
}
