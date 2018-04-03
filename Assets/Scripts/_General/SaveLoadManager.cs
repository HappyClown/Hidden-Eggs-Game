using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager : MonoBehaviour 
{
	public static void SaveEggs(GlobalVariables eggSaver)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream stream = new FileStream(Application.persistentDataPath + "/eggSaver.sav", FileMode.Create);

		MarketEggsData data = new MarketEggsData(eggSaver);

		bf.Serialize(stream, data);
		stream.Close();
	}

	public static List<bool> LoadEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/eggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/eggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.eggsFound;
		}
		else 
		{
			Debug.LogError("FILE DOES NOT EXIST");
			return new List<bool>();
		}
	}



	public static int LoadSilverEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/eggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/eggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.silverEggsFound;
		}
		else 
		{
			Debug.LogError("FILE DOES NOT EXIST");
			return 0;
		}
	}



	public static bool LoadRainbowRiddle()
	{
		if (File.Exists(Application.persistentDataPath + "/eggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/eggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.rainbowRiddle;
		}
		else 
		{
			Debug.LogError("FILE DOES NOT EXIST");
			return false;
		}
	}



	
}

[Serializable]
public class MarketEggsData 
{
	public List<bool> eggsFound;

	public int silverEggsFound;

	public bool rainbowRiddle;

	public MarketEggsData(GlobalVariables eggSaver)
	{
		// foreach(GameObject egg in eggSaver.eggs)
		// {
		// 	eggsFound.Add(egg.GetComponent<EggGoToCorner>().eggFound);
		// }
		eggsFound = eggSaver.eggsFoundBools;

		silverEggsFound = eggSaver.silverEggsCount;

		rainbowRiddle = eggSaver.rainbowRiddleSolved;
	}
	//public List<GameObject> marketEggs;

	// public EggData(EggGoToCorner eggSaver)
	// {
	// 	eggs = new bool[eggSaver.]
	// }


}
