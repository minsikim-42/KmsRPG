using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

	// [SerializeField]
	Player player;
	// [SerializeField]
	Player2 player2;
	// Start is called before the first frame update
	void Start()
	{
		player = FindObjectOfType<Player>();
		player2 = FindObjectOfType<Player2>();
	}

	// Update is called once per frame
	void Update()
	{
		if (player != null) // player1
		{
			if ((-17.5 > player.transform.position.x || player.transform.position.x > 17.5) &&
					(-9.5 > player.transform.position.y || player.transform.position.y > 9.5))
				return;
			else if (-17.5 > player.transform.position.x || player.transform.position.x > 17.5)
			{
				transform.position = new Vector3(transform.position.x, player.transform.position.y, -10);
			}
			else if (-9.5 > player.transform.position.y || player.transform.position.y > 9.5)
			{
				transform.position = new Vector3(player.transform.position.x, transform.position.y, -10);
			}
			else
				transform.position = player.transform.position + Vector3.back * 10;
		}
		else // player2
		{
			if ((-17.5 > player2.transform.position.x || player2.transform.position.x > 17.5) &&
					(-9.5 > player2.transform.position.y || player2.transform.position.y > 9.5))
				return;
			else if (-17.5 > player2.transform.position.x || player2.transform.position.x > 17.5)
			{
				transform.position = new Vector3(transform.position.x, player2.transform.position.y, -10);
			}
			else if (-9.5 > player2.transform.position.y || player2.transform.position.y > 9.5)
			{
				transform.position = new Vector3(player2.transform.position.x, transform.position.y, -10);
			}
			else
				transform.position = player2.transform.position + Vector3.back * 10;
		}
	}
}
