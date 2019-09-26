using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonCadres : MonoBehaviour {
	public List<ParticleSystem> summerCadreParticles, fallCadreParticles, winterCadreParticles, springCadreParticles;
	public List<ParticleSystem> cadreParticles;
	//private string seasonName;
	public Dictionary<string, List<ParticleSystem>> sceneDict = new Dictionary<string, List<ParticleSystem>>();
	//public GlobalVariables globoVaro;
	
	public void Start () {
		CreateSceneDict();
	}
	public void CreateSceneDict() {
		//sceneDict.Clear();
		sceneDict.Add(GlobalVariables.globVarScript.marketName, summerCadreParticles);
		sceneDict.Add(GlobalVariables.globVarScript.marketPuzName, summerCadreParticles);
		sceneDict.Add(GlobalVariables.globVarScript.parkName, summerCadreParticles);
		sceneDict.Add(GlobalVariables.globVarScript.parkPuzName, summerCadreParticles);
		sceneDict.Add(GlobalVariables.globVarScript.beachName, summerCadreParticles);
		sceneDict.Add(GlobalVariables.globVarScript.beachPuzName, summerCadreParticles);
	}

	public List<ParticleSystem> GetCadreParticles(string scene) {
		// if (sceneDict[scene] == null) {
		// 	CreateSceneDict();
		// }
		sceneDict.Clear();
		CreateSceneDict();
		foreach (string key in sceneDict.Keys)
		{
			List<ParticleSystem> val = sceneDict[key];
			Debug.Log(key + " = " + val);
		}
		//List<ParticleSystem> chosenParts = new List<ParticleSystem>();
		// Debug.Log("Should be the Dict: " + sceneDict);
		// Debug.Log("The scene being fed in: " + scene);
		cadreParticles.Clear();
		cadreParticles = sceneDict[scene];
		//chosenParts = sceneDict[scene];
		return cadreParticles;
	}

	// Method to call when you want a scene's season and cadre particle systems.
	// public static Dictionary<string, Scenes> GetScenes() {
	// 	var scenesDict = new Dictionary<string, Scenes>();
		//var sceneDict = new Scenes("Summer", summerCadreParticles);
		// scenesDict.Add(GlobalVariables.globVarScript.marketName, sceneDict);
		// scenesDict.Add(GlobalVariables.globVarScript.marketPuzName, sceneDict);
		// scenesDict.Add(GlobalVariables.globVarScript.parkName, sceneDict);
		// scenesDict.Add(GlobalVariables.globVarScript.parkPuzName, sceneDict);
		// scenesDict.Add(GlobalVariables.globVarScript.beachName, sceneDict);
		// scenesDict.Add(GlobalVariables.globVarScript.beachPuzName, sceneDict);
		// var sceneDict = new Scenes("Fall", fallCadreParticles);
	// 	return scenesDict;
	// }
	

}
