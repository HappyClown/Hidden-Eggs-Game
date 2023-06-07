using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ToyStoreCellChecker : MonoBehaviour{
    public PuzzleCell pieceCell, gridCell;
    public bool cellFits = false; 
    public ToyStoreCellChecker(PuzzleCell cell1, PuzzleCell cell2, bool fit){
        pieceCell=cell1;
        gridCell = cell2;
        cellFits = fit;
    }
}