using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kms;

public class Player2 : MonoBehaviour
{
	public Animator animator;
	[SerializeField]
	private SpriteRenderer mySpriteRenderer;

	[SerializeField]
	DamageText damageText;
	float t = 0;
	Vector3 movement;
	[SerializeField]
	private float attackDelay = 0.3f; // 0.38
	[SerializeField]
	private float attackCool = 0.5f; // 0.38
	[SerializeField]
	private float hittedCool = 0.4f; // 0.4
	[SerializeField]
	float I_Time = 0.2f; // 0.2f
	bool isHitted = false;
	[SerializeField]
	Bullet bullet;

	private float hitForce = 10f;

	void Start()
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		t = attackCool + attackDelay;
	}

	private void Update()
	{
		// why??? key input not act in fixedUpdate
		// if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -26.5f)
		// {
		// 	movement.x = -1f;
		// 	movement.y = 0f;
		// }
		// if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 26.5f)
		// {
		// 	movement.x = 1f;
		// 	movement.y = 0f;
		// }
		// if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < 13.5f)
		// {
		// 	movement.y = 1f;
		// 	movement.x = 0f;
		// }
		// if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > -13.5f)
		// {
		// 	movement.y = -1f;
		// 	movement.x = 0f;
		// }
		// if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
		// {
		// 	movement.x = Input.GetAxisRaw("Horizontal");
		// 	movement.y = Input.GetAxisRaw("Vertical");
		// }
		// if (transform.position.x < -26f && movement.x < 0)
		// 	movement.x = 0;
		// else if (transform.position.x > 26f && movement.x > 0)
		// 	movement.x = 0;
		// if (transform.position.y < -13.5f && movement.y < 0)
		// 	movement.y = 0;
		// else if (transform.position.y > 14f && movement.y > 0)
		// 	movement.y = 0;

		movement = GameManager.instance.movement; // break normalize?
		if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
			movement.x = 0;
		else
			movement.y = 0;
		movement.Normalize();
		if (Input.GetKey(KeyCode.Space))
		{
			Attack(1);
		}
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		if (GameManager.instance.isDie == false)
		{
			if (movement.sqrMagnitude > 0)
				animator.SetBool("isMove", true);
			else
				animator.SetBool("isMove", false);

			animator.SetFloat("Horizontal", movement.x);
			animator.SetFloat("Vertical", movement.y);
			if (movement.x > 0)
			{
				animator.SetFloat("LR_", 1f);
				animator.SetFloat("BF_", 0f);
			}
			else if (movement.x < 0)
			{
				animator.SetFloat("LR_", -1f);
				animator.SetFloat("BF_", 0f);
			}
			else if (movement.y > 0)
			{
				animator.SetFloat("BF_", 1f);
				animator.SetFloat("LR_", 0f);
			}
			else if (movement.y < 0)
			{
				animator.SetFloat("BF_", -1f);
				animator.SetFloat("LR_", 0f);
			}

			transform.position += movement * GameManager.instance.getMoveSpeed() * Time.fixedDeltaTime;
		}
	}

	public void Attack(int atkCount)
	{
		if (t >= attackDelay + attackCool && animator.GetBool("isHitted") == false && animator.GetBool("isAttack") == false)
			StartCoroutine("CoIsAttack", atkCount);
	}

	public void attacked(GameObject other)
	{
		if (other.gameObject.tag == "Enemy" && isHitted == false)
		{
			StartCoroutine("CoI_Time");

			Vector2 direction = other.gameObject.transform.position - transform.position;
			direction = direction.normalized;
			other.GetComponent<Enemy>().GetComponent<Rigidbody2D>().AddForce(direction * hitForce, ForceMode2D.Force);

			float damage = other.GetComponent<Enemy>().getATK();
			StopCoroutine("CoHitted");
			StartCoroutine("CoHitted", damage);
		}
		else if (other.gameObject.tag == "Boss" && isHitted == false)
		{
			StartCoroutine("CoI_Time");

			Vector2 direction = other.gameObject.transform.position - transform.position;
			direction = direction.normalized;
			other.GetComponent<Boss>().GetComponent<Rigidbody2D>().AddForce(direction * hitForce, ForceMode2D.Force);

			float damage = other.GetComponent<Boss>().getATK();
			StopCoroutine("CoHitted");
			StartCoroutine("CoHitted", damage);
		}
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Item")
		{
			// Debug.Log("Hp Up!!!");
			int heal = other.GetComponent<Item>().getHp();
			GameManager.instance.hpUp(heal);
			DamageText dmgText = Instantiate(damageText).GetComponent<DamageText>();
			dmgText.transform.position = transform.position;
			dmgText.damage = heal; // 데미지 전달
			dmgText.setHeal();
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "Rune")
		{
			int runeNum = other.GetComponent<spawnedKmsish>().getIdx();
			DataManager.instance.data.kmsishNum[runeNum]++;
			Destroy(other.gameObject);
		}
	}

	public void Zskill()
	{
		StopCoroutine("CoIsAttack");
		StartCoroutine("CoIsAttack", 3);
	}

	public IEnumerator CoIsAttack(int skill)
	{
		animator.SetBool("isAttack", true);
		yield return new WaitForSeconds(attackDelay);
		float LR = animator.GetFloat("LR_");
		float BF = animator.GetFloat("BF_");
		if (LR == -1f)
		{
			for (int i = 0; i < skill; i++)
			{
				Bullet bll = Instantiate(bullet).GetComponent<Bullet>();
				bll.transform.position = transform.position;
				bll.setDir(9);
				yield return new WaitForSeconds(0.15f);
			}
		}
		else if (LR == 1f)
		{
			for (int i = 0; i < skill; i++)
			{
				Bullet bll = Instantiate(bullet).GetComponent<Bullet>();
				bll.transform.position = transform.position;
				bll.setDir(3);
				yield return new WaitForSeconds(0.15f);
			}
		}
		else if (BF == -1f)
		{
			for (int i = 0; i < skill; i++)
			{
				Bullet bll = Instantiate(bullet).GetComponent<Bullet>();
				bll.transform.position = transform.position;
				bll.setDir(6);
				yield return new WaitForSeconds(0.15f);
			}
		}
		else if (BF == 1f)
		{
			for (int i = 0; i < skill; i++)
			{
				Bullet bll = Instantiate(bullet).GetComponent<Bullet>();
				bll.transform.position = transform.position;
				bll.setDir(0);
				yield return new WaitForSeconds(0.15f);
			}
		}
		yield return new WaitForSeconds(attackCool - skill * 0.15f + 0.15f);
		animator.SetBool("isAttack", false);
	}
	IEnumerator CoHitted(int damage)
	{
		StopCoroutine("CoIsAttack"); // cencel Attack
		animator.SetBool("isAttack", false);

		GameManager.instance.damaged(damage);
		DamageText dmgText = Instantiate(damageText).GetComponent<DamageText>();
		dmgText.transform.position = transform.position;
		// dmgText.setPosition(Camera.main.WorldToScreenPoint(transform.position)); // 표시될 위치
		dmgText.damage = damage; // 데미지 전달
								 // StartCoroutine("CoDmgPanel", damage);

		animator.SetBool("isHitted", true);
		if (GameManager.instance.checkIsDie() == true)
		{
			// animator.SetBool("isDead", true);
			StopCoroutine("CoHitted"); // cencel Hitted

			animator.SetBool("isHitted", false);
			StartCoroutine("CoIsDead");
		}
		yield return new WaitForSeconds(hittedCool);
		animator.SetBool("isHitted", false);
	}
	IEnumerator CoI_Time()
	{
		isHitted = true;
		yield return new WaitForSeconds(I_Time);
		isHitted = false;
	}
	IEnumerator CoIsDead()
	{
		// animator.Play("Dead");
		yield return new WaitForSeconds(2f);
		GameManager.instance.makeBackToMenuPanel();
		// animator.SetFloat("isDead", 1f);
		animator.StartPlayback();
	}
}
