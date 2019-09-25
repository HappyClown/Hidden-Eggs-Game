using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonCadres : MonoBehaviour {
	public List<ParticleSystem> summerCadreParticles, fallCadreParticles, winterCadreParticles, springCadreParticles;
	private List<ParticleSystem> cadreParticles;
	private string seasonName;
	public Dictionary<string, List<ParticleSystem>> sceneStuff = new Dictionary<string, List<ParticleSystem>>();
	public GlobalVariables globoVaro;
	// public string SeasonName
	// {
	// 	get
	// 	{
	// 		return seasonName;
	// 	}
	// 	set
	// 	{
	// 		if (seasonName == "Summer") {
	// 			cadreParticles = summerParticles;
	// 		}
	// 		else if (seasonName == "Fall") {
	// 			cadreParticles = fallParticles;
	// 		}
	// 		else if (seasonName == "Winter") {
	// 			cadreParticles = winterParticles;
	// 		}
	// 		else if (seasonName == "Spring") {
	// 			cadreParticles = springParticles;
	// 		}
	// 		seasonName = value;
	// 	}
	// }

	public List<ParticleSystem> GetCadreParticles(string scene) {
		sceneStuff.Clear();
		sceneStuff.Add(GlobalVariables.globVarScript.marketName, summerCadreParticles);
		sceneStuff.Add(GlobalVariables.globVarScript.marketPuzName, summerCadreParticles);
		sceneStuff.Add(GlobalVariables.globVarScript.parkName, summerCadreParticles);
		sceneStuff.Add(GlobalVariables.globVarScript.parkPuzName, summerCadreParticles);
		sceneStuff.Add(GlobalVariables.globVarScript.beachName, summerCadreParticles);
		sceneStuff.Add(GlobalVariables.globVarScript.beachPuzName, summerCadreParticles);
		List<ParticleSystem> chosenParts = new List<ParticleSystem>();
		Debug.Log(scene);
		Debug.Log(sceneStuff[scene]);
		chosenParts = sceneStuff[scene];
		return chosenParts;
	}

	// Method to call when you want a scene's season and cadre particle systems.
	// public static Dictionary<string, Scenes> GetScenes() {
	// 	var scenesDict = new Dictionary<string, Scenes>();
		//var sceneStuff = new Scenes("Summer", summerCadreParticles);
		// scenesDict.Add(GlobalVariables.globVarScript.marketName, sceneStuff);
		// scenesDict.Add(GlobalVariables.globVarScript.marketPuzName, sceneStuff);
		// scenesDict.Add(GlobalVariables.globVarScript.parkName, sceneStuff);
		// scenesDict.Add(GlobalVariables.globVarScript.parkPuzName, sceneStuff);
		// scenesDict.Add(GlobalVariables.globVarScript.beachName, sceneStuff);
		// scenesDict.Add(GlobalVariables.globVarScript.beachPuzName, sceneStuff);
		// var sceneStuff = new Scenes("Fall", fallCadreParticles);
	// 	return scenesDict;
	// }
	

}
