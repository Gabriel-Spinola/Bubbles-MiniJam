using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim = null;
    private Player player = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("[Player]").GetComponent<Player>();
    }

    private void LateUpdate()
    {
        anim.SetBool("isMoving", InputManager.I.xAxis != 0);
        anim.SetBool("isJumping", InputManager.I.keyJump);
        anim.SetBool("isFalling", player.GetRigidbody().velocity.y < .1f && !player.isGrounded);
    }
}
