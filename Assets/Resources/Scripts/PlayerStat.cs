using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
	[SerializeField]
	Player player;
	Player2 player2;

	[SerializeField]
	SpriteRenderer renderer_;

	[SerializeField]
	GameObject HpBar;
	[SerializeField]
	GameObject AtkBox_left;
	[SerializeField]
	GameObject AtkBox;
	[SerializeField]
	float hitForce;

	void Start()
	{
		player = FindObjectOfType<Player>();
		if (player == null)
		{
			AtkBox.SetActive(false);
			AtkBox_left.SetActive(false);
		}
		player2 = FindObjectOfType<Player2>();
		renderer_ = HpBar.GetComponent<SpriteRenderer>();

		hitForce = 300f;
	}

	// Update is called once per frame
	void Update()
	{
		if (player != null)
			transform.position = player.transform.position;
		else
			transform.position = player2.transform.position;
		float hpPer = GameManager.instance.getHpPer();
		if (hpPer > 0.8f)
		{
			// renderer.color = new Color(40, 180, 40, 255);
			renderer_.color = Color.green;
		}
		else if (hpPer > 0.25f)
		{
			renderer_.color = Color.yellow;
		}
		// else if (hpPer > 0.15f)
		// {
		// 	renderer.color = Color.magenta;
		// }
		else
		{
			renderer_.color = Color.red;
		}
		HpBar.transform.localScale = new Vector3(hpPer, 0.15f, 1);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Vector3 pos;
		if (player != null)
		{
			pos = player.transform.position;
		}
		else
		{
			pos = player2.transform.position;
		}
		if (other.gameObject.tag == "Enemy" && other.GetComponent<Enemy>().getIsHitted() == false)
		{
			Vector2 direction = other.gameObject.transform.position - pos;
			direction = direction.normalized;
			Enemy emy = other.GetComponent<Enemy>();
			DamageState DMG = GameManager.instance.calculDMG();
			if (player.isZ == true)
			{
				DMG.setDMG(5f);
			}
			emy.attacked(AtkBox, DMG);
			emy.attacked(AtkBox_left, DMG);
			emy.GetComponent<Rigidbody2D>().AddForce(direction * hitForce, ForceMode2D.Force);
		}
		else if (other.gameObject.tag == "Boss" && other.GetComponent<Boss>().getIsHitted() == false)
		{
			Vector2 direction = other.gameObject.transform.position - pos;
			direction = direction.normalized;
			Boss boss = other.GetComponent<Boss>();
			DamageState DMG = GameManager.instance.calculDMG();
			if (player.isZ == true)
			{
				DMG.setDMG(5f);
			}
			boss.attacked(AtkBox, DMG);
			boss.attacked(AtkBox_left, DMG);
			boss.GetComponent<Rigidbody2D>().AddForce(direction * hitForce, ForceMode2D.Force);
		}
	}
}
