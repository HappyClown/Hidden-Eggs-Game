using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ToyStorePiece", order = 1)]
public class ToyStorePieceData : ScriptableObject
{
    public float pieceWeight;
    public GameObject piecePrefab;
    public bool inGame;
    public int type,version;
    public Vector3 spotPos;
}
