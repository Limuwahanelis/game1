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
        if (!PauseMenu.isGamePaused)
        {
            if (player.isAlive)
            {
                player.playerMovement.MovePlayer(direction);
                if (Input.GetButtonDown("Jump"))
                {
                    player.playerMovement.Jump();
                }
                if (Input.GetButtonDown("Attack"))
                {
                    player.playerCombat.Attack();
                }
            }
        }
    }
}
