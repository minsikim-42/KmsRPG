// using UnityEngine;
// using System.IO;

// public class DataManager : MonoBehaviour
// {
// 	static GameObject container;
// 	static DataManager instance; // 싱글톤으로 작성
// 	public static DataManager Instance
// 	{
// 		get
// 		{
// 			if (!instance)
// 			{
// 				container = new GameObject();
// 				container.name = "DataManager";
// 				instance = container.AddComponent(typeof(DataManager)) as DataManager;
// 				DontDestroyOnLoad(container);
// 			}
// 			return instance;
// 		}
// 	}

// 	private void Awake()
// 	{
// 		LoadGameData();
// 	}

// 	string GameDataFileName = "GameData.json";
// 	public GameData data = new GameData();

// 	public void SaveGameData()
// 	{
// 		string toJsonData = JsonUtility.ToJson(data, true);
// 		string filePath = Application.persistentDataPath + "/" + GameDataFileName;

// 		File.WriteAllText(filePath, toJsonData);
// 		print("저장했소\n" + data);
// 	}

// 	public void LoadGameData()
// 	{
// 		string filePath = Application.persistentDataPath + "/" + GameDataFileName;

// 		if (File.Exists(filePath))
// 		{
// 			string fromJsonData = File.ReadAllText(filePath);
// 			data = JsonUtility.FromJson<GameData>(fromJsonData);
// 			print("불러왔소\n" + data);
// 		}
// 	}
// }
