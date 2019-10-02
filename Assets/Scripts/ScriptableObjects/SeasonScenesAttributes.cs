using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects Scene", menuName = "SceneAttributes")]
public class SeasonScenesAttributes : ScriptableObject {
    public string seasonName;
    public string[] sceneNames;
    public ParticleSystem[] cadreParticles;
    public Sprite cadre;
}
