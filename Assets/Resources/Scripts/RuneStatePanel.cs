using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuneStatePanel : MonoBehaviour
{
	[SerializeField] Image[] KmsRuneImage;
	[SerializeField] Button equipButton;
	[SerializeField] Button upgradeButton;
	[SerializeField] GameObject RSButtons;

	[SerializeField] GameObject upgradePanel;
	[SerializeField] Button okButton;
	int printIdx;
	void Awake()
	{
		foreach (Image image in KmsRuneImage)
		{
			image.gameObject.SetActive(false);
		}
	}
	private void Start()
	{
		upgradePanel.SetActive(false);
	}

	public void printRune(int idx)
	{
		KmsRuneImage[idx].gameObject.SetActive(true);
		printIdx = idx;
		if (DataManager.instance.data.equipRuneIdx == idx)
		{
			equipButton.interactable = false;
		}
		else
		{
			equipButton.interactable = true;
		}
	}
	public void eraseRune()
	{
		KmsRuneImage[printIdx].gameObject.SetActive(false);
	}
	public void equipRune()
	{
		DataManager.instance.data.equipRuneIdx = printIdx;
		equipButton.interactable = false;
	}
	public void upgradeRune()
	{
		RSButtons.SetActive(false);
		upgradePanel.SetActive(true);
		DataManager.instance.KmsRune[printIdx].upgradeSetText();
		okButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Upgrade: " + DataManager.instance.data.kmsishNum[printIdx].ToString());
	}
	public void upgradeOkButton()
	{
		DataManager.instance.KmsRune[printIdx].upgrade();
		okButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Upgrade: " + DataManager.instance.data.kmsishNum[printIdx].ToString());
	}
	public void upgradeBackButton()
	{
		RSButtons.SetActive(true);
		upgradePanel.SetActive(false);
	}
}
