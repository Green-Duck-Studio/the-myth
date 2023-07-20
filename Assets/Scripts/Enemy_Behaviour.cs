using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour
{
	public Transform rayCast;
	public LayerMask rayCastMask;
	public float rayCastLength;
	public float attackDistance;
	public float moveSpeed;
	public float timer;

	private RaycastHit2D hit;
	private GameObject target;
	private Animator anim;
	private float distance;
	private bool attackMode;
	private bool inRange;
	private bool cooling;
	private float intTimer;

	private void Awake()
	{
		intTimer = timer;
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if(inRange)
		{
			hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
			RaycastDebugger();
		}
		
		if(hit.collider != null)
		{
			EnemyLogic();
		}else if(hit.collider == null)
		{
			inRange = false;
		}

		if (inRange == false)
		{
			anim.SetBool("Walk", false);
			StopAttack();
		}

	}

	private void EnemyLogic()
	{
		distance = Vector2.Distance(transform.position, target.transform.position);
		if(distance > attackDistance)
		{
			Move();
			StopAttack();
		}
		else if(attackDistance >= distance && cooling == false)
		{
			Attack();
		}

		if(cooling)
		{
			anim.SetBool("Attack", false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			target = collision.gameObject;
			inRange = true;
		}
	}

	private void Attack()
	{
		timer = intTimer;
		attackMode = true;
		
		anim.SetBool("Walk", false);
		anim.SetBool("Attack", true);
	}

	private void StopAttack()
	{
		cooling = false;
		attackMode = false;
		anim.SetBool("Attack", false);
	}

	private void Move()
	{
		anim.SetBool("Walk", true);
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Skell_attack"))
		{
			Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

			transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
		}
	}
	
	private void RaycastDebugger()
	{
		if(distance > attackDistance)
		{
			Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
		}
		else if (attackDistance > distance)
		{
			Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
		}
	}
}
