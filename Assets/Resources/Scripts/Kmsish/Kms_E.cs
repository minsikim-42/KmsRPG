using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kms;

public class Kms_E : Kmsish
{
	// int num = 4;
	// int lv = DataManager.instance.data.kmsishLv[4];
	int P_maxATK;
	int P_minATK;
	int P_maxHp;
	float P_criticalRate;

	int upg_maxATK;
	int upg_minATK;
	int upg_maxHp;
	float upg_criticalRate;
	float successRate;

	private void Awake()
	{
	}

	public override void initRune()
	{
		lv = DataManager.instance.data.kmsishLv[4];
		num = 4;
		P_maxATK = 2 * lv;
		P_minATK = 1 * lv;
		P_maxHp = 30 * lv;
		P_criticalRate = 0.07f * lv;

		upg_maxATK = 2;
		upg_minATK = 1;
		upg_maxHp = 30;
		upg_criticalRate = 0.07f;
		successRate = 1f;
		for (int i = 0; i < lv; i++)
			successRate *= 0.9f;
	}

	public override void SetText()
	{
		GameObject.FindGameObjectWithTag("RSText").GetComponent<TextMeshProUGUI>().SetText(
			"<color=\"purple\">Rune - E - Epic\n Base Master Lv." + lv.ToString() +
			"</color>\n\nmaxATK + " + P_maxATK.ToString() +
			"\nminATK + " + P_minATK.ToString() +
			"\nmaxHp + " + P_maxHp.ToString() +
			"\ncriticalRate + " + P_criticalRate.ToString("0.00")
		);
	}
	public override void upgradeSetText()
	{
		GameObject.FindGameObjectWithTag("UpgradeText").GetComponent<TextMeshProUGUI>().SetText(
			"<color=\"purple\">Rune - E - Epic\n Base Master Lv." + lv.ToString() +
			"</color>\n\nmaxATK + " + (P_maxATK + upg_maxATK).ToString() +
			"\nminATK + " + (P_minATK + upg_minATK).ToString() +
			"\nmaxHp + " + (P_maxHp + upg_maxHp).ToString() +
			"\ncriticalRate + " + (P_criticalRate + upg_criticalRate).ToString("0.00") +
			"\nSuccess Rate : " + (successRate * 100).ToString("0.00") + "%"
		);
	}
	public override void upgrade()
	{
		if (DataManager.instance.data.kmsishNum[4] > 0)
		{
			DataManager.instance.data.kmsishNum[4]--;
			if (Random.Range(0f, 1f) < successRate)
			{
				lv++;
				DataManager.instance.data.kmsishLv[4]++;
				successRate *= 0.9f;
				P_maxATK += upg_maxATK;
				P_minATK += upg_minATK;
				P_maxHp += upg_maxHp;
				P_criticalRate += upg_criticalRate;
				this.upgradeSetText();
				this.SetText();
			}
		}
	}
	public override PlayerStateDTO getState()
	{
		PlayerStateDTO dto = new PlayerStateDTO();
		dto.maxATK = P_maxATK;
		dto.minATK = P_minATK;
		dto.maxHp = P_maxHp;
		dto.criticalRate = P_criticalRate;
		return dto;
	}
}
