using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kms;

public class RunePanel : MonoBehaviour
{
	[SerializeField] GameObject[] RuneBlinder;

	[SerializeField] RuneStatePanel RSPanel;

	private void Start()
	{
		RSPanel.gameObject.SetActive(false);
	}

	public void setRuneStat(int runeNum)
	{
		this.gameObject.SetActive(false);
		RSPanel.gameObject.SetActive(true);
		RSPanel.printRune(runeNum);
		DataManager.instance.KmsRune[runeNum].SetText();
	}

	public void backToRune()
	{
		RSPanel.eraseRune();
		RSPanel.gameObject.SetActive(false);
		this.gameObject.SetActive(true);
	}

	public void SetBlind()
	{
		for (int i = 0; i < 26; i++)
		{
			if (DataManager.instance.data.kmsishNum[i] >= 0)
			{
				RuneBlinder[i].SetActive(false);
			}
			else
			{
				RuneBlinder[i].SetActive(true);
			}
		}
	}
}