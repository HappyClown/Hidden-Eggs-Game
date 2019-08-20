using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GeneralSaveLoadManager : MonoBehaviour {
	public static void SaveGeneralData(GlobalVariables villageSaver) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream stream = new FileStream(Application.persistentDataPath + "/generalSaver.sav", FileMode.Create);

		GeneralData data = new GeneralData(villageSaver);

		bf.Serialize(stream, data);
		stream.Close();
	}


	public static int LoadLevelsCompleted() {
		if (File.Exists(Application.persistentDataPath + "/generalSaver.sav")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/generalSaver.sav", FileMode.Open);

			GeneralData data = bf.Deserialize(stream) as GeneralData;

			stream.Close();
			return data.levelsCompleted;
		}
		else {
			Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}

	public static float LoadLastEggTotVal() {
		if (File.Exists(Application.persistentDataPath + "/generalSaver.sav")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/generalSaver.sav", FileMode.Open);

			GeneralData data = bf.Deserialize(stream) as GeneralData;

			stream.Close();
			return data.lastEggTotVal;
		}
		else {
			Debug.LogWarning("FILE DOES NOT EXIST");
			return 0;
		}
	}

	public static bool LoadFallLocked() {
		if (File.Exists(Application.persistentDataPath + "/generalSaver.sav")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/generalSaver.sav", FileMode.Open);

			GeneralData data = bf.Deserialize(stream) as GeneralData;

			stream.Close();
			return data.fallLocked;
		}
		else {
			Debug.LogWarning("FILE DOES NOT EXIST");
			return true;
		}
	}

	public static void DeleteGeneralSaveFile() {
		File.Delete(Application.persistentDataPath + "/generalSaver.sav");
		Debug.LogWarning("Save file deleted.");
	}
}

[Serializable]
public class GeneralData {
	public int levelsCompleted;
	public float lastEggTotVal;
	public bool fallLocked;

	public GeneralData(GlobalVariables generalSaver) {
		levelsCompleted = generalSaver.levelsCompleted;
		lastEggTotVal = generalSaver.lastEggTotVal;
		fallLocked = generalSaver.fallLocked;
	}
}

