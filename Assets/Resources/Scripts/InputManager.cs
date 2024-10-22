using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[SerializeField] Player player0;
	[SerializeField] Player2 player2;

	Vector3 position;

	[SerializeField] GameObject left;
	[SerializeField] GameObject right;
	[SerializeField] GameObject up;
	[SerializeField] GameObject down;
	[SerializeField] GameObject attack;
	[SerializeField] GameObject skill;
	[SerializeField] GameObject joystick;
	Vector3 touchedPos;
	int fingerId;
	bool joy;
	bool isTouch;
	bool isLeft;
	bool isRight;
	bool isUp;
	bool isDown;

	bool isAttack;
	bool isSkill;
	void Start()
	{
		joy = DataManager.instance.data.isJoy;
		if (joy == true)
		{
			joystick.SetActive(true);

			left.SetActive(false);
			right.SetActive(false);
			up.SetActive(false);
			down.SetActive(false);
		}
		else
		{
			joystick.SetActive(false);
			left.SetActive(true);
			right.SetActive(true);
			up.SetActive(true);
			down.SetActive(true);
		}
		// tempTouch = new Touch[2];
		fingerId = -1;
		// left.transform.localScale = new Vector3(100, 100, 0);

		Debug.Log(left.GetComponent<SpriteRenderer>().bounds.center);
		Debug.Log(left.GetComponent<SpriteRenderer>().bounds.extents);
		Debug.Log(left.GetComponent<SpriteRenderer>().bounds.size);
	}

	// Update is called once per frame
	void Update()
	{
		isLeft = false; isRight = false; isUp = false; isDown = false; isAttack = false; isSkill = false;
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);

			if (joy == true)
			{
				switch (touch.phase)
				{
					case TouchPhase.Began: // at first
						if (joystick.GetComponent<SpriteRenderer>().bounds.Contains(touch.position) &&
							fingerId == -1)
						{
							touchedPos = touch.position;
							fingerId = touch.fingerId;
						}
						break;
					case TouchPhase.Moved: // moving
						if (fingerId == touch.fingerId)
						{
							Vector3 endPos = touch.position;
							Vector3 delta = (endPos - touchedPos).normalized;
							GameManager.instance.movement.x = delta.x;
							GameManager.instance.movement.y = delta.y;
						}
						break;
					case TouchPhase.Ended:
						if (fingerId == touch.fingerId)
						{
							GameManager.instance.movement.x = 0;
							GameManager.instance.movement.y = 0;
							fingerId = -1;
						}
						break;
				}
			}
			else if (joy == false)
			{
				isLeft |= left.GetComponent<SpriteRenderer>().bounds.Contains(touch.position);
				isRight |= right.GetComponent<SpriteRenderer>().bounds.Contains(touch.position);
				isUp |= up.GetComponent<SpriteRenderer>().bounds.Contains(touch.position);
				isDown |= down.GetComponent<SpriteRenderer>().bounds.Contains(touch.position);
			}
			isAttack |= attack.GetComponent<SpriteRenderer>().bounds.Contains(touch.position);
			isSkill |= skill.GetComponent<SpriteRenderer>().bounds.Contains(touch.position);
		}

		if (joy == false)
		{
			// touch pos update
			// touchedPos = Camera.main.ScreenToWorldPoint(tempTouch.position);

			if (isLeft || isRight)
			{
				if (isLeft)
					GameManager.instance.movement.x = -1;
				else if (isRight)
					GameManager.instance.movement.x = 1;
			}
			else
				GameManager.instance.movement.x = 0;
			if (isUp || isDown)
			{
				if (isUp)
					GameManager.instance.movement.y = 1;
				else if (isDown)
					GameManager.instance.movement.y = -1;
			}
			else
				GameManager.instance.movement.y = 0;
		}
		if (isAttack)
		{
			GameManager.instance.AttackButtonPressed();
		}
		else if (isSkill)
		{
			GameManager.instance.Skill();
		}


		// if (isTouch == false)
		// {
		// 	GameManager.instance.movement.x = Input.GetAxisRaw("Horizontal"); // player1
		// 	GameManager.instance.movement.y = Input.GetAxisRaw("Vertical");
		// }

		if (GameManager.instance.PlayerNum == 0)
			position = player0.transform.position;
		else
			position = player2.transform.position;

		if (position.x < -26f && GameManager.instance.movement.x < 0)
			GameManager.instance.movement.x = 0;
		else if (position.x > 26f && GameManager.instance.movement.x > 0)
			GameManager.instance.movement.x = 0;

		if (position.y < -13.5f && GameManager.instance.movement.y < 0)
			GameManager.instance.movement.y = 0;
		else if (position.y > 14f && GameManager.instance.movement.y > 0)
			GameManager.instance.movement.y = 0;

		// Debug.Log(GameManager.instance.movement);
		GameManager.instance.movement = GameManager.instance.movement.normalized;
	}
}
