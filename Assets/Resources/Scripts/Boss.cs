using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss : MonoBehaviour
{
	[SerializeField]
	Player player;
	Player2 player2;

	GameObject item;

	Collider2D hitBox;
	int Hp = 500;
	int MaxHp = 500;
	int ATK = 25;
	float hitForce = 200f;
	float moveSpeed = 1f;
	float I_Time = 0.1f;
	bool isHitted = false;
	int giveExp = 50;

	[SerializeField]
	GameObject hpBar;
	SpriteRenderer renderer_;

	[SerializeField]
	private GameObject damageText;

	// Start is called before the first frame update
	void Start()
	{
		renderer_ = hpBar.GetComponent<SpriteRenderer>();
		player = FindObjectOfType<Player>();
		player2 = FindObjectOfType<Player2>();
		item = Resources.Load<GameObject>("Prefabs/Item");

		if (item == null)
		{
			Debug.Log("item is null!!!");
		}
	}

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
		float hpPer = 2f * Hp / MaxHp;
		hpBar.transform.localScale = new Vector3(hpPer, 0.2f, 1);
		if (hpPer > 1.6f)
		{
			renderer_.color = Color.green;
		}
		else if (hpPer > 0.5f)
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
			dmgText.transform.position = transform.position + Vector3.up / 2f;
			dmgText.damage = DMG.getDMG(); // 데미지 전달
			if (DMG.getIsCri())
				dmgText.setCri();

			if (Hp < 0f)
			{
				GameManager.instance.isBoss = false;

				GameManager.instance.earnExp(giveExp);
				if (Random.Range(0f, 1f) < 0.5f) // HPup item
				{
					GameObject it = Instantiate(item, transform.position + (Vector3.right / 4), Quaternion.identity);
					it.GetComponent<Item>().setHp(50);
				}

				if (Random.Range(0f, 1f) < 0.5f) // 50% Rune
				{
					int idx = Random.Range(0, 4);
					GameObject inst = Instantiate(GameManager.instance.KmsRunePrefabs[idx], transform.position + (Vector3.left / 4), Quaternion.identity);
					inst.GetComponent<spawnedKmsish>().setIdx(idx);
				}
				else if (Random.Range(0f, 1f) < 0.1f) // 10% epic
				{
					int idx = Random.Range(4, 5);
					GameObject inst = Instantiate(GameManager.instance.KmsRunePrefabs[idx], transform.position + (Vector3.left / 4), Quaternion.identity);
					inst.GetComponent<spawnedKmsish>().setIdx(idx);
				}
				else if (Random.Range(0f, 1f) < 0.01f) // 1% legend
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
	public void setBoss(int lv)
	{
		MaxHp *= lv;
		Hp *= lv;
		ATK += 20 * lv;
		hitForce += 20 * lv;
		moveSpeed += 0.01f * lv;
		giveExp *= lv;
	}
}
