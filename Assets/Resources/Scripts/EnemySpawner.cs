using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	private float spawnCool = 3f;
	float t = 5f;

	[SerializeField]
	Enemy enemy;

	[SerializeField]
	Boss boss;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.instance.isDie != true && GameManager.instance.isBoss == false)
		{
			t += Time.deltaTime;
			if (t > spawnCool)
			{
				Instantiate(enemy, transform);
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
