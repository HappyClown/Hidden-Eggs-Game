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
	public List<BeachClam> currentClams;
	void Start(){
		myLvls[currentLevel -1].gameObject.SetActive(true);
		myLvls[currentLevel -1].SetUpLevel();
	}
	void Update()
	{
		if(myInput.Tapped){
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
			Vector2 mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if(hit){
				Debug.Log(hit.collider.gameObject.name);
				if(hit.collider.CompareTag("Puzzle")){
					if(openedClams.Count > 0){
						openedClams[0].forceClose = true;
						openedClams[1].forceClose = true;
						openedClams.Clear();
					}
					BeachClam tappedClam = hit.collider.gameObject.GetComponent<BeachClam>();
					currentClams.Add(tappedClam);
					tappedClam.Tapped = true;
					if (currentClams.Count == 2)
					{
						if(tappedClam.myMatch.open){
							myLvls[currentLevel -1].CheckClams();
							currentClams.Clear();
						}
						else{
							currentClams[0].failed = true;
							currentClams[1].failed = true;
							openedClams.Add(currentClams[0]);
							openedClams.Add(currentClams[1]);
							currentClams.Clear();
						}

					}
				}
			}
		}
	}
}
