using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
	[SerializeField] Player player;
	[SerializeField] Player2 player2;

	GameObject item;

	Collider2D hitBox;
	int Hp = 250;
	int MaxHp = 250;
	int ATK = 15;
	float hitForce = 200f;
	float moveSpeed = 1.5f; // 1.5f
	float I_Time = 0.2f; // 0.2f
	bool isHitted = false;
	int giveExp = 10;

	[SerializeField]
	GameObject hpBar;
	SpriteRenderer renderer_;
	public Rigidbody2D rb;
	public Vector3 dir;
	public float nextUpdateTime;

	[SerializeField]
	private GameObject damageText;

	// Start is called before the first frame update
	void Awake()
	{
		player = GameManager.instance.player;
		// hpBar = FindObjectOfType<GameObject>();
		renderer_ = hpBar.GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		nextUpdateTime = Time.time;
		// player = FindObjectOfType<Player>();
		// player2 = FindObjectOfType<Player2>();
		// item = Resources.Load<GameObject>("Prefabs/Item");
		// canvas = gameObject.GetComponent<Canvas>();
		// text = canvas.GetComponent<TextMeshProUGUI>();
		//
		// if (item == null)
		// {
		// 	Debug.Log("item is null!!!");
		// }
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if (GameManager.instance.isDie == false)
		{
			// HpBarRender();
			Moving();
		}
	}

	// private void Update()
	// {
	// 	AABB(); // AABB
	// }
	//
	//
	// void AABB()
	// {
	// 	float sx = transform.position.x - 0.2f;
	// 	float sy = transform.position.y - 0.2f;
	// 	float ex = transform.position.x + 0.2f;
	// 	float ey = transform.position.y + 0.2f;
	// 	
	// 	float psx = player.transform.position.x - 0.25f;
	// 	float psy = player.transform.position.y - 0.5f;
	// 	float pex = player.transform.position.x + 0.25f;
	// 	float pey = player.transform.position.y + 0.5f;
	//
	// 	if (sx < pex && ex > psx && sy < pey && ey > psy)
	// 	{
	// 		player.attacked(gameObject);
	// 	}
	// }
	
	void Moving()
	{
		if (Time.time < nextUpdateTime)
		{
			rb.MovePosition(rb.position + (Vector2)dir * (moveSpeed * Time.fixedDeltaTime));
			return;
		}
		nextUpdateTime += 2f;
		Vector2 target = GameManager.instance.playerPos;
		dir = (player.transform.position - transform.position).normalized;
		rb.MovePosition(rb.position + (Vector2)dir * (moveSpeed * Time.fixedDeltaTime));
		// // if (player != null)
		// // 	dir = player.transform.position - transform.position;
		// // else
		// // 	dir = player2.transform.position - transform.position;
		// transform.position += dir * (moveSpeed * Time.fixedDeltaTime);
	}
	void HpBarRender()
	{
		float hpPer = (float)Hp / MaxHp;
		hpBar.transform.localScale = new Vector3(hpPer, 0.15f, 1);
		if (hpPer > 0.8f)
		{
			renderer_.color = Color.green;
		}
		else if (hpPer > 0.25f)
		{
			renderer_.color = Color.yellow;
		}
		else
		{
			renderer_.color = Color.red;
		}
	}
	public void attacked(GameObject other, DamageState DMG)
	{
		// Debug.Log("enemy attacked-" + other.gameObject.tag);
		if ((other.gameObject.tag == "AttackBox" || other.gameObject.tag == "AttackBox_left" || other.gameObject.tag == "Bullet") && isHitted == false)
		{
			StartCoroutine("CoI_Time");
	
			Hp -= DMG.getDMG();
	
			DamageText dmgText = Instantiate(damageText).GetComponent<DamageText>();
			dmgText.transform.position = transform.position;
			dmgText.damage = DMG.getDMG(); // 데미지 전달
			if (DMG.getIsCri())
				dmgText.setCri();
	
			if (Hp < 0f)
			{
				GameManager.instance.earnExp(giveExp);
				if (Random.Range(0f, 1f) < 0.5f) // HPup item
				{
					Instantiate(item, transform.position + (Vector3.right / 4), Quaternion.identity);
				}
	
				if (Random.Range(0f, 1f) < 0.1f) // 10% Rune
				{
					int idx = Random.Range(0, 4);
					GameObject inst = Instantiate(GameManager.instance.KmsRunePrefabs[idx], transform.position + (Vector3.left / 4), Quaternion.identity);
					inst.GetComponent<spawnedKmsish>().setIdx(idx);
				}
				else if (Random.Range(0f, 1f) <= 0.01f) // 1% epic
				{
					int idx = Random.Range(4, 5);
					GameObject inst = Instantiate(GameManager.instance.KmsRunePrefabs[idx], transform.position + (Vector3.left / 4), Quaternion.identity);
					inst.GetComponent<spawnedKmsish>().setIdx(idx);
				}
				else if (Random.Range(0f, 1f) <= 0.001f) // 0.1% legend
				{
					GameObject inst = Instantiate(GameManager.instance.KmsRunePrefabs[5], transform.position + (Vector3.left / 4), Quaternion.identity);
					inst.GetComponent<spawnedKmsish>().setIdx(5);
				}
				Destroy(gameObject);
			}
		}
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Vector2 direction = other.gameObject.transform.position - transform.position;
			direction = direction.normalized;
			player.GetComponent<Rigidbody2D>().AddForce(direction * hitForce, ForceMode2D.Force);
			player.attacked(gameObject);
		}
		else if (other.gameObject.tag == "Player2")
		{
			Vector2 direction = other.gameObject.transform.position - transform.position;
			direction = direction.normalized;
			player2.GetComponent<Rigidbody2D>().AddForce(direction * hitForce, ForceMode2D.Force);
			player2.attacked(gameObject);
		}
	}
	IEnumerator CoI_Time()
	{
		isHitted = true;
		yield return new WaitForSeconds(I_Time);
		isHitted = false;
	}

	public float getATK()
	{
		return ATK;
	}
	public bool getIsHitted()
	{
		return isHitted;
	}
	public int getExp()
	{
		return giveExp;
	}
}
