using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kms;

public class Kms_F : Kmsish
{
	// int num = 5;
	// int lv = DataManager.instance.data.kmsishLv[5];
	int P_maxATK;
	int P_minATK;
	int P_maxHp;
	float P_criticalRate;
	float P_moveSpeed;

	int upg_maxATK;
	int upg_minATK;
	int upg_maxHp;
	float upg_criticalRate;
	float upg_moveSpeed;

	float successRate;

	private void Awake()
	{
	}

	public override void initRune()
	{
		lv = DataManager.instance.data.kmsishLv[5];
		num = 5;
		P_maxATK = 30 * lv;
		P_minATK = 30 * lv;
		P_maxHp = 100 * lv;
		P_criticalRate = 0.1f * lv;
		P_moveSpeed = 0.5f * lv;

		upg_maxATK = 30;
		upg_minATK = 30;
		upg_maxHp = 100;
		upg_criticalRate = 0.1f;
		upg_moveSpeed = 0.5f;

		successRate = 0.5f;
	}

	public override void SetText()
	{
		GameObject.FindGameObjectWithTag("RSText").GetComponent<TextMeshProUGUI>().SetText(
			"<color=\"yellow\">Rune - F - Legend\n My Grade Lv." + lv.ToString() +
			"</color>\n\nmaxATK + " + P_maxATK.ToString() +
			"\nminATK + " + P_minATK.ToString() +
			"\nmaxHp + " + P_maxHp.ToString() +
			"\ncriticalRate + " + P_criticalRate.ToString("0.00") +
			"\nMove Speed + " + P_moveSpeed.ToString()
		);
	}
	public override void upgradeSetText()
	{
		GameObject.FindGameObjectWithTag("UpgradeText").GetComponent<TextMeshProUGUI>().SetText(
			"<color=\"yellow\">Rune - F - Legend\n My Grade Lv." + lv.ToString() +
			"</color>\n\nmaxATK + " + (P_maxATK + upg_maxATK).ToString() +
			"\nminATK + " + (P_minATK + upg_minATK).ToString() +
			"\nmaxHp + " + (P_maxHp + upg_maxHp).ToString() +
			"\ncriticalRate + " + (P_criticalRate + upg_criticalRate).ToString("0.00") +
			"\nmMove Speed + " + (P_moveSpeed + upg_moveSpeed).ToString("0.00") +
			"\nSuccess Rate : " + (successRate * 100).ToString("0.00") + "%"
		);
	}
	public override void upgrade()
	{
		if (DataManager.instance.data.kmsishNum[5] > 0)
		{
			DataManager.instance.data.kmsishNum[5]--;
			if (Random.Range(0f, 1f) < successRate)
			{
				lv++;
				DataManager.instance.data.kmsishLv[5]++;
				successRate *= 0.9f;
				P_maxATK += upg_maxATK;
				P_minATK += upg_minATK;
				P_maxHp += upg_maxHp;
				P_criticalRate += upg_criticalRate;
				P_moveSpeed += upg_moveSpeed;
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
		dto.moveSpeed = P_moveSpeed;
		return dto;
	}
}
