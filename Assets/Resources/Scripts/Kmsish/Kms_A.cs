using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kms;
using UnityEngine.UI;

public class Kms_A : Kmsish
{
	// int num = 0;
	// int lv = DataManager.instance.data.kmsishLv[0];
	int P_maxATK;
	int P_MinATK;

	int upg_maxATK;
	int upg_MinATK;

	float successRate;

	private void Start()
	{
	}

	public override void initRune()
	{
		lv = DataManager.instance.data.kmsishLv[0];
		num = 0;
		P_maxATK = 1 * lv;
		P_MinATK = 1 * lv;

		upg_maxATK = 1;
		upg_MinATK = 1;

		successRate = 1f;
		for (int i = 0; i < lv; i++)
			successRate *= 0.9f;

		// Debug.Log("Kms_A init");
	}

	public override void SetText()
	{
		GameObject.FindGameObjectWithTag("RSText").GetComponent<TextMeshProUGUI>().SetText(
			"Rune - A - Common\nAttack Base Lv." + lv.ToString() +
			"\n\nMinATK + " + P_MinATK.ToString() + "\nmaxATK + " + P_maxATK.ToString()
		);
	}
	public override void upgradeSetText()
	{
		GameObject.FindGameObjectWithTag("UpgradeText").GetComponent<TextMeshProUGUI>().SetText(
			"Rune - A - Common\nAttack Base Lv." + (lv + 1).ToString() +
			"\n\nMinATK + " + (P_MinATK + upg_MinATK).ToString() + "\nmaxATK + " + (P_maxATK + upg_maxATK).ToString() +
			"\nSuccess Rate : " + (successRate * 100).ToString("0.00") + "%"
		);
	}

	public override void upgrade()
	{
		if (DataManager.instance.data.kmsishNum[0] > 0)
		{
			DataManager.instance.data.kmsishNum[0]--;
			if (Random.Range(0f, 1f) < successRate)
			{
				lv++;
				DataManager.instance.data.kmsishLv[0]++;
				successRate *= 0.9f;
				P_maxATK += upg_maxATK;
				P_MinATK += upg_MinATK;
				this.upgradeSetText();
				this.SetText();
			}
		}
	}
	public override PlayerStateDTO getState()
	{
		PlayerStateDTO dto = new PlayerStateDTO();
		dto.maxATK = P_maxATK;
		dto.minATK = P_MinATK;
		return dto;
	}
}
