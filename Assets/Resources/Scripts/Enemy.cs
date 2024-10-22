using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

	[SerializeField]
	private GameObject damageText;

	// Start is called before the first frame update
	void Start()
	{
		// hpBar = FindObjectOfType<GameObject>();
		renderer_ = hpBar.GetComponent<SpriteRenderer>();
		player = FindObjectOfType<Player>();
		player2 = FindObjectOfType<Player2>();
		item = Resources.Load<GameObject>("Prefabs/Item");
		// canvas = gameObject.GetComponent<Canvas>();
		// text = canvas.GetComponent<TextMeshProUGUI>();

		if (item == null)
		{
			Debug.Log("item is null!!!");
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (GameManager.instance.isDie == false)
		{
			HpBarRender();
			Moving();
		}
	}

	void Moving()
	{
		Vector3 dir;
		if (player != null)
			dir = player.transform.position - transform.position;
		else
			dir = player2.transform.position - transform.position;
		dir = dir.normalized * moveSpeed * Time.fixedDeltaTime;
		transform.position += dir;
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
