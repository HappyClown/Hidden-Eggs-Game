using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonCadresManager : MonoBehaviour {
	public SeasonCadre[] seasonCadresScripts;

	public SeasonCadre GetCadreInfo(string sceneName) {
		foreach(SeasonCadre sCScript in seasonCadresScripts)
		{
			foreach(string nameInArray in sCScript.sceneNames)
			{
				if (sceneName == nameInArray) {
					return sCScript;
				}
			}
		}
		if (seasonCadresScripts.Length > 0) {
			return seasonCadresScripts[0];
		}
		else {
			return null;
		}
	}
}
