using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy_Behaviour : MonoBehaviour
{
	public Transform rayCast;
	public LayerMask rayCastMask;
	public float rayCastLength;
	public float attackDistance;
	public float moveSpeed;
	public float timer;
	public float maxHealth;
	public Image healthBar;
	
	[Header("Attack")]
	public Transform attackPoint;
	public float attackRange = .5f;
	public int attackDamage;
	public LayerMask playerLayer;
	public PlayerControl targetPlayer;

	private RaycastHit2D hit;
	private Transform target;
	private Animator anim;
	private float distance;
	private bool attackMode;
	private bool inRange;
	private bool cooling;
	private float intTimer;
	private Vector2 raycastVectorRotate;
	private float currentHealth;
	private bool isDead = false;

	private void Start()
	{
		currentHealth = maxHealth;
	}

	private void Awake()
	{
		intTimer = timer;
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (!isDead)
		{
			if(inRange)
			{
				hit = Physics2D.Raycast(rayCast.position, raycastVectorRotate, rayCastLength, rayCastMask);
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
		
			healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, -100, 100);
		}else
		{
			anim.SetBool("Walk", false);
			anim.SetBool("Attack", false);
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
			Cooldown();
			anim.SetBool("Attack", false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			target = collision.transform;
			inRange = true;
			Flip();
		}
	}

	private void Flip()
	{
		Vector3 rotation = transform.eulerAngles;
		if(transform.position.x > target.position.x)
		{
			rotation.y = 180f;
			raycastVectorRotate = Vector2.left;
		}else
		{
			rotation.y = 0f;
			raycastVectorRotate = Vector2.right;
		}

		transform.eulerAngles = rotation;
	}
	

	private void Attack()
	{
		timer = intTimer;
		attackMode = true;
	
		anim.SetBool("Walk", false);
		anim.SetBool("Attack", true);
	
		Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

		foreach (Collider2D enemy in hitPlayer)
		{
			//enemy.GetComponent<Enemy_Behaviour>().TakeDamage(attackDamage);
			targetPlayer.TakeDamage(attackDamage);
		}


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
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Boss_attack"))
		{
			Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

			transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
		}
	}

	public void Cooldown()
	{
		timer -= Time.deltaTime;

		if (timer <= 0 && cooling && attackMode)
		{
			cooling = false;
			timer = intTimer;
		}
	}

	public void TriggerCooldown()
	{
		cooling = true;
	}

	private void RaycastDebugger()
	{
		if(distance > attackDistance)
		{
			Debug.DrawRay(rayCast.position, raycastVectorRotate * rayCastLength, Color.red);
		}
		else if (attackDistance > distance)
		{
			Debug.DrawRay(rayCast.position, raycastVectorRotate * rayCastLength, Color.green);
		}
	}

	public void TakeDamage(int damage, int loadScene)
	{
		currentHealth -= damage;
		
		if (currentHealth < 0)
		{
			isDead = true;
			StartCoroutine(Testingfungsi(loadScene));
		}
	}

	private void OnDrawGizmos()
	{
		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}

	public IEnumerator Testingfungsi(int scene)
	{
		yield return new WaitForSeconds(2f);
		gameObject.SetActive(false);
		SceneManager.LoadScene(scene);
	}
}
