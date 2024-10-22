using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kms;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

public class DataManager : MonoBehaviour
{

	public GameData data = new GameData();

	public static DataManager instance;
	public Kmsish[] KmsRune;

	// 싱글톤으로 작성 ???
	// public static GameData Instance
	// {
	// 	get
	// 	{
	// 		if (!instance)
	// 		{
	// 			container = new GameObject();
	// 			container.name = "DataManager";
	// 			instance = container.AddComponent(typeof(GameData)) as GameData;
	// 			DontDestroyOnLoad(container);
	// 		}
	// 		return instance;
	// 	}
	// }

	string GameDataFileName = "GameData.json";

	public void SaveGameData()
	{
		data.isLoad = false;
		string toJsonData = JsonUtility.ToJson(data, false);
		string decData = Encrypt(toJsonData, 0x42);
		string filePath = Application.persistentDataPath + "/" + GameDataFileName;

		File.WriteAllText(filePath, decData);
		print("저장했소\n" + filePath + "\n" + decData);
	}

	public void LoadGameData()
	{
		string filePath = Application.persistentDataPath + "/" + GameDataFileName;

		if (File.Exists(filePath))
		{
			string fromJsonData = File.ReadAllText(filePath);
			string encData = Decrypt(fromJsonData, 0x42);
			data = JsonUtility.FromJson<GameData>(encData);
			print("불러왔소\n" + filePath + "\n" + encData);
			data.isLoad = true;
		}
	}

	private void initData()
	{
		if (data.isLoad == false)
		{
			data.kmsishNum = new int[26];
			data.kmsishLv = new int[26];
			for (int i = 0; i < 26; i++)
			{
				data.kmsishNum[i] = -1;
				data.kmsishLv[i] = 0;
				if (data.kmsishLv[i] > 0) { data.kmsishNum[i] = 0; }
			}
			data.kmsishLv[0] = 1;
			data.kmsishNum[0] = 1;
			data.kmsishLv[1] = 1;
			data.kmsishNum[1] = 1;
			data.equipRuneIdx = 0;
		}
	}
	private void initRune()
	{
		KmsRune = new Kmsish[26];
		KmsRune[0] = new Kms_A();
		KmsRune[1] = new Kms_B();
		KmsRune[2] = new Kms_C();
		KmsRune[3] = new Kms_D();
		KmsRune[4] = new Kms_E();
		KmsRune[5] = new Kms_F();
		KmsRune[0].initRune();
		KmsRune[1].initRune();
		KmsRune[2].initRune();
		KmsRune[3].initRune();
		KmsRune[4].initRune();
		KmsRune[5].initRune();
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			// instance.data.chLock = true; ??
			LoadGameData();
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		initData();
		initRune();
	}

	string Encrypt(string s, byte key)
	{
		var bytes = System.Text.Encoding.UTF8.GetBytes(s);
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] = (byte)(bytes[i] ^ key);
		}
		return System.Convert.ToBase64String(bytes);
	}
	string Decrypt(string s, byte key)
	{
		var bytes = System.Convert.FromBase64String(s);
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] = (byte)(bytes[i] ^ key);
		}
		return System.Text.Encoding.UTF8.GetString(bytes);
	}
}
