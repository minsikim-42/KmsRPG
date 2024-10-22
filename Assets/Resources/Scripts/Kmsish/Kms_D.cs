using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kms;

public class Kms_D : Kmsish
{
	// int num = 3;
	// int lv = DataManager.instance.data.kmsishLv[3];
	int P_maxHp;
	float P_moveSpeed;
	int upg_maxHp;
	float upg_moveSpeed;
	float successRate;

	private void Awake()
	{
	}

	public override void initRune()
	{
		lv = DataManager.instance.data.kmsishLv[3];
		num = 3;
		P_maxHp = 50 * lv;
		P_moveSpeed = 0.05f * lv;
		upg_maxHp = 50;
		upg_moveSpeed = 0.05f;
		successRate = 1f;
		for (int i = 0; i < lv; i++)
			successRate *= 0.9f;
	}

	public override void SetText()
	{
		GameObject.FindGameObjectWithTag("RSText").GetComponent<TextMeshProUGUI>().SetText(
			"Rune - D - Common\n Health Base Lv." + lv.ToString() +
			"\n\nmaxHp + " + P_maxHp.ToString() +
			"\n\nMove Speed + " + P_moveSpeed.ToString()
		);
	}
	public override void upgradeSetText()
	{
		GameObject.FindGameObjectWithTag("UpgradeText").GetComponent<TextMeshProUGUI>().SetText(
			"Rune - D - Common\n Health Base Lv." + lv.ToString() +
			"\n\nmaxHp + " + (P_maxHp + upg_maxHp).ToString() +
			"\n\nMove Speed + " + (P_moveSpeed + upg_moveSpeed).ToString() +
			"\nSuccess Rate : " + (successRate * 100).ToString("0.00") + "%"
		);
	}
	public override void upgrade()
	{
		if (DataManager.instance.data.kmsishNum[3] > 0)
		{
			DataManager.instance.data.kmsishNum[3]--;
			if (Random.Range(0f, 1f) < successRate)
			{
				lv++;
				DataManager.instance.data.kmsishLv[3]++;
				successRate *= 0.9f;
				P_maxHp += upg_maxHp;
				P_moveSpeed += upg_moveSpeed;
				this.upgradeSetText();
				this.SetText();
			}
		}
	}
	public override PlayerStateDTO getState()
	{
		PlayerStateDTO dto = new PlayerStateDTO();
		dto.maxHp = P_maxHp;
		dto.moveSpeed = P_moveSpeed;
		return dto;
	}
}
