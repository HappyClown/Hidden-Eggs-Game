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
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<bool>();
		}
	}


	public static List<int> LoadMarketEggsOrder()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.eggsFoundOrder;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
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
			Debug.LogWarning("FILE DOES NOT EXIST");
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
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}


	public static int LoadMarketPuzzMaxLvl()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.puzzMaxLvl;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}


	public static int LoadMarketTotalEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.totalEggsFound;
		}
		else 
		{
			//Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}


	public static List<int> LoadMarketPuzzSilEggsCount()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.puzzSilEggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
		}
	}


	public static List<int> LoadMarketSceneSilEggsCount()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.sceneSilEggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
		}
	}


	public static bool LoadMarketLevelComplete()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.levelComplete;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}

	public static bool LoadMarketBirdIntro()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.introDone;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}

	public static bool LoadMarketPuzzIntro()
	{
		if (File.Exists(Application.persistentDataPath + "/marketEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/marketEggSaver.sav", FileMode.Open);

			MarketEggsData data = bf.Deserialize(stream) as MarketEggsData;

			stream.Close();
			return data.puzzIntroDone;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}

	public static void DeleteMarketSaveFile()
	{
		File.Delete(Application.persistentDataPath + "/marketEggSaver.sav");
		Debug.LogWarning("Save file deleted.");
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
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<bool>();
		}
	}


	public static List<int> LoadParkEggsOrder()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.eggsFoundOrder;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
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
			Debug.LogWarning("FILE DOES NOT EXIST");
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
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}


	public static int LoadParkPuzzMaxLvl()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.puzzMaxLvl;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}


	public static int LoadParkTotalEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.totalEggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}


	public static List<int> LoadParkPuzzSilEggsCount()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.puzzSilEggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
		}
	}


	public static List<int> LoadParkSceneSilEggsCount()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.sceneSilEggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
		}
	}


	public static bool LoadParkLevelComplete()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.levelComplete;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}


	public static bool LoadParkBirdIntro()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.introDone;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}


	public static bool LoadParkPuzzIntro()
	{
		if (File.Exists(Application.persistentDataPath + "/parkEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/parkEggSaver.sav", FileMode.Open);

			ParkEggsData data = bf.Deserialize(stream) as ParkEggsData;

			stream.Close();
			return data.puzzIntroDone;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}


	public static void DeleteParkSaveFile ()
	{
		File.Delete(Application.persistentDataPath + "/parkEggSaver.sav");
		Debug.LogWarning("Save file deleted.");
	}
}

public class BeachSaveLoadManager : MonoBehaviour 
{
	public static void SaveBeachEggs(GlobalVariables beachEggSaver)
	{
		BinaryFormatter bf = new BinaryFormatter();
		//Directory.CreateDirectory("/eggSaver");
		FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Create);

		BeachEggsData data = new BeachEggsData(beachEggSaver);

		bf.Serialize(stream, data);
		stream.Close();
	}


		public static List<bool> LoadBeachEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.eggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<bool>();
		}
	}


	public static List<int> LoadBeachEggsOrder()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.eggsFoundOrder;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
		}
	}


	public static int LoadBeachSilverEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.silverEggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}


	public static bool LoadCrabRiddle()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.crabRiddle;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}


	public static int LoadBeachPuzzMaxLvl()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.puzzMaxLvl;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}


	public static int LoadBeachTotalEggs()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.totalEggsFound;
		}
		else 
		{
			//Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}


	public static List<int> LoadBeachPuzzSilEggsCount()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.puzzSilEggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
		}
	}


	public static List<int> LoadBeachSceneSilEggsCount()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.sceneSilEggsFound;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<int>();
		}
	}


	public static bool LoadBeachLevelComplete()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.levelComplete;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}

	public static bool LoadBeachBirdIntro()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.introDone;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}

	public static bool LoadBeachPuzzIntro()
	{
		if (File.Exists(Application.persistentDataPath + "/beachEggSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/beachEggSaver.sav", FileMode.Open);

			BeachEggsData data = bf.Deserialize(stream) as BeachEggsData;

			stream.Close();
			return data.puzzIntroDone;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return false;
		}
	}


	public static void DeleteBeachSaveFile ()
	{
		File.Delete(Application.persistentDataPath + "/beachEggSaver.sav");
		Debug.LogWarning("Save file deleted.");
	}
}

[Serializable]
public class MarketEggsData 
{
	public List<bool> eggsFound;
	public List<int> eggsFoundOrder;
	public int silverEggsFound;
	public List<int> puzzSilEggsFound;
	public List<int> sceneSilEggsFound;
	public bool rainbowRiddle;
	public int puzzMaxLvl;
	public int totalEggsFound;
	public bool levelComplete;
	public bool introDone;
	public bool puzzIntroDone;

	public MarketEggsData(GlobalVariables marketEggSaver)
	{
		eggsFound = marketEggSaver.eggsFoundBools;

		eggsFoundOrder = marketEggSaver.eggsFoundOrder;

		silverEggsFound = marketEggSaver.silverEggsCount;

		puzzSilEggsFound = marketEggSaver.puzzSilEggsCount;
		
		sceneSilEggsFound = marketEggSaver.sceneSilEggsCount;

		rainbowRiddle = marketEggSaver.riddleSolved;

		puzzMaxLvl = marketEggSaver.puzzMaxLvl;

		totalEggsFound = marketEggSaver.totalEggsFound;

		levelComplete = marketEggSaver.levelComplete;

		introDone = marketEggSaver.birdIntroDone;

		puzzIntroDone = marketEggSaver.puzzIntroDone;
	}
}

[Serializable]
public class ParkEggsData 
{
	public List<bool> eggsFound;
	public List<int> eggsFoundOrder;
	public int silverEggsFound;
	public List<int> puzzSilEggsFound;
	public List<int> sceneSilEggsFound;
	public bool hopscotchRiddle;
	public int puzzMaxLvl;
	public int totalEggsFound;
	public bool levelComplete;
	public bool introDone;
	public bool puzzIntroDone;

	public ParkEggsData(GlobalVariables parkEggSaver)
	{
		eggsFound = parkEggSaver.eggsFoundBools;

		eggsFoundOrder = parkEggSaver.eggsFoundOrder;

		silverEggsFound = parkEggSaver.silverEggsCount;

		puzzSilEggsFound = parkEggSaver.puzzSilEggsCount;
		
		sceneSilEggsFound = parkEggSaver.sceneSilEggsCount;

		hopscotchRiddle = parkEggSaver.riddleSolved;

		puzzMaxLvl = parkEggSaver.puzzMaxLvl;

		totalEggsFound = parkEggSaver.totalEggsFound;

		levelComplete = parkEggSaver.levelComplete;

		introDone = parkEggSaver.birdIntroDone;

		puzzIntroDone = parkEggSaver.puzzIntroDone;
	}
}

[Serializable]
public class BeachEggsData 
{
	public List<bool> eggsFound;
	public List<int> eggsFoundOrder;
	public int silverEggsFound;
	public List<int> puzzSilEggsFound;
	public List<int> sceneSilEggsFound;
	public bool crabRiddle;
	public int puzzMaxLvl;
	public int totalEggsFound;
	public bool levelComplete;
	public bool introDone;
	public bool puzzIntroDone;

	public BeachEggsData(GlobalVariables beachEggSaver)
	{
		eggsFound = beachEggSaver.eggsFoundBools;

		eggsFoundOrder = beachEggSaver.eggsFoundOrder;

		silverEggsFound = beachEggSaver.silverEggsCount;

		puzzSilEggsFound = beachEggSaver.puzzSilEggsCount;
		
		sceneSilEggsFound = beachEggSaver.sceneSilEggsCount;

		crabRiddle = beachEggSaver.riddleSolved;

		puzzMaxLvl = beachEggSaver.puzzMaxLvl;

		totalEggsFound = beachEggSaver.totalEggsFound;

		levelComplete = beachEggSaver.levelComplete;

		introDone = beachEggSaver.birdIntroDone;

		puzzIntroDone = beachEggSaver.puzzIntroDone;
	}
}
