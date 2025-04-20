using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	private float spawnCool = 3f;
	float t = 5f;
	public List<Enemy> enemies;

	[SerializeField]
	Enemy enemy;

	[SerializeField]
	Boss boss;

	// Start is called before the first frame update
	void Start()
	{
		enemies = new List<Enemy>();
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.instance.isDie != true && GameManager.instance.isBoss == false)
		{
			t += Time.deltaTime;
			if (t > spawnCool)
			{
				enemies.Add(Instantiate(enemy, transform));
				t = 0f;
			}
		}
	}

	public void makeBoss(int lv)
	{
		Boss b = Instantiate(boss, transform);
		b.setBoss(lv);
	}
}
