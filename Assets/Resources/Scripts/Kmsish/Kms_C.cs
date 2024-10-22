using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kms;

public class Kms_C : Kmsish
{
	// int num = 2;
	// int lv = DataManager.instance.data.kmsishLv[2];
	float P_criticalRate;
	float upg_criticalRate;
	float successRate;

	private void Start()
	{
	}

	public override void initRune()
	{
		lv = DataManager.instance.data.kmsishLv[2];
		num = 2;
		P_criticalRate = 0.02f * lv;
		upg_criticalRate = 0.02f;

		successRate = 1f;
		for (int i = 0; i < lv; i++)
			successRate *= 0.9f;
	}

	public override void SetText()
	{
		GameObject.FindGameObjectWithTag("RSText").GetComponent<TextMeshProUGUI>().SetText(
			"Rune - C - Common\nVital point Base Lv." + lv.ToString() +
			"\ncriticalRate + " + P_criticalRate.ToString()
		);
	}
	public override void upgradeSetText()
	{
		GameObject.FindGameObjectWithTag("UpgradeText").GetComponent<TextMeshProUGUI>().SetText(
			"Rune - C - Common\nVital point Base Lv." + (lv + 1).ToString() +
			"\ncriticalRate + " + (P_criticalRate + upg_criticalRate).ToString() +
			"\nSuccess Rate : " + (successRate * 100).ToString("0.00") + "%"
		);
	}
	public override void upgrade()
	{
		if (DataManager.instance.data.kmsishNum[2] > 0)
		{
			DataManager.instance.data.kmsishNum[2]--;
			if (Random.Range(0f, 1f) < successRate)
			{
				lv++;
				DataManager.instance.data.kmsishLv[2]++;
				successRate *= 0.9f;
				P_criticalRate += upg_criticalRate;
				this.upgradeSetText();
				this.SetText();
			}
		}
	}
	public override PlayerStateDTO getState()
	{
		PlayerStateDTO dto = new PlayerStateDTO();
		dto.criticalRate = P_criticalRate;
		return dto;
	}
}
