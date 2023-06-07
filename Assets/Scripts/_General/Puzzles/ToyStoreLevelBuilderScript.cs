using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToyStoreLevelBuilderScript : MonoBehaviour
{
    public bool selecting = false;
    public ToyStorePuzzleLevel myLevel;
    public Color selectedColor = Color.red, removedColor = Color.white;   


    public void StartSelecting(bool select){
        if(select){            
            myLevel = this.gameObject.GetComponent<ToyStorePuzzleLevel>();
            for (int i = 0; i < myLevel.goalCells.Count; i++)
            {
                myLevel.goalCells[i].gameObject.GetComponent<SpriteRenderer>().color = selectedColor    ;
            }
        }else{
            for (int i = 0; i < myLevel.goalCells.Count; i++)
            {
                myLevel.goalCells[i].gameObject.GetComponent<SpriteRenderer>().color = removedColor;
            }
            myLevel = null;
        }
        selecting = select;
    }
    public void AddSelectedCell(GameObject selObject){
        if(selecting){
            PuzzleCell selectedCell = selObject.GetComponent<PuzzleCell>();
            if(selectedCell && selObject.tag == "Tile"){                
                bool addNewCell = true;
                for (int i = 0; i < myLevel.goalCells.Count; i++)
                {
                    if(myLevel.goalCells[i].gameObject.name == selObject.name){
                        myLevel.goalCells.Remove(myLevel.goalCells[i]);
                        selObject.GetComponent<SpriteRenderer>().color = removedColor;
                        addNewCell = false;
                    }
                }
                if(addNewCell){
                    myLevel.goalCells.Add(selectedCell);
                    selObject.GetComponent<SpriteRenderer>().color = selectedColor;
                }                
            }
        }
    }
    public void EmptyGoals(){
        for (int i = 0; i < myLevel.goalCells.Count; i++)
        {
            myLevel.goalCells[i].gameObject.GetComponent<SpriteRenderer>().color = removedColor;
        }
        myLevel.goalCells.Clear();
    }
}
