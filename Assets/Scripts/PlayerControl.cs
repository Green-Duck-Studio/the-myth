using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerControl : MonoBehaviour
{
	private Rigidbody2D rb;
	public float speed;
	public float jumpPower;

	private Vector2 vecMove;
	private bool facingRight;
	public Animator anim;
	public int loadScene;
	
	public bool gameIsPaused = false;
	public GameObject pauseMenuUI;
	public bool isOnGround;
	public Transform attackPoint;
	public float attackRange = .5f;
	public int attackDamage = 30;
	public LayerMask enemyLayer;
	public Enemy_Behaviour targetEnemy;

	private float currentHealth;

	public float maxHealth;
	//public Image healthBar;

	void Start()
    {
		rb = GetComponent<Rigidbody2D>();

		maxHealth = currentHealth;
    }

	private void Update()
	{
		//healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, -100, 100);
	}

	public void Pause(InputAction.CallbackContext ctx)
    {
	    if (ctx.started)
	    {
		    if (gameIsPaused)
		    {
			    Resume();
		    }
		    else
		    {
			    Paused();
		    }
	    }
    }

	public void Jump(InputAction.CallbackContext ctx)
	{
		if(ctx.started)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpPower);
			RaycastHit2D hit = Physics2D.CircleCast(transform.position, .2f, Vector2.down, .1f, LayerMask.GetMask("Ground"));
			isOnGround = hit.collider != null;

			if (isOnGround)
			{
				rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
				anim.SetBool("Jump", true);
			}
			else
			{
				anim.SetBool("Jump", false);
			}
		}
	}

	public void Movement(InputAction.CallbackContext ctx)
	{
		vecMove = ctx.ReadValue<Vector2>();
		Flip();

		if (vecMove.magnitude > 0)
		{
			anim.SetBool("Walk", true);
		}
		else
		{
			anim.SetBool("Walk", false);
		}
		
	}

	public void Interact(InputAction.CallbackContext ctx)
	{
		
	}

	public void Attack(InputAction.CallbackContext ctx)
	{
		if(ctx.performed)
		{
			anim.SetBool("Attack", true);

			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

			foreach (Collider2D enemy in hitEnemies)
			{
				//enemy.GetComponent<Enemy_Behaviour>().TakeDamage(attackDamage);
				targetEnemy.TakeDamage(attackDamage, loadScene);
			}
		}
		else
		{
			anim.SetBool("Attack", false);
		}
	}

	private void OnDrawGizmos()
	{
		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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
	
	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		gameIsPaused = false;
	}

	public void Paused()
	{
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		gameIsPaused = true;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		
		if (currentHealth < 0)
		{
			// Destroy(gameObject);
		}
	}
}
