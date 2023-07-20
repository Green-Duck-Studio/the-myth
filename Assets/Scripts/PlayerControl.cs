using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
	private Rigidbody2D rb;
	public float speed;
	public float jumpPower;

	private Vector2 vecMove;
	private bool facingRight;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

	public void Jump(InputAction.CallbackContext ctx)
	{
		if(ctx.started)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpPower);
		}
	}

	public void Movement(InputAction.CallbackContext ctx)
	{
		vecMove = ctx.ReadValue<Vector2>();
		Flip();
	}

	public void Interact(InputAction.CallbackContext ctx)
	{

	}

	private void FixedUpdate()
	{
		rb.velocity = new Vector2(vecMove.x * speed, rb.velocity.y);
	}

	private void Flip()
	{
		if (vecMove.x < -0.01f) transform.localScale = new Vector3(-1, 1, 1);
		if (vecMove.x > 0.01f) transform.localScale = new Vector3(1, 1, 1);
	}
}
