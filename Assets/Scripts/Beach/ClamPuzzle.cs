using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClamPuzzle : MonoBehaviour 
{

	private Ray2D ray;
	private RaycastHit2D hit;
	public inputDetector myInput;
	public int currentLevel;
	public BeachClamLevel[] myLvls;
	public List<BeachClam> openedClams;
	void Start(){
		myLvls[currentLevel -1].SetUpLevel();
	}
	void Update()
	{
		if(myInput.Tapped){
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
			Vector2 mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			Debug.Log(hit.collider.gameObject.name);
			if(hit){
				if(hit.collider.CompareTag("Puzzle")){
					BeachClam tappedClam = hit.collider.gameObject.GetComponent<BeachClam>();
					openedClams.Add(tappedClam);
					tappedClam.Tapped = true;
					if (openedClams.Count == 2)
					{
						if(tappedClam.myMatch.open){
							myLvls[currentLevel -1].CheckClams();
							openedClams.Clear();
						}
						else{
							openedClams[0].failed = true;
							openedClams[1].failed = true;
							openedClams.Clear();
						}

					}
				}
			}
		}
	}
}
