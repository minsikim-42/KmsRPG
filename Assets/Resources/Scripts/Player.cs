using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kms;

public class Player : MonoBehaviour
{
	public Animator animator;
	public Rigidbody2D rb;

	// private Vector3 GameManager.instance.movement;

	[SerializeField]
	private SpriteRenderer mySpriteRenderer;

	[SerializeField]
	private Collider2D hitBox;
	[SerializeField]
	private GameObject damageText;

	// float ATK = 9f;

	[SerializeField]
	private float attackDelay = 0.38f; // 0.38
	[SerializeField]
	private float attackCool = 0.38f; // 0.38
	[SerializeField]
	private float hittedCool = 0.4f; // 0.4
	[SerializeField]
	float I_Time = 0.2f; // 0.2f
	bool isHitted = false;

	private float t = 0f;

	public bool isZ = false;

	[SerializeField]
	private float hitForce = 10f;

	[SerializeField]
	private Collider2D leftHitBox;

	[SerializeField]
	private Collider2D rightHitBox;

	// Start is called before the first frame update
	void Start()
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		leftHitBox.enabled = false;
		rightHitBox.enabled = false;

		// isHittedCool = 12 * Time.fixedDeltaTime; // frameCut * frame * deltaTime???
		// isAttackDelay = 12 * Time.fixedDeltaTime; // 0.38
		// isAttackCool = 24 * Time.fixedDeltaTime; // 0.38
		t = attackCool + attackDelay;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (GameManager.instance.isDie == false)
		{
			// rb.MovePosition(rb.position + GameManager.instance.movement * GameManager.instance.moveSpeed * Time.fixedDeltaTime);
		}
		if (GameManager.instance.isDie == false && GameManager.instance.isKeyButtonPressed == false)
		{
			t += Time.deltaTime;

			if (Input.GetKey(KeyCode.Space))
			{
				Attack();
			}
		}
		animator.SetFloat("Speed", GameManager.instance.movement.sqrMagnitude);
		if (GameManager.instance.movement.x > 0)
		{
			animator.SetFloat("Horizontal", 1f);
			animator.SetFloat("RL_", 1f);
		}
		else if (GameManager.instance.movement.x < 0)
		{
			animator.SetFloat("Horizontal", -1f);
			animator.SetFloat("RL_", -1f);
		}
		else
		{
			animator.SetFloat("Horizontal", 0f);
		}
		transform.position += GameManager.instance.movement * GameManager.instance.getMoveSpeed() * Time.fixedDeltaTime;
		// else
		// {
		// 	// StartCoroutine("CoIsDead");
		// 	rb.totalForce = new Vector2(0, 0);
		// 	rb.velocity = new Vector2(0, 0);
		// }
	}

	public void Attack()
	{
		if (t >= attackDelay + attackCool && animator.GetFloat("isHitted") < 0.5f && animator.GetFloat("isAttack") < 0.5f)
			StartCoroutine("CoIsAttack");
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
		mySpriteRenderer.color = Color.blue;
		isZ = true;
		StopCoroutine("CoHitted");
		animator.SetFloat("isHitted", 0f);

		StopCoroutine("CoIsAttack");
		animator.SetFloat("isAttack", 0f); // skill motion strenth..
		leftHitBox.enabled = false;
		rightHitBox.enabled = false;
		StartCoroutine("WaitAndAttack", 0.01f);
		// StartCoroutine("CoIsAttack");
	}

	IEnumerator WaitAndAttack(float t)
	{ // for stop AttackMotion
		yield return new WaitForSeconds(t);
		StartCoroutine("CoIsAttack");
	}

	public IEnumerator CoIsAttack()
	{
		animator.SetFloat("isAttack", 1f);
		yield return new WaitForSeconds(attackDelay);
		if (animator.GetFloat("RL_") == -1f)
			leftHitBox.enabled = true;
		else
			rightHitBox.enabled = true;
		yield return new WaitForSeconds(attackCool);
		leftHitBox.enabled = false;
		rightHitBox.enabled = false;
		animator.SetFloat("isAttack", 0f);
		mySpriteRenderer.color = Color.white;
		isZ = false;
	}

	IEnumerator CoHitted(int damage)
	{
		GameManager.instance.damaged(damage);
		DamageText dmgText = Instantiate(damageText).GetComponent<DamageText>();
		dmgText.transform.position = transform.position;
		// dmgText.setPosition(Camera.main.WorldToScreenPoint(transform.position)); // 표시될 위치
		dmgText.damage = damage; // 데미지 전달
								 // StartCoroutine("CoDmgPanel", damage);

		if (isZ == false)
		{
			StopCoroutine("CoIsAttack"); // cencel Attack
			leftHitBox.enabled = false;
			rightHitBox.enabled = false;
			animator.SetFloat("isAttack", 0f);

			animator.SetFloat("isHitted", 1f);
			if (GameManager.instance.checkIsDie() == true)
			{
				animator.SetFloat("isDead", 1f);
				StopCoroutine("CoHitted"); // cencel Hitted
				animator.SetFloat("isHitted", 0f);

				StartCoroutine("CoIsDead");
			}
			yield return new WaitForSeconds(hittedCool);
			animator.SetFloat("isHitted", 0f);
		}
		if (GameManager.instance.checkIsDie() == true)
		{
			animator.SetFloat("isDead", 1f);
			StartCoroutine("CoIsDead");
		}
	}
	IEnumerator CoI_Time()
	{
		isHitted = true;
		yield return new WaitForSeconds(I_Time);
		isHitted = false;
	}

	IEnumerator CoIsDead()
	{
		animator.Play("Dead");
		Vector2 down = new Vector2(0, -0.5f);
		rb.MovePosition(rb.position + down);
		// animator.StopPlayback();
		yield return new WaitForSeconds(2f);
		GameManager.instance.makeBackToMenuPanel();
		animator.SetFloat("isDead", 1f);
		animator.StartPlayback();
	}
	// IEnumerator CoDmgPanel(float atk)
	// {
	// 	damageText.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(transform.position);
	// 	// text.rectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
	// 	damageText.SetText(atk.ToString());
	// 	yield return new WaitForSeconds(0.7f);
	// 	damageText.SetText("");
	// }

	public bool getIsHitted()
	{
		return isHitted;
	}

	// public void SetActive(bool TF) {
	// 	gameObject.SetActive(TF);
	// }

	public void KeyButtonPressed(int clkDir)
	{
		GameManager.instance.isKeyButtonPressed = true;
		switch (clkDir)
		{
			case 12:
				GameManager.instance.movement.y = 1;
				break;
			case 3:
				GameManager.instance.movement.x = 1;
				break;
			case 6:
				GameManager.instance.movement.y = -1;
				break;
			case 9:
				GameManager.instance.movement.x = -1;
				break;
			default:
				break;
		}


		// check
		if (transform.position.x < -26f && GameManager.instance.movement.x < 0)
			GameManager.instance.movement.x = 0;
		else if (transform.position.x > 26f && GameManager.instance.movement.x > 0)
			GameManager.instance.movement.x = 0;
		if (transform.position.y < -13.5f && GameManager.instance.movement.y < 0)
			GameManager.instance.movement.y = 0;
		else if (transform.position.y > 14f && GameManager.instance.movement.y > 0)
			GameManager.instance.movement.y = 0;

		GameManager.instance.movement = GameManager.instance.movement.normalized;
	}
	public void KeyButtonDepressed(int clkDir)
	{
		GameManager.instance.isKeyButtonPressed = false;
		switch (clkDir)
		{
			case 12:
				GameManager.instance.movement.y = 0;
				break;
			case 3:
				GameManager.instance.movement.x = 0;
				break;
			case 6:
				GameManager.instance.movement.y = 0;
				break;
			case 9:
				GameManager.instance.movement.x = 0;
				break;
			default:
				break;
		}
	}
}
