using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	float moveSpeed = 8f;
	Vector3 movement;

	private void Start()
	{
		Destroy(gameObject, 1f);
	}

	public void setDir(int dir)
	{
		switch (dir)
		{
			case 0:
				movement.y = 1;
				break;
			case 3:
				movement.x = 1;
				break;
			case 6:
				movement.y = -1;
				break;
			case 9:
				movement.x = -1;
				break;
			default:
				break;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		transform.position += movement * moveSpeed * Time.fixedDeltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		DamageState DMG = GameManager.instance.calculDMG();
		if (other.gameObject.tag == "Boss")
		{
			other.GetComponent<Boss>().attacked(gameObject, DMG);
		}
		else if (other.gameObject.tag == "Enemy")
		{
			other.GetComponent<Enemy>().attacked(gameObject, DMG);
		}
	}
}
