using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kms;

public class Kms_B : Kmsish
{
	// int num = 1;
	// int lv = DataManager.instance.data.kmsishLv[1];
	int P_maxATK = 2;
	int upg_maxATK = 2;
	float successRate = 1f;

	public override void initRune()
	{
		lv = DataManager.instance.data.kmsishLv[1];
		num = 1;
		P_maxATK *= lv;

		successRate = 1f;
		for (int i = 0; i < lv; i++)
			successRate *= 0.9f;
		// Debug.Log("Kms_B init");
	}

	public override void SetText()
	{
		GameObject.FindGameObjectWithTag("RSText").GetComponent<TextMeshProUGUI>().SetText(
			"Rune - B - Common\nPower Base lv." + lv.ToString() +
			"\n\nMaxATK + " + P_maxATK.ToString()
		);
	}
	public override void upgradeSetText()
	{
		GameObject.FindGameObjectWithTag("UpgradeText").GetComponent<TextMeshProUGUI>().SetText(
			"Rune - B - Common\nPower Base lv." + (lv + 1).ToString() +
			"\n\nMaxATK + " + (P_maxATK + upg_maxATK).ToString() +
			"\nSuccess Rate : " + (successRate * 100).ToString("0.00") + "%"
		);
	}
	public override void upgrade()
	{
		if (DataManager.instance.data.kmsishNum[1] > 0)
		{
			DataManager.instance.data.kmsishNum[1]--;
			if (Random.Range(0f, 1f) < successRate)
			{
				lv++;
				DataManager.instance.data.kmsishLv[1]++;
				successRate *= 0.9f;
				P_maxATK += upg_maxATK;
				this.upgradeSetText();
				this.SetText();
			}
		}
	}
	public override PlayerStateDTO getState()
	{
		PlayerStateDTO dto = new PlayerStateDTO();
		dto.maxATK = P_maxATK;
		return dto;
	}
}
