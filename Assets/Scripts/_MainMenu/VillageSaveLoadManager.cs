using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class VillageSaveLoadManager : MonoBehaviour 
{
	public static void SaveVillage(GlobalVariables villageSaver)
	{
		BinaryFormatter bf = new BinaryFormatter();
		//Directory.CreateDirectory("/eggSaver");
		FileStream stream = new FileStream(Application.persistentDataPath + "/villageSaver.sav", FileMode.Create);

		VillageData data = new VillageData(villageSaver);

		bf.Serialize(stream, data);
		stream.Close();
	}

	public static List<bool> LoadDissolvedSeasons()
	{
		if (File.Exists(Application.persistentDataPath + "/villageSaver.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/villageSaver.sav", FileMode.Open);

			VillageData data = bf.Deserialize(stream) as VillageData;

			stream.Close();
			return data.dissolvedSeasons;
		}
		else 
		{
			Debug.LogWarning("FILE DOES NOT EXIST");
			return new List<bool>();
		}
	}

	public static void DeleteVillageSaveFile()
	{
		File.Delete(Application.persistentDataPath + "/villageSaver.sav");
		Debug.LogWarning("Save file deleted.");
	}
}

[Serializable]
public class VillageData 
{
	public List<bool> dissolvedSeasons;

	public VillageData(GlobalVariables villageSaver)
	{
		dissolvedSeasons = villageSaver.dissSeasonsBools;

	}
}
