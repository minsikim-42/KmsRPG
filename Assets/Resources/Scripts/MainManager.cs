using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Kms;
using TMPro;

public class MainManager : MonoBehaviour
{
	[SerializeField] public GameObject MenuPanel;
	[SerializeField] public GameObject SelectPanel;
	[SerializeField] public RunePanel RunePanel;
	[SerializeField] public GameObject SettingPanel;
	[SerializeField] public RuneStatePanel RSPanel;
	[SerializeField] public GameObject elf_black;
	[SerializeField] public TextMeshProUGUI elf_text;


	[SerializeField] Image[] KmsishImg;
	[SerializeField] public TextMeshProUGUI settingText;

	public static MainManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		// DontDestroyOnLoad(gameObject);
	}
	private void Start()
	{
		MenuPanel.SetActive(true);
		SelectPanel.SetActive(false);
		RunePanel.gameObject.SetActive(false);
		SettingPanel.SetActive(false);
		RSPanel.gameObject.SetActive(false);

		if (DataManager.instance.data.isJoy == true)
			MainManager.instance.settingText.SetText("Now: JoyStick");
		else

			MainManager.instance.settingText.SetText("Now: ArrowButton");
	}

	public void SelectCharactor()
	{
		MenuPanel.SetActive(false);
		SelectPanel.SetActive(true);
		if (DataManager.instance.data.chLock == true)
		{
			elf_black.SetActive(true);
			elf_text.SetText("???");
		}
		else
		{
			elf_black.SetActive(false);
			elf_text.SetText("Green Elf");
		}
	}
	public void RunePage()
	{
		MenuPanel.SetActive(false);

		RunePanel.gameObject.SetActive(true);
		RunePanel.SetBlind();
	}
	public void MenuPage()
	{
		MenuPanel.SetActive(true);

		RunePanel.gameObject.SetActive(false);
		SelectPanel.SetActive(false);
		SettingPanel.SetActive(false);
	}
	public void SettingPage()
	{
		MenuPanel.SetActive(false);

		SettingPanel.SetActive(true);
	}

	public void GameScnCtl(int ch)
	{
		if (ch == 1 && DataManager.instance.data.chLock == true)
		{
			return;
		}
		PlayerPrefs.SetInt("Charactor", ch);
		SceneManager.LoadScene("Game");
		this.gameObject.SetActive(false);
	}

	public void SettingJoy()
	{
		DataManager.instance.data.isJoy = true;
		settingText.SetText("Now: JoyStick");
	}
	public void SettingArrow()
	{
		DataManager.instance.data.isJoy = false;
		settingText.SetText("Now: ArrowButton");
	}

	public void Quit()
	{
		DataManager.instance.SaveGameData();
		Application.Quit();
	}
}
