using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonCadre : MonoBehaviour {
	// public List<ParticleSystem> summerCadreParticles, fallCadreParticles, winterCadreParticles, springCadreParticles;
	// public List<ParticleSystem> cadreParticles;
	// //private string seasonName;
	// public Dictionary<string, List<ParticleSystem>> sceneDict = new Dictionary<string, List<ParticleSystem>>();
	// //public GlobalVariables globoVaro;
	// public void Start () {
	// 	CreateSceneDict();
	// }
	// public void CreateSceneDict() {
	// 	//sceneDict.Clear();
	// 	sceneDict.Add(GlobalVariables.globVarScript.marketName, summerCadreParticles);
	// 	sceneDict.Add(GlobalVariables.globVarScript.marketPuzName, summerCadreParticles);
	// 	sceneDict.Add(GlobalVariables.globVarScript.parkName, summerCadreParticles);
	// 	sceneDict.Add(GlobalVariables.globVarScript.parkPuzName, summerCadreParticles);
	// 	sceneDict.Add(GlobalVariables.globVarScript.beachName, summerCadreParticles);
	// 	sceneDict.Add(GlobalVariables.globVarScript.beachPuzName, summerCadreParticles);
	// }
	// public List<ParticleSystem> GetCadreParticles(string scene) {
	// 	// if (sceneDict[scene] == null) {
	// 	// 	CreateSceneDict();
	// 	// }
	// 	sceneDict.Clear();
	// 	CreateSceneDict();
	// 	foreach (string key in sceneDict.Keys)
	// 	{
	// 		List<ParticleSystem> val = sceneDict[key];
	// 		Debug.Log(key + " = " + val);
	// 	}
	// 	//List<ParticleSystem> chosenParts = new List<ParticleSystem>();
	// 	// Debug.Log("Should be the Dict: " + sceneDict);
	// 	// Debug.Log("The scene being fed in: " + scene);
	// 	cadreParticles.Clear();
	// 	cadreParticles = sceneDict[scene];
	// 	//chosenParts = sceneDict[scene];
	// 	return cadreParticles;
	// }

	public string[] sceneNames;
	public Sprite cadreSprite;
	public ParticleSystem[] cadreParticles;
}
