using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MarketSaveLoadManager : MonoBehaviour 
{
	public static void SaveMarketEggs(GlobalVariables marketEggSaver)
	{
		BinaryFormatter bf = new BinaryFormatter();
		//Directory.CreateDirectory("/eggSaver");
		FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Create);

		MarketEggsData data = new MarketEggsData(marketEggSaver);

		bf.Serialize(stream, data);
		stream.Close();
	}



	public static List<bool> LoadMarketEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

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



	public static int LoadMarketSilverEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

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
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

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



	public static void DeleteMarketSaveFile ()
	{
		File.Delete(Application.persistentDataPath + "/marketEggSaver.sav");
		Debug.Log("Save file deleted.");
	}


	
}

public class ParkSaveLoadManager : MonoBehaviour 
{
	public static void SaveParkEggs(GlobalVariables parkEggSaver)
	{
		BinaryFormatter bf = new BinaryFormatter();
		//Directory.CreateDirectory("/eggSaver");
		FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Create);

		ParkEggsData data = new ParkEggsData(parkEggSaver);

		bf.Serialize(stream, data);
		stream.Close();
	}



	public static List<bool> LoadParkEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.eggsFound;
		}
		else 
		{
			Debug.LogError("FILE DOES NOT EXIST");
			return new List<bool>();
		}
	}



	public static int LoadParkSilverEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.silverEggsFound;
		}
		else 
		{
			Debug.LogError("FILE DOES NOT EXIST");
			return 0;
		}
	}



	public static bool LoadHopscotchRiddle()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.hopscotchRiddle;
		}
		else 
		{
			Debug.LogError("FILE DOES NOT EXIST");
			return false;
		}
	}



	public static void DeleteParkSaveFile ()
	{
		File.Delete(Application.persistentDataPath + "/parkEggSaver.sav");
		Debug.Log("Save file deleted.");
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
		eggsFound = eggSaver.eggsFoundBools;

		silverEggsFound = eggSaver.silverEggsCount;

		rainbowRiddle = eggSaver.rainbowRiddleSolved;
	}
}

[Serializable]
public class ParkEggsData 
{
	public List<bool> eggsFound;

	public int silverEggsFound;

	public bool hopscotchRiddle;

	public ParkEggsData(GlobalVariables eggSaver)
	{
		eggsFound = eggSaver.eggsFoundBools;

		silverEggsFound = eggSaver.silverEggsCount;

		hopscotchRiddle = eggSaver.hopscotchRiddleSolved;
	}
}
