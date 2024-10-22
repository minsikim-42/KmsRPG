using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Kms;

public class GameManager : MonoBehaviour
{
	[SerializeField] Camera mainCamera;
	[SerializeField] Camera playerCamera;
	public bool isDie = false;
	public static GameManager instance = null;

	[SerializeField] private bool isMobile;

	Player player;
	Player2 player2;

	[SerializeField] public GameObject[] KmsRunePrefabs;

	public int PlayerNum;

	public int Lv = 1;
	[SerializeField] TextMeshProUGUI lvText;
	int maxExp = 100;
	int Exp = 0;
	int maxHp = 100;
	int Hp = 500;
	public int minATK = 25;
	public int maxATK = 60;

	public Vector3 movement;
	[SerializeField] Image ExpBar;
	[SerializeField] TextMeshProUGUI expPer;
	[SerializeField] GameObject BossPanel;
	public bool isBoss = false;
	[SerializeField] GameObject statePanel;
	[SerializeField] TextMeshProUGUI stateText;
	[SerializeField] GameObject BackToMenuPanel;

	float moveSpeed = 2f; // 2f 
	float criticalRate = 0.1f; // 0.1
	float criticalDMG = 1.5f; // 1.5
	[SerializeField] Image ZskillCoolBar;
	public float tm = 0f;
	[SerializeField] float ZskillCool = 10f;

	[SerializeField] EnemySpawner enemySpawner;
	public bool isKeyButtonPressed = false;

	[SerializeField] GameObject grass1;
	[SerializeField] GameObject grass2;
	[SerializeField] GameObject grass3;
	[SerializeField] GameObject grass4;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		if (FindObjectOfType<DataManager>() == null) // first main menu
		{
			SceneManager.LoadScene("Menu");
		}

		isMobile = false;
		
		// DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		PlayerNum = PlayerPrefs.GetInt("Charactor", 0);
		BossPanel.SetActive(false);
		statePanel.SetActive(false);
		BackToMenuPanel.SetActive(false);

		player = FindObjectOfType<Player>();
		player2 = FindObjectOfType<Player2>();
		if (PlayerNum == 0)
		{ // WC
			player.gameObject.SetActive(true);
			player2.gameObject.SetActive(false);
			maxHp = 500;
			Hp = 500;
			minATK = 25;
			maxATK = 60;
			moveSpeed = 2f; // 2f 
			criticalRate = 0.1f; // 0.1
			criticalDMG = 1.5f; // 1.5
		}
		else if (PlayerNum == 1)
		{ // G E
			player.gameObject.SetActive(false);
			player2.gameObject.SetActive(true);
			maxHp = 250;
			Hp = 250;
			minATK = 15;
			maxATK = 70;
			moveSpeed = 2.8f; // 2f 
			criticalRate = 0.07f; // 0.1
			criticalDMG = 1.8f; // 1.5
		}
		equipRune();
		setStateText();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Z))
		{
			Skill();
		}
	}

	private void FixedUpdate()
	{
		float exPer = 200f * Exp / maxExp;
		expPer.SetText((exPer / 2).ToString("#.###"));
		ExpBar.rectTransform.offsetMin = new Vector2(exPer, -17.5f);
		ExpBar.rectTransform.offsetMax = new Vector2(0, 17.5f);
		lvText.SetText("LV." + Lv.ToString());

		float ZskillPer = 100f * tm / ZskillCool;
		ZskillCoolBar.rectTransform.offsetMin = new Vector2(0, -15f);
		ZskillCoolBar.rectTransform.offsetMax = new Vector2(ZskillPer - 100, 15f);

		// grassRender();

		if (Input.GetKey(KeyCode.Tab))
		{
			statePanel.SetActive(true);
		}
		else
		{
			statePanel.SetActive(false);
		}

		if (Input.GetKey(KeyCode.BackQuote))
		{
			reBorn();
		}

		if (tm < ZskillCool)
			tm += Time.fixedDeltaTime;
	}

	void equipRune()
	{
		int runeNum = DataManager.instance.data.equipRuneIdx;
		PlayerStateDTO pDTO = DataManager.instance.KmsRune[runeNum].getState();
		maxHp += pDTO.maxHp;
		maxATK += pDTO.maxATK;
		minATK += pDTO.minATK;
		criticalDMG += pDTO.criticalDMG;
		criticalRate += pDTO.criticalRate;
		moveSpeed += pDTO.moveSpeed;

		Hp = maxHp;
	}

	public void earnExp(int exp)
	{
		Exp += exp;
		if (maxExp <= Exp)
		{
			LvUp();
			setStateText();
			StartCoroutine("CoBossPanel");
		}
	}

	public DamageState calculDMG()
	{
		float DMG = Random.Range(minATK, maxATK);
		if (Random.Range(0f, 1f) <= criticalRate)
		{
			DMG *= criticalDMG;
			return new DamageState((int)DMG, true);
		}
		return new DamageState((int)DMG, false);
	}
	void setStateText()
	{
		stateText.SetText("minATK : " + minATK +
			"\nmaxATK : " + maxATK +
			"\nMaxHp : " + maxHp +
			"\nSpeed : " + moveSpeed.ToString("#.###") +
			"\nCriticalDMG : " + criticalDMG.ToString("#.###") +
			"\nCriticalRate : " + criticalRate.ToString("#.###"));
	}
	void reBorn()
	{
		isDie = false;
		hpUp(maxHp);
		player.animator.SetFloat("isDead", 0f);
		player.animator.StopPlayback();
	}
	public void LvUp()
	{
		Exp = Exp - maxExp;
		maxExp = (int)(maxExp * 1.5f);
		Lv++;
		if (Lv < 15)
		{
			minATK += 2;
			maxATK += 3;
			criticalDMG *= 1.02f;
			criticalRate *= 1.05f;
			setMaxHp(1.1f);
			hpUp(0.25f);
			moveSpeed *= 1.02f;
		}
		else
		{
			minATK += 2;
			maxATK += 4;
			criticalDMG *= 1 + 0.3f / Lv;
			criticalRate *= 1 + 0.3f / Lv;
			setMaxHp((1 + 1f / Lv));
			hpUp(0.2f);
			moveSpeed *= 1 + 0.5f / Lv;
		}
	}

	public void makeBackToMenuPanel()
	{
		BackToMenuPanel.SetActive(true);
	}
	public void BackToMenu()
	{
		// this.gameObject.SetActive(false);
		// MainManager.instance.gameObject.SetActive(true);
		// // MainManager.instance.MenuPanel.SetActive(true);
		// MainManager.instance.SelectPanel.SetActive(false);
		// MainManager.instance.RunePanel.gameObject.SetActive(false);
		// MainManager.instance.RSPanel.gameObject.SetActive(false);
		if (Lv > 6)
		{
			DataManager.instance.data.chLock = false;
		}
		SceneManager.LoadScene("Menu");
	}

	// Getter Setter
	public void hpUp(int plus)
	{
		Hp += plus;
		if (Hp > maxHp)
		{
			Hp = maxHp;
		}
	}
	public void hpUp(float per)
	{
		Hp += (int)(maxHp * per);
		if (Hp > maxHp)
		{
			Hp = maxHp;
		}
	}
	public void setMaxHp(float per)
	{
		maxHp = (int)(maxHp * per);
	}
	public void damaged(int dmg)
	{
		Hp -= dmg;
	}
	public float getHpPer()
	{
		return ((float)Hp / maxHp);
	}
	public bool checkIsDie()
	{
		if (Hp < 0)
		{
			isDie = true;
			Hp = 0;
			return true;
		}
		return false;
	}
	public float getMoveSpeed()
	{
		return moveSpeed;
	}
	public float getCriticalDMG()
	{
		return criticalDMG;
	}

	IEnumerator CoBossPanel()
	{
		isBoss = true;
		enemySpawner.makeBoss(Lv);
		BossPanel.SetActive(true);
		yield return new WaitForSeconds(3f);
		BossPanel.SetActive(false);
	}

	public void AttackButtonPressed()
	{
		if (PlayerNum == 0)
		{
			player.Attack();
		}
		else
		{
			player2.Attack(1);
		}
	}

	public void Skill()
	{
		if (tm < ZskillCool)
		{
			return;
		}
		if (PlayerNum == 0)
		{
			tm = 0;
			player.Zskill();
		}
		else if (PlayerNum == 1)
		{
			tm = 0;
			player2.Zskill();
		}
	}

	public void KeyButtonPressed(int clkDir)
	{
		if (PlayerNum == 0)
		{
			player.KeyButtonPressed(clkDir);
		}
	}
	public void KeyButtonDepressed(int clkDir)
	{
		if (PlayerNum == 0)
		{
			player.KeyButtonDepressed(clkDir);
		}
	}
	void grassRender()
	{
		if (PlayerNum == 0)
		{
			if (player.transform.position.x > 0)
			{
				grass4.transform.position = new Vector3(18, 0, 0);
			}
			else
			{
				grass4.transform.position = new Vector3(-18, 0, 0);
			}
			if (player.transform.position.y > 0)
			{
				grass2.transform.position = new Vector3(0, 10, 0);
			}
			else
			{
				grass2.transform.position = new Vector3(0, -10, 0);
			}
			if (player.transform.position.x > 0 && player.transform.position.y > 0)
			{
				grass1.transform.position = new Vector3(18, 10, 0);
			}
			else if (player.transform.position.x < 0 && player.transform.position.y < 0)
			{
				grass1.transform.position = new Vector3(-18, -10, 0);
			}
			else if (player.transform.position.x > 0 && player.transform.position.y < 0)
			{
				grass1.transform.position = new Vector3(18, -10, 0);
			}
			else if (player.transform.position.x < 0 && player.transform.position.y > 0)
			{
				grass1.transform.position = new Vector3(-18, 10, 0);
			}
		}
		else if (PlayerNum == 1)
		{
			if (player2.transform.position.x > 0)
			{
				grass4.transform.position = new Vector3(18, 0, 0);
			}
			else
			{
				grass4.transform.position = new Vector3(-18, 0, 0);
			}
			if (player2.transform.position.y > 0)
			{
				grass2.transform.position = new Vector3(0, 10, 0);
			}
			else
			{
				grass2.transform.position = new Vector3(0, -10, 0);
			}
			if (player2.transform.position.x > 0 && player2.transform.position.y > 0)
			{
				grass1.transform.position = new Vector3(18, 10, 0);
			}
			else if (player2.transform.position.x < 0 && player2.transform.position.y < 0)
			{
				grass1.transform.position = new Vector3(-18, -10, 0);
			}
			else if (player2.transform.position.x > 0 && player2.transform.position.y < 0)
			{
				grass1.transform.position = new Vector3(18, -10, 0);
			}
			else if (player2.transform.position.x < 0 && player2.transform.position.y > 0)
			{
				grass1.transform.position = new Vector3(-18, 10, 0);
			}
		}
	}
}
