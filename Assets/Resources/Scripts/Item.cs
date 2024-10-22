using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	int hp = 15;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public int getHp()
	{
		return hp;
	}
	public void setHp(int _hp)
	{
		hp = _hp;
	}
}
